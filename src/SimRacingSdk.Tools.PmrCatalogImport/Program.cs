using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

// Usage:
//   dotnet run --project SimRacingSdk.Tools.PmrCatalogImport
//     Reads pmr-cars.csv / pmr-tracks.csv (repo root, hand-curated - the source of truth) and
//     writes PmrCarInfoProvider.generated.cs / PmrTrackInfoProvider.generated.cs next to the
//     repo root for review.
//   dotnet run --project SimRacingSdk.Tools.PmrCatalogImport -- --write
//     Overwrites PmrCarInfoProvider.cs / PmrTrackInfoProvider.cs directly.
//   dotnet run --project SimRacingSdk.Tools.PmrCatalogImport -- --seed
//     Parses the game's own install data (data/vehicles/*/data/*.vdef,
//     data/tracks/*/data/*.tdef + data/tracks/*/data/layouts/*.xml) and appends any car/track
//     not already present in pmr-cars.csv/pmr-tracks.csv (matched by Manufacturer+Name for cars,
//     TrackName+LayoutName for tracks). Never touches or removes an existing row, so it's safe
//     to re-run after a game update to pick up anything new. Confirmed 2026-09-17 that the
//     install's own files are NOT a complete car/track list (e.g. the Plymouth Barracuda is
//     selectable in-game with no .vdef anywhere on disk) - seeding only gets a starting point,
//     the rest has to be added by hand after checking the game itself.
//   dotnet run --project SimRacingSdk.Tools.PmrCatalogImport -- --seed --installPath="D:\Games\Project Motor Racing"
//     --seed against a non-default install location.
//
// pmr-cars.csv / pmr-tracks.csv (repo root) are the source of truth - a deliberately trimmed
// column set: cars carry only what's visible in the game's own car-select screen (Manufacturer,
// Name, VehicleClass, PowerKw, TorqueNm, WeightKg, EngineDisplacementLitres, EngineType,
// EngineLayout, PowertrainLayout, Transmission) plus FuelCapacityLitres (kept deliberately - see
// the Virtual Energy note below); tracks add Latitude/Longitude/AltitudeMetersAmsl and
// Country/Continent on top of what's visible (TrackName, LayoutName, Turns, GridSize,
// LengthMeters) - all four researched/typed by hand the same way Ace's track Corners column was
// (Mike's call, 2026-09-17). Both CSVs also carry a trailing GameId column - the game's own id
// (vehicle .vdef / track .tdef "ID", or savegame1\stats.xml for cars and tracks not present as
// loose files), which a saved setup's .vset file name is keyed on. A track's GameId is shared by
// all its layouts. PmrCarInfo.Id and PmrTrackInfo.TrackId/LayoutId are still derived at
// generation time from the name fields.
//
// --seed splits the install's single combined EngineName ("2.4L V6 Twin Turbo") and Layout
// ("Mid Engine - RWD") strings into the CSV's separate columns, and converts PowerBHP to
// PowerKw, since the game's own UI shows these as distinct facts, not one combined string, and
// in kW not bhp.

var repoRoot = FindRepoRoot();
var write = args.Contains("--write");
var seed = args.Contains("--seed");
var carsCsvPath = Path.Combine(repoRoot, "pmr-cars.csv");
var tracksCsvPath = Path.Combine(repoRoot, "pmr-tracks.csv");
var warnings = new List<string>();

if (seed)
{
    var installPath = ResolveInstallPath(args);
    if (installPath is null)
    {
        Console.WriteLine("Could not find a Project Motor Racing install. Pass --installPath=\"<path>\" explicitly.");
        return 1;
    }

    var vehiclesRoot = Path.Combine(installPath, "data", "vehicles");
    var tracksRoot = Path.Combine(installPath, "data", "tracks");

    var addedCars = SeedCars(carsCsvPath, ParseInstallCars(vehiclesRoot, warnings), warnings);
    var addedTracks = SeedTracks(tracksCsvPath, ParseInstallTracks(tracksRoot, warnings), warnings);

    Console.WriteLine($"Added {addedCars} new car row(s) to {carsCsvPath}");
    Console.WriteLine($"Added {addedTracks} new track/layout row(s) to {tracksCsvPath}");
    foreach (var warning in warnings)
    {
        Console.WriteLine($"WARNING: {warning}");
    }

    return 0;
}

if (!File.Exists(carsCsvPath) || !File.Exists(tracksCsvPath))
{
    Console.WriteLine(
        $"{carsCsvPath} / {tracksCsvPath} not found. Run with --seed first to create them from an install, "
        + "or create them by hand.");
    return 1;
}

var carRows = LoadCarsCsv(carsCsvPath, warnings);
var trackRows = LoadTracksCsv(tracksCsvPath, warnings);
ValidateCarGameIds(carRows, warnings);
ValidateTrackGameIds(trackRows, warnings);

