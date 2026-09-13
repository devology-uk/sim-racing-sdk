using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;

// Usage:
//   dotnet run --project SimRacingSdk.Tools.PmrCatalogImport
//     Dry run - writes PmrCarInfoProvider.generated.cs / PmrTrackInfoProvider.generated.cs
//     next to the repo root for review.
//   dotnet run --project SimRacingSdk.Tools.PmrCatalogImport -- --write
//     Overwrites PmrCarInfoProvider.cs / PmrTrackInfoProvider.cs directly.
//   dotnet run --project SimRacingSdk.Tools.PmrCatalogImport -- --installPath="D:\Games\Project Motor Racing"
//     Reads from a non-default Project Motor Racing install.
//
// Source of truth is the game's own install - data/vehicles/*/data/*.vdef and
// data/tracks/*/data/*.tdef (+ data/tracks/*/data/layouts/*.xml) - a custom
// "<table><parameters><parameter name=".." value=".."/></parameters></table>" XML format.
// Unlike SimRacingSdk.Tools.AceCarCatalogImport, there is no hand-curated CSV and nothing to
// preserve between runs - every run fully regenerates both provider files from the install.

var repoRoot = FindRepoRoot();
var write = args.Contains("--write");
var installPath = ResolveInstallPath(args);

if (installPath is null)
{
    Console.WriteLine("Could not find a Project Motor Racing install. Pass --installPath=\"<path>\" explicitly.");
    return 1;
}

var vehiclesRoot = Path.Combine(installPath, "data", "vehicles");
var tracksRoot = Path.Combine(installPath, "data", "tracks");
var warnings = new List<string>();

var carRows = ParseCars(vehiclesRoot, warnings);
var trackRows = ParseTracks(tracksRoot, warnings);

var carProviderPath = Path.Combine(repoRoot, "src", "SimRacingSdk.Pmr.Core", "PmrCarInfoProvider.cs");
var trackProviderPath = Path.Combine(repoRoot, "src", "SimRacingSdk.Pmr.Core", "PmrTrackInfoProvider.cs");
var carOutputPath = write ? carProviderPath : Path.Combine(repoRoot, "PmrCarInfoProvider.generated.cs");
var trackOutputPath = write ? trackProviderPath : Path.Combine(repoRoot, "PmrTrackInfoProvider.generated.cs");

File.WriteAllText(carOutputPath, BuildCarProviderSource(carRows));
File.WriteAllText(trackOutputPath, BuildTrackProviderSource(trackRows));

Console.WriteLine($"Wrote {carRows.Count} car entries to {carOutputPath}");
Console.WriteLine($"Wrote {trackRows.Count} track/layout entries to {trackOutputPath}");

var noFuelData = carRows.Where(c => c.FuelCapacityLitres is null)
                         .ToList();
if (noFuelData.Count > 0)
{
    Console.WriteLine(
        $"{noFuelData.Count} cars have no fuel-capacity data anywhere (vdef or vset) - all confirmed Hypercar/LMDh, "
        + "consistent with that class using an energy allocation rather than a fixed tank size:");
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

static List<CarRow> ParseCars(string vehiclesRoot, List<string> warnings)
{
    var rows = new List<CarRow>();
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

        var manufacturer = GetString(parameters, "Manufacturer");

        rows.Add(new CarRow(
            id,
            manufacturer,
            GetString(parameters, "Name"),
            GetString(parameters, "VehicleClass"),
            GetString(parameters, "ClassString"),
            GetNumber(parameters, "TorqueNM"),
            GetNumber(parameters, "WeightKG"),
            GetString(parameters, "Transmission"),
            GetNumber(parameters, "PowerBHP"),
            fuelCapacityLitres,
            GetString(parameters, "Layout"),
            GetString(parameters, "EngineName"),
            GetString(parameters, "Region"),
            GetInt(parameters, "Year"),
            GetString(parameters, "description"),
            GetString(parameters, "VehiclePath"),
            ResolveIconFileName(manufacturer)));
    }

    return rows.OrderBy(c => c.Manufacturer, StringComparer.OrdinalIgnoreCase)
               .ThenBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
               .ToList();
}

// The game's own Country strings are inconsistently cased/snake_cased (e.g. "united_states",
// "South_Africa", both "United_Kingdom" and "united_kingdom" across different tracks) - keyed
// here with underscores normalised to spaces and lower-cased so all variants resolve to one
// clean display name plus the ISO 3166-1 alpha-3 code the flag PNGs are named with.
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
        ["south africa"] = ("South Africa", "ZAF"),
        ["united kingdom"] = ("United Kingdom", "GBR"),
        ["united states"] = ("United States", "USA")
    };
}

static (string Country, string CountryCode) ResolveCountry(string rawCountry, List<string> warnings, string trackId)
{
    var key = rawCountry.Replace('_', ' ')
                         .Trim()
                         .ToLowerInvariant();
    if (BuildCountryLookup().TryGetValue(key, out var match))
    {
        return (match.DisplayName, match.Iso3Code);
    }

    warnings.Add($"No country-code mapping for \"{rawCountry}\" ({trackId}) - add it to BuildCountryLookup.");
    return (rawCountry, "");
}

// The game's own tdef files use "Other" for its "Rest of World" grouping (confirmed against
// the in-game track selector's four groups: All, Americas, Europe, Rest of World - "All" is
// just a UI meta-filter, not a real Continent value). Europe/Americas pass through unchanged.
static string ResolveContinent(string rawContinent)
{
    return rawContinent.Equals("Other", StringComparison.OrdinalIgnoreCase) ? "Rest of World" : rawContinent;
}