var carProviderPath = Path.Combine(repoRoot, "src", "SimRacingSdk.Pmr.Core", "PmrCarInfoProvider.cs");
var trackProviderPath = Path.Combine(repoRoot, "src", "SimRacingSdk.Pmr.Core", "PmrTrackInfoProvider.cs");
var carOutputPath = write ? carProviderPath : Path.Combine(repoRoot, "PmrCarInfoProvider.generated.cs");
var trackOutputPath = write ? trackProviderPath : Path.Combine(repoRoot, "PmrTrackInfoProvider.generated.cs");

File.WriteAllText(carOutputPath, BuildCarProviderSource(carRows, warnings));
File.WriteAllText(trackOutputPath, BuildTrackProviderSource(trackRows, warnings));

Console.WriteLine($"Wrote {carRows.Count} car entries to {carOutputPath}");
Console.WriteLine($"Wrote {trackRows.Count} track/layout entries to {trackOutputPath}");

var noFuelData = carRows.Where(c => c.FuelCapacityLitres is null)
                         .ToList();
if (noFuelData.Count > 0)
{
    Console.WriteLine(
        $"{noFuelData.Count} cars have no fuel-capacity data - consistent with Hypercar/LMDh's regulated energy "
        + "allocation rather than a fixed tank size, but worth a glance if an unexpected car shows up here:");
    foreach (var car in noFuelData)
    {
        Console.WriteLine($"  {car.Manufacturer} {car.Name} ({car.VehicleClass})");
    }
}

if (!write)
{
    Console.WriteLine("Dry run - review the output, then pass --write to overwrite the real provider files.");
}

foreach (var warning in warnings)
{
    Console.WriteLine($"WARNING: {warning}");
}

return 0;

static string FindRepoRoot()
{
    var dir = new DirectoryInfo(AppContext.BaseDirectory);
    while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "CLAUDE.md")))
    {
        dir = dir.Parent;
    }

    return dir?.FullName
           ?? throw new InvalidOperationException(
               "Could not find repo root (walked up from AppContext.BaseDirectory looking for CLAUDE.md).");
}

static string? ResolveInstallPath(string[] cliArgs)
{
    var explicitPath = cliArgs.FirstOrDefault(a => a.StartsWith("--installPath=", StringComparison.OrdinalIgnoreCase));
    if (explicitPath is not null)
    {
        var path = explicitPath["--installPath=".Length..].Trim('"');
        return Directory.Exists(path) ? path : null;
    }

    string[] defaultCandidates =
    [
        @"C:\Program Files (x86)\Steam\steamapps\common\Project Motor Racing",
        @"C:\Program Files\Steam\steamapps\common\Project Motor Racing"
    ];

    return defaultCandidates.FirstOrDefault(Directory.Exists);
}

// A stable, human-readable slug derived from the name fields at generation time - distinct from
// the game's own GameId, which is stored in the CSVs.
static string Slugify(string value)
{
    return Regex.Replace(value.Trim(), @"\s+", "_");
}

static XElement LoadParameters(string path)
{
    try
    {
        // Every .vset file closes its root with "</table name=".." type=".."/>" - not valid
        // XML (attributes on a closing tag), but it's the format the game itself writes, so
        // its own loader must be lenient about it. Strip the attributes back off before parsing.
        var xml = Regex.Replace(File.ReadAllText(path), "</table[^>]*>", "</table>");
        var document = XDocument.Parse(xml);
        return document.Root!.Element("parameters")!;
    }
    catch (Exception exception)
    {
        throw new InvalidOperationException($"Failed to parse {path}: {exception.Message}", exception);
    }
}

static string? GetRawValue(XElement parameters, string elementName, string name)
{
    return parameters.Elements(elementName)
                      .FirstOrDefault(p => (string?)p.Attribute("name") == name)
                      ?.Attribute("value")
                      ?.Value;
}

static string GetString(XElement parameters, string name, string defaultValue = "")
{
    return GetRawValue(parameters, "parameter", name) ?? defaultValue;
}

static double GetNumber(XElement parameters, string name, double defaultValue = 0)
{
    var raw = GetRawValue(parameters, "parameter", name);
    return raw is not null && double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
        ? value
        : defaultValue;
}

static int GetInt(XElement parameters, string name, int defaultValue = 0)
{
    return (int)GetNumber(parameters, name, defaultValue);
}

static bool GetBool(XElement parameters, string name, bool defaultValue = false)
{
    var raw = GetRawValue(parameters, "parameter", name);
    return raw is not null ? raw.Equals("true", StringComparison.OrdinalIgnoreCase) : defaultValue;
}

static double? GetVsetNumber(string vehicleDataFolder, string name)
{
    var vsetPath = Path.Combine(vehicleDataFolder, "default.vset");
    if (!File.Exists(vsetPath))
    {
        return null;
    }

    var parameters = LoadParameters(vsetPath);
    var raw = GetRawValue(parameters, "param", name);
    return raw is not null && double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
        ? value
        : null;
}

// Manufacturers that trade under more than one name in the game's own data but share a single
// logo asset in SimRacingSdk.Wpf.Shared/Images/Manufacturers - "Mercedes-AMG" and
// "Mercedes-Benz" both resolve to the one generic "mercedes.png". Anything not listed here uses
// its own Manufacturer value unchanged.
static Dictionary<string, string> BuildManufacturerIconLookup()
{
    return new Dictionary<string, string>
    {
        ["Mercedes-AMG"] = "Mercedes",
        ["Mercedes-Benz"] = "Mercedes"
    };
}

static string ResolveIconFileName(string manufacturer)
{
    return BuildManufacturerIconLookup().TryGetValue(manufacturer, out var iconFileName)
        ? iconFileName
        : manufacturer;
}

static double BhpToKw(double bhp)
{
    // 1 bhp = 0.745699872 kW (imperial horsepower - the vdef field is explicitly named PowerBHP).
    // Rounded to a whole kW to match how the game's own UI displays it, and how Mike will type it.
    return Math.Round(bhp * 0.745699872);
}

// Splits the vdef's single combined "2.4L V6 Twin Turbo" style string into displacement and
// cylinder configuration, dropping the forced-induction suffix ("Turbo"/"Twin Turbo") - not on
// Mike's visible-in-game field list, so out of scope for this schema.
static (double DisplacementLitres, string EngineType) SplitEngineName(string engineName, List<string> warnings, string context)
{
    var match = Regex.Match(
        engineName,
        @"^(?<displacement>\d+(\.\d+)?)L\s+(?<type>.+?)(?:\s+(?:Twin\s+)?Turbo)?$");
    if (!match.Success)
    {
        warnings.Add($"Could not split EngineName \"{engineName}\" ({context}) into displacement/type.");
        return (0, engineName);
    }

    return (double.Parse(match.Groups["displacement"].Value, CultureInfo.InvariantCulture), match.Groups["type"].Value);
}

// Splits the vdef's single combined "Mid Engine - RWD" style string into engine position and
// drivetrain - two distinct facts on the game's own car-select screen, not one combined string.
static (string EngineLayout, string PowertrainLayout) SplitLayout(string layout, List<string> warnings, string context)
{
    var parts = layout.Split(" - ", 2);
    if (parts.Length != 2)
    {
        warnings.Add($"Could not split Layout \"{layout}\" ({context}) into engine/powertrain layout.");
        return (layout, "");
    }

    return (parts[0], parts[1]);
}

static List<CarSpecRow> ParseInstallCars(string vehiclesRoot, List<string> warnings)
{
    var rows = new List<CarSpecRow>();
    foreach (var vdefPath in Directory.GetFiles(vehiclesRoot, "*.vdef", SearchOption.AllDirectories))
    {
        var parameters = LoadParameters(vdefPath);
        var id = GetString(parameters, "ID");
        if (string.IsNullOrEmpty(id))
        {
            warnings.Add($"Skipped vdef with no ID: {vdefPath}");
            continue;
        }

        if (!GetBool(parameters, "Active"))
        {
            continue;
        }

        var fuelFromVdef = GetNumber(parameters, "FuelCapacityLitres");
        var fuelCapacityLitres = fuelFromVdef > 0
            ? fuelFromVdef
            : GetVsetNumber(Path.GetDirectoryName(vdefPath)!, "ice-fuel-capacity");

        var (displacementLitres, engineType) = SplitEngineName(GetString(parameters, "EngineName"), warnings, id);
        var (engineLayout, powertrainLayout) = SplitLayout(GetString(parameters, "Layout"), warnings, id);

        rows.Add(new CarSpecRow(
            GetString(parameters, "Manufacturer"),
            GetString(parameters, "Name"),
            GetString(parameters, "VehicleClass"),
            BhpToKw(GetNumber(parameters, "PowerBHP")),
            GetNumber(parameters, "TorqueNM"),
            GetNumber(parameters, "WeightKG"),
            displacementLitres,
            engineType,
            engineLayout,
            powertrainLayout,
            GetString(parameters, "Transmission"),
            fuelCapacityLitres,
            GetInt(parameters, "Year"),
            id));
    }

    return rows.OrderBy(c => c.Manufacturer, StringComparer.OrdinalIgnoreCase)
               .ThenBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
               .ToList();
}