static List<TrackRow> ParseTracks(string tracksRoot, List<string> warnings)
{
    var rows = new List<TrackRow>();
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

            var (country, countryCode) = ResolveCountry(GetString(trackParameters, "Country"), warnings, trackId);

            rows.Add(new TrackRow(
                trackId,
                GetString(trackParameters, "Name"),
                Path.GetFileName(trackFolder),
                GetString(layoutParameters, "ID"),
                GetString(layoutParameters, "Name"),
                GetNumber(layoutParameters, "Length"),
                GetInt(layoutParameters, "Turns"),
                GetInt(layoutParameters, "GridSize"),
                GetString(layoutParameters, "Direction"),
                ResolveContinent(GetString(trackParameters, "Continent")),
                country,
                countryCode,
                GetString(trackParameters, "City"),
                GetInt(trackParameters, "TrackYear"),
                GetNumber(trackParameters, "TrackLatitude"),
                GetNumber(trackParameters, "TrackLongitude"),
                GetNumber(trackParameters, "TrackAltitudeMetresAMSL"),
                GetNumber(trackParameters, "TrackTimeZoneUTC"),
                GetNumber(trackParameters, "pitSpeedLimit"),
                GetNumber(trackParameters, "maxOvertime")));
        }
    }

    return rows.OrderBy(t => t.TrackName, StringComparer.OrdinalIgnoreCase)
               .ThenBy(t => t.LayoutName, StringComparer.OrdinalIgnoreCase)
               .ToList();
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

static string BuildCarProviderSource(List<CarRow> cars)
{
    var entryLines = cars.Select(c =>
        "        new() { "
        + $"ClassString = {CsString(c.ClassString)}, "
        + $"DescriptionKey = {CsString(c.DescriptionKey)}, "
        + $"EngineName = {CsString(c.EngineName)}, "
        + $"FuelCapacityLitres = {CsNullableNumber(c.FuelCapacityLitres)}, "
        + $"IconFileName = {CsString(c.IconFileName)}, "
        + $"Id = {CsString(c.Id)}, "
        + $"Layout = {CsString(c.Layout)}, "
        + $"Manufacturer = {CsString(c.Manufacturer)}, "
        + $"Name = {CsString(c.Name)}, "
        + $"PowerBhp = {CsNumber(c.PowerBhp)}, "
        + $"Region = {CsString(c.Region)}, "
        + $"TorqueNm = {CsNumber(c.TorqueNm)}, "
        + $"Transmission = {CsString(c.Transmission)}, "
        + $"VehicleClass = {CsString(c.VehicleClass)}, "
        + $"VehiclePath = {CsString(c.VehiclePath)}, "
        + $"WeightKg = {CsNumber(c.WeightKg)}, "
        + $"Year = {c.Year} "
        + "},");

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

                 public PmrCarInfo? FindById(string id)
                 {
                     return this.cars.FirstOrDefault(c => c.Id == id);
                 }

                 public PmrCarInfo? FindByVehiclePath(string vehiclePath)
                 {
                     return this.cars.FirstOrDefault(c => c.VehiclePath == vehiclePath);
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

static string BuildTrackProviderSource(List<TrackRow> tracks)
{
    var entryLines = tracks.Select(t =>
        "        new() { "
        + $"AltitudeMetresAmsl = {CsNumber(t.AltitudeMetresAmsl)}, "
        + $"City = {CsString(t.City)}, "
        + $"Continent = {CsString(t.Continent)}, "
        + $"Country = {CsString(t.Country)}, "
        + $"CountryCode = {CsString(t.CountryCode)}, "
        + $"Direction = {CsString(t.Direction)}, "
        + $"GridSize = {t.GridSize}, "
        + $"Latitude = {CsNumber(t.Latitude)}, "
        + $"LayoutId = {CsString(t.LayoutId)}, "
        + $"LayoutName = {CsString(t.LayoutName)}, "
        + $"LengthKm = {CsNumber(t.LengthKm)}, "
        + $"Longitude = {CsNumber(t.Longitude)}, "
        + $"MaxOvertimeSeconds = {CsNumber(t.MaxOvertimeSeconds)}, "
        + $"PitSpeedLimitMetresPerSecond = {CsNumber(t.PitSpeedLimitMetresPerSecond)}, "
        + $"Turns = {t.Turns}, "
        + $"TrackFolder = {CsString(t.TrackFolder)}, "
        + $"TrackId = {CsString(t.TrackId)}, "
        + $"TrackName = {CsString(t.TrackName)}, "
        + $"TimeZoneUtcOffset = {CsNumber(t.TimeZoneUtcOffset)}, "
        + $"TrackYear = {t.TrackYear} "
        + "},");

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

internal record CarRow(
    string Id,
    string Manufacturer,
    string Name,
    string VehicleClass,
    string ClassString,
    double TorqueNm,
    double WeightKg,
    string Transmission,
    double PowerBhp,
    double? FuelCapacityLitres,
    string Layout,
    string EngineName,
    string Region,
    int Year,
    string DescriptionKey,
    string VehiclePath,
    string IconFileName);

internal record TrackRow(
    string TrackId,
    string TrackName,
    string TrackFolder,
    string LayoutId,
    string LayoutName,
    double LengthKm,
    int Turns,
    int GridSize,
    string Direction,
    string Continent,
    string Country,
    string CountryCode,
    string City,
    int TrackYear,
    double Latitude,
    double Longitude,
    double AltitudeMetresAmsl,
    double TimeZoneUtcOffset,
    double PitSpeedLimitMetresPerSecond,
    double MaxOvertimeSeconds);