// The game's own Country strings are inconsistently cased/snake_cased (e.g. "united_states",
// "South_Africa", both "United_Kingdom" and "united_kingdom" across different tracks) - keyed
// here with underscores normalised to spaces and lower-cased so all variants resolve to one
// clean display name plus the ISO 3166-1 alpha-3 code the flag PNGs are named with. Idempotent
// against an already-clean CSV value too (e.g. "Italy" normalises to the same "italy" lookup
// key), so this doubles as the CSV's own Country -> CountryCode resolver.
static Dictionary<string, (string DisplayName, string Iso3Code)> BuildCountryLookup()
{
    return new Dictionary<string, (string, string)>
    {
        ["australia"] = ("Australia", "AUS"),
        ["austria"] = ("Austria", "AUT"),
        ["belgium"] = ("Belgium", "BEL"),
        ["brazil"] = ("Brazil", "BRA"),
        ["canada"] = ("Canada", "CAN"),
        ["germany"] = ("Germany", "DEU"),
        ["italy"] = ("Italy", "ITA"),
        ["japan"] = ("Japan", "JPN"),
        ["south africa"] = ("South Africa", "ZAF"),
        ["united kingdom"] = ("United Kingdom", "GBR"),
        ["united states"] = ("United States", "USA")
    };
}

static (string Country, string CountryCode) ResolveCountry(string rawCountry, List<string> warnings, string context)
{
    var key = rawCountry.Replace('_', ' ')
                         .Trim()
                         .ToLowerInvariant();
    if (BuildCountryLookup().TryGetValue(key, out var match))
    {
        return (match.DisplayName, match.Iso3Code);
    }

    warnings.Add($"No country-code mapping for \"{rawCountry}\" ({context}) - add it to BuildCountryLookup.");
    return (rawCountry, "");
}

// The game's own tdef files use "Other" for its "Rest of World" grouping (confirmed against
// the in-game track selector's four groups: All, Americas, Europe, Rest of World - "All" is
// just a UI meta-filter, not a real Continent value). Europe/Americas pass through unchanged.
static string ResolveContinent(string rawContinent)
{
    return rawContinent.Equals("Other", StringComparison.OrdinalIgnoreCase) ? "Rest of World" : rawContinent;
}

static List<TrackSpecRow> ParseInstallTracks(string tracksRoot, List<string> warnings)
{
    var rows = new List<TrackSpecRow>();
    foreach (var trackFolder in Directory.GetDirectories(tracksRoot))
    {
        var dataFolder = Path.Combine(trackFolder, "data");
        if (!Directory.Exists(dataFolder))
        {
            continue;
        }

        var tdefFiles = Directory.GetFiles(dataFolder, "*.tdef");
        if (tdefFiles.Length == 0)
        {
            continue;
        }

        var tdefsById = new Dictionary<string, XElement>();
        foreach (var tdefFile in tdefFiles)
        {
            var parameters = LoadParameters(tdefFile);
            var id = GetString(parameters, "ID");
            if (!string.IsNullOrEmpty(id))
            {
                tdefsById[id] = parameters;
            }
        }

        var layoutsFolder = Path.Combine(dataFolder, "layouts");
        if (!Directory.Exists(layoutsFolder))
        {
            continue;
        }

        // Utility/UI backdrop folders (car_selection, frontend_track) also carry a .tdef but
        // ship zero layout xml files, so they fall out naturally here without a hardcoded
        // exclude list.
        foreach (var layoutFile in Directory.GetFiles(layoutsFolder, "*.xml"))
        {
            var layoutParameters = LoadParameters(layoutFile);

            // Only present when a folder holds more than one venue (e.g. Nurburg_full holds
            // both the short Nordschleife and the 24h combined layout, each its own .tdef) -
            // absent, single-tdef folders are the norm.
            var explicitTrackId = GetRawValue(layoutParameters, "parameter", "TRACK_ID");
            var trackId = explicitTrackId ?? (tdefsById.Count == 1 ? tdefsById.Keys.Single() : null);
            if (trackId is null || !tdefsById.TryGetValue(trackId, out var trackParameters))
            {
                warnings.Add($"Could not resolve owning track for layout file: {layoutFile}");
                continue;
            }

            var (country, _) = ResolveCountry(GetString(trackParameters, "Country"), warnings, trackId);

            rows.Add(new TrackSpecRow(
                GetString(trackParameters, "Name"),
                GetString(layoutParameters, "Name"),
                country,
                ResolveContinent(GetString(trackParameters, "Continent")),
                Math.Round(GetNumber(layoutParameters, "Length") * 1000), // the game's own layout xml stores this in km
                GetInt(layoutParameters, "Turns"),
                GetInt(layoutParameters, "GridSize"),
                GetNumber(trackParameters, "TrackLatitude"),
                GetNumber(trackParameters, "TrackLongitude"),
                GetNumber(trackParameters, "TrackAltitudeMetresAMSL"),
                trackId));
        }
    }

    return rows.OrderBy(t => t.TrackName, StringComparer.OrdinalIgnoreCase)
               .ThenBy(t => t.LayoutName, StringComparer.OrdinalIgnoreCase)
               .ToList();
}

static (string[] Headers, List<string[]> Rows) ParseCsv(string csvPath)
{
    var lines = ReadAllLinesShared(csvPath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
    if (lines.Length == 0)
    {
        return (Array.Empty<string>(), []);
    }

    var headers = ParseCsvLine(lines[0]);
    var rows = lines.Skip(1).Select(ParseCsvLine).ToList();
    return (headers, rows);
}

// Plain File.ReadAllLines can fail with a sharing violation while the CSV is open in Excel -
// FileShare.ReadWrite tolerates that, matching AceCarCatalogImport's own ReadAllLinesShared.
static string[] ReadAllLinesShared(string path)
{
    using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    using var reader = new StreamReader(stream);
    var lines = new List<string>();
    while (reader.ReadLine() is { } line)
    {
        lines.Add(line);
    }

    return lines.ToArray();
}

static string[] ParseCsvLine(string line)
{
    var fields = new List<string>();
    var current = new StringBuilder();
    var inQuotes = false;

    for (var i = 0; i < line.Length; i++)
    {
        var c = line[i];
        if (inQuotes)
        {
            if (c == '"')
            {
                if (i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = false;
                }
            }
            else
            {
                current.Append(c);
            }
        }
        else if (c == '"')
        {
            inQuotes = true;
        }
        else if (c == ',')
        {
            fields.Add(current.ToString());
            current.Clear();
        }
        else
        {
            current.Append(c);
        }
    }

    fields.Add(current.ToString());
    return fields.ToArray();
}

static Dictionary<string, int> BuildColumnIndex(string[] headers)
{
    var index = new Dictionary<string, int>();
    for (var i = 0; i < headers.Length; i++)
    {
        index[headers[i].Trim()] = i;
    }

    return index;
}

static string GetCell(string[] row, Dictionary<string, int> columnIndex, string columnName)
{
    return columnIndex.TryGetValue(columnName, out var i) && i < row.Length ? row[i].Trim() : "";
}

static string CsvCell(string value)
{
    return value.Contains(',') || value.Contains('"') || value.Contains('\n')
        ? $"\"{value.Replace("\"", "\"\"")}\""
        : value;
}

static List<CarSpecRow> LoadCarsCsv(string csvPath, List<string> warnings)
{
    var (headers, rows) = ParseCsv(csvPath);
    var columnIndex = BuildColumnIndex(headers);
    var result = new List<CarSpecRow>();

    foreach (var row in rows)
    {
        string Cell(string columnName) => GetCell(row, columnIndex, columnName);

        double? CellNullableNumber(string columnName)
        {
            var raw = Cell(columnName);
            return !string.IsNullOrEmpty(raw)
                && double.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
                ? value
                : null;
        }

        double CellNumber(string columnName) => CellNullableNumber(columnName) ?? 0;

        var manufacturer = Cell("Manufacturer");
        var name = Cell("Name");
        if (string.IsNullOrEmpty(manufacturer) || string.IsNullOrEmpty(name))
        {
            warnings.Add($"Skipped pmr-cars.csv row with missing Manufacturer/Name: {string.Join(",", row)}");
            continue;
        }

        result.Add(new CarSpecRow(
            manufacturer,
            name,
            Cell("VehicleClass"),
            CellNumber("PowerKw"),
            CellNumber("TorqueNm"),
            CellNumber("WeightKg"),
            CellNumber("EngineDisplacementLitres"),
            Cell("EngineType"),
            Cell("EngineLayout"),
            Cell("PowertrainLayout"),
            Cell("Transmission"),
            CellNullableNumber("FuelCapacityLitres"),
            (int)CellNumber("Year"),
            Cell("GameId")));
    }

    return result.OrderBy(c => c.Manufacturer, StringComparer.OrdinalIgnoreCase)
                 .ThenBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
                 .ToList();
}

static List<TrackSpecRow> LoadTracksCsv(string csvPath, List<string> warnings)
{
    var (headers, rows) = ParseCsv(csvPath);
    var columnIndex = BuildColumnIndex(headers);
    var result = new List<TrackSpecRow>();

    foreach (var row in rows)
    {
        string Cell(string columnName) => GetCell(row, columnIndex, columnName);
        double CellNumber(string columnName) =>
            double.TryParse(Cell(columnName), NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
                ? value
                : 0;

        var trackName = Cell("TrackName");
        var layoutName = Cell("LayoutName");
        if (string.IsNullOrEmpty(trackName) || string.IsNullOrEmpty(layoutName))
        {
            warnings.Add($"Skipped pmr-tracks.csv row with missing TrackName/LayoutName: {string.Join(",", row)}");
            continue;
        }

        result.Add(new TrackSpecRow(
            trackName,
            layoutName,
            Cell("Country"),
            Cell("Continent"),
            CellNumber("LengthMeters"),
            (int)CellNumber("Turns"),
            (int)CellNumber("GridSize"),
            CellNumber("Latitude"),
            CellNumber("Longitude"),
            CellNumber("AltitudeMetersAmsl"),
            Cell("GameId")));
    }

    return result.OrderBy(t => t.TrackName, StringComparer.OrdinalIgnoreCase)
                 .ThenBy(t => t.LayoutName, StringComparer.OrdinalIgnoreCase)
                 .ToList();
}

static void SaveCarsCsv(string csvPath, List<CarSpecRow> cars)
{
    var lines = new List<string>
    {
        "Manufacturer,Name,VehicleClass,PowerKw,TorqueNm,WeightKg,EngineDisplacementLitres,EngineType,EngineLayout,PowertrainLayout,Transmission,FuelCapacityLitres,Year,GameId"
    };

    lines.AddRange(cars.Select(c => string.Join(",",
        CsvCell(c.Manufacturer),
        CsvCell(c.Name),
        CsvCell(c.VehicleClass),
        c.PowerKw.ToString(CultureInfo.InvariantCulture),
        c.TorqueNm.ToString(CultureInfo.InvariantCulture),
        c.WeightKg.ToString(CultureInfo.InvariantCulture),
        c.EngineDisplacementLitres.ToString(CultureInfo.InvariantCulture),
        CsvCell(c.EngineType),
        CsvCell(c.EngineLayout),
        CsvCell(c.PowertrainLayout),
        CsvCell(c.Transmission),
        c.FuelCapacityLitres?.ToString(CultureInfo.InvariantCulture) ?? "",
        c.Year.ToString(CultureInfo.InvariantCulture),
        CsvCell(c.GameId))));

    File.WriteAllLines(csvPath, lines);
}

static void SaveTracksCsv(string csvPath, List<TrackSpecRow> tracks)
{
    var lines = new List<string>
    {
        "TrackName,LayoutName,Country,Continent,LengthMeters,Turns,GridSize,Latitude,Longitude,AltitudeMetersAmsl,GameId"
    };

    lines.AddRange(tracks.Select(t => string.Join(",",
        CsvCell(t.TrackName),
        CsvCell(t.LayoutName),
        CsvCell(t.Country),
        CsvCell(t.Continent),
        t.LengthMeters.ToString(CultureInfo.InvariantCulture),
        t.Turns.ToString(CultureInfo.InvariantCulture),
        t.GridSize.ToString(CultureInfo.InvariantCulture),
        t.Latitude.ToString(CultureInfo.InvariantCulture),
        t.Longitude.ToString(CultureInfo.InvariantCulture),
        t.AltitudeMetersAmsl.ToString(CultureInfo.InvariantCulture),
        CsvCell(t.GameId))));

    File.WriteAllLines(csvPath, lines);
}

static string CarKey(CarSpecRow car)
{
    return $"{car.Manufacturer}|{car.Name}|{car.Year}";
}

static string TrackKey(TrackSpecRow track)
{
    return $"{track.TrackName}|{track.LayoutName}";
}

static int SeedCars(string csvPath, List<CarSpecRow> installCars, List<string> warnings)
{
    var existing = File.Exists(csvPath) ? LoadCarsCsv(csvPath, warnings) : [];
    var existingKeys = existing.Select(CarKey).ToHashSet(StringComparer.OrdinalIgnoreCase);
    var added = installCars.Where(c => !existingKeys.Contains(CarKey(c))).ToList();

    var merged = existing.Concat(added)
                          .OrderBy(c => c.Manufacturer, StringComparer.OrdinalIgnoreCase)
                          .ThenBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
                          .ToList();

    SaveCarsCsv(csvPath, merged);
    return added.Count;
}

static int SeedTracks(string csvPath, List<TrackSpecRow> installTracks, List<string> warnings)
{
    var existing = File.Exists(csvPath) ? LoadTracksCsv(csvPath, warnings) : [];
    var existingKeys = existing.Select(TrackKey).ToHashSet(StringComparer.OrdinalIgnoreCase);
    var added = installTracks.Where(t => !existingKeys.Contains(TrackKey(t))).ToList();

    var merged = existing.Concat(added)
                          .OrderBy(t => t.TrackName, StringComparer.OrdinalIgnoreCase)
                          .ThenBy(t => t.LayoutName, StringComparer.OrdinalIgnoreCase)
                          .ToList();

    SaveTracksCsv(csvPath, merged);
    return added.Count;
}

static void ValidateCarGameIds(List<CarSpecRow> cars, List<string> warnings)
{
    foreach (var car in cars.Where(c => string.IsNullOrEmpty(c.GameId)))
    {
        warnings.Add($"Car has no GameId: {car.Manufacturer} {car.Name} ({car.Year}).");
    }

    var duplicates = cars.Where(c => !string.IsNullOrEmpty(c.GameId))
                         .GroupBy(c => c.GameId, StringComparer.OrdinalIgnoreCase)
                         .Where(g => g.Count() > 1);
    foreach (var duplicate in duplicates)
    {
        warnings.Add(
            $"GameId \"{duplicate.Key}\" is shared by more than one car: "
            + string.Join("; ", duplicate.Select(c => $"{c.Manufacturer} {c.Name} ({c.Year})")));
    }
}

static void ValidateTrackGameIds(List<TrackSpecRow> tracks, List<string> warnings)
{
    foreach (var track in tracks.Where(t => string.IsNullOrEmpty(t.GameId)))
    {
        warnings.Add($"Track layout has no GameId: {track.TrackName} - {track.LayoutName}.");
    }

    var namesByGameId = tracks.Where(t => !string.IsNullOrEmpty(t.GameId))
                              .GroupBy(t => t.GameId, StringComparer.OrdinalIgnoreCase);
    foreach (var group in namesByGameId)
    {
        var trackNames = group.Select(t => t.TrackName)
                              .Distinct()
                              .ToList();
        if (trackNames.Count > 1)
        {
            warnings.Add($"GameId \"{group.Key}\" is shared by more than one track: {string.Join("; ", trackNames)}");
        }
    }

    var gameIdsByTrackName = tracks.Where(t => !string.IsNullOrEmpty(t.GameId))
                                   .GroupBy(t => t.TrackName);
    foreach (var group in gameIdsByTrackName)
    {
        var gameIds = group.Select(t => t.GameId)
                           .Distinct(StringComparer.OrdinalIgnoreCase)
                           .ToList();
        if (gameIds.Count > 1)
        {
            warnings.Add($"Track \"{group.Key}\" has layouts with different GameIds: {string.Join("; ", gameIds)}");
        }
    }
}

static string CsString(string value)
{
    return $"\"{value.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";
}

static string CsNumber(double value)
{
    return value.ToString(CultureInfo.InvariantCulture);
}

static string CsNullableNumber(double? value)
{
    return value.HasValue ? CsNumber(value.Value) : "null";
}

static string BuildCarProviderSource(List<CarSpecRow> cars, List<string> warnings)
{
    var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    var entryLines = cars.Select(c =>
    {
        // Includes Year - Manufacturer+Name alone collides for cars the game lists twice across
        // eras (e.g. a 2024 Chevrolet Camaro and a 1969 "Historic USV8" Chevrolet Camaro).
        var id = Slugify($"{c.Manufacturer} {c.Name} {c.Year}");
        if (!seenIds.Add(id))
        {
            warnings.Add($"Duplicate derived car Id \"{id}\" ({c.Manufacturer} {c.Name}) - rename one of the rows.");
        }

        return "        new() { "
            + $"EngineDisplacementLitres = {CsNumber(c.EngineDisplacementLitres)}, "
            + $"EngineLayout = {CsString(c.EngineLayout)}, "
            + $"EngineType = {CsString(c.EngineType)}, "
            + $"FuelCapacityLitres = {CsNullableNumber(c.FuelCapacityLitres)}, "
            + $"GameId = {CsString(c.GameId)}, "
            + $"IconFileName = {CsString(ResolveIconFileName(c.Manufacturer))}, "
            + $"Id = {CsString(id)}, "
            + $"Manufacturer = {CsString(c.Manufacturer)}, "
            + $"Name = {CsString(c.Name)}, "
            + $"PowerKw = {CsNumber(c.PowerKw)}, "
            + $"PowertrainLayout = {CsString(c.PowertrainLayout)}, "
            + $"TorqueNm = {CsNumber(c.TorqueNm)}, "
            + $"Transmission = {CsString(c.Transmission)}, "
            + $"VehicleClass = {CsString(c.VehicleClass)}, "
            + $"WeightKg = {CsNumber(c.WeightKg)}, "
            + $"Year = {c.Year} "
            + "},";
    });

    return $$"""
             using SimRacingSdk.Pmr.Core.Abstractions;
             using SimRacingSdk.Pmr.Core.Models;

             namespace SimRacingSdk.Pmr.Core;

             public class PmrCarInfoProvider : IPmrCarInfoProvider
             {
                 private static PmrCarInfoProvider? singletonInstance;

                 private readonly List<PmrCarInfo> cars =
                 [
             {{string.Join(Environment.NewLine, entryLines)}}
                 ];

                 public static PmrCarInfoProvider Instance => singletonInstance ??= new PmrCarInfoProvider();

                 public PmrCarInfo? FindByGameId(string gameId)
                 {
                     return this.cars.FirstOrDefault(
                         c => string.Equals(c.GameId, gameId, StringComparison.OrdinalIgnoreCase));
                 }

                 public PmrCarInfo? FindById(string id)
                 {
                     return this.cars.FirstOrDefault(c => c.Id == id);
                 }

                 public IReadOnlyCollection<PmrCarInfo> GetCarInfos()
                 {
                     return this.cars.AsReadOnly();
                 }

                 public IReadOnlyCollection<PmrCarInfo> GetCarInfosForManufacturer(string manufacturer)
                 {
                     return this.cars.Where(c => c.Manufacturer == manufacturer)
                                      .ToList()
                                      .AsReadOnly();
                 }

                 public IReadOnlyCollection<string> GetManufacturers()
                 {
                     return this.cars.Select(c => c.Manufacturer)
                                      .Distinct()
                                      .OrderBy(m => m)
                                      .ToList()
                                      .AsReadOnly();
                 }
             }

             """;
}

static string BuildTrackProviderSource(List<TrackSpecRow> tracks, List<string> warnings)
{
    var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    var entryLines = tracks.Select(t =>
    {
        var trackId = Slugify(t.TrackName);
        var layoutId = Slugify(t.LayoutName);
        if (!seenIds.Add($"{trackId}|{layoutId}"))
        {
            warnings.Add(
                $"Duplicate derived track/layout Id \"{trackId}\"/\"{layoutId}\" ({t.TrackName} - {t.LayoutName}) "
                + "- rename one of the rows.");
        }

        var (country, countryCode) = ResolveCountry(t.Country, warnings, trackId);

        return "        new() { "
            + $"AltitudeMetersAmsl = {CsNumber(t.AltitudeMetersAmsl)}, "
            + $"Continent = {CsString(t.Continent)}, "
            + $"Country = {CsString(country)}, "
            + $"CountryCode = {CsString(countryCode)}, "
            + $"GameId = {CsString(t.GameId)}, "
            + $"GridSize = {t.GridSize}, "
            + $"Latitude = {CsNumber(t.Latitude)}, "
            + $"LayoutId = {CsString(layoutId)}, "
            + $"LayoutName = {CsString(t.LayoutName)}, "
            + $"LengthMeters = {CsNumber(t.LengthMeters)}, "
            + $"Longitude = {CsNumber(t.Longitude)}, "
            + $"Turns = {t.Turns}, "
            + $"TrackId = {CsString(trackId)}, "
            + $"TrackName = {CsString(t.TrackName)} "
            + "},";
    });

    return $$"""
             using System.Collections.ObjectModel;
             using SimRacingSdk.Pmr.Core.Abstractions;
             using SimRacingSdk.Pmr.Core.Models;

             namespace SimRacingSdk.Pmr.Core;

             public class PmrTrackInfoProvider : IPmrTrackInfoProvider
             {
                 private static PmrTrackInfoProvider? singletonInstance;

                 private readonly List<PmrTrackInfo> tracks =
                 [
             {{string.Join(Environment.NewLine, entryLines)}}
                 ];

                 public static PmrTrackInfoProvider Instance => singletonInstance ??= new PmrTrackInfoProvider();

                 public PmrTrackInfo? FindByTrackAndLayout(string trackName, string layoutId)
                 {
                     return this.tracks.FirstOrDefault(t => t.TrackName == trackName && t.LayoutId == layoutId);
                 }

                 public ReadOnlyCollection<string> GetContinents()
                 {
                     return this.tracks.Select(t => t.Continent)
                                        .Distinct()
                                        .OrderBy(c => c)
                                        .ToList()
                                        .AsReadOnly();
                 }

                 public ReadOnlyCollection<PmrTrackInfo> GetLayoutsForGameId(string gameId)
                 {
                     return this.tracks.Where(t => string.Equals(t.GameId, gameId, StringComparison.OrdinalIgnoreCase))
                                        .ToList()
                                        .AsReadOnly();
                 }

                 public ReadOnlyCollection<PmrTrackInfo> GetLayoutsForTrack(string trackName)
                 {
                     return this.tracks.Where(t => t.TrackName == trackName)
                                        .ToList()
                                        .AsReadOnly();
                 }

                 public ReadOnlyCollection<PmrTrackInfo> GetTrackInfos()
                 {
                     return this.tracks.AsReadOnly();
                 }

                 public ReadOnlyCollection<string> GetTrackNames()
                 {
                     return this.tracks.Select(t => t.TrackName)
                                        .Distinct()
                                        .OrderBy(n => n)
                                        .ToList()
                                        .AsReadOnly();
                 }

                 public ReadOnlyCollection<string> GetTrackNamesForContinent(string continent)
                 {
                     return this.tracks.Where(t => t.Continent == continent)
                                        .Select(t => t.TrackName)
                                        .Distinct()
                                        .OrderBy(n => n)
                                        .ToList()
                                        .AsReadOnly();
                 }
             }

             """;
}

internal record CarSpecRow(
    string Manufacturer,
    string Name,
    string VehicleClass,
    double PowerKw,
    double TorqueNm,
    double WeightKg,
    double EngineDisplacementLitres,
    string EngineType,
    string EngineLayout,
    string PowertrainLayout,
    string Transmission,
    double? FuelCapacityLitres,
    int Year,
    string GameId);

internal record TrackSpecRow(
    string TrackName,
    string LayoutName,
    string Country,
    string Continent,
    double LengthMeters,
    int Turns,
    int GridSize,
    double Latitude,
    double Longitude,
    double AltitudeMetersAmsl,
    string GameId);
