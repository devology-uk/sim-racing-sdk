using System.Text;
using System.Text.RegularExpressions;

// Regenerates AceCarInfoProvider.cs's hardcoded car list from ace-cars.csv (the source of truth
// Mike also uses to populate a car catalog on a couple of WordPress sites). Every catalog field
// (Manufacturer, Name, physical specs, ...) comes from the CSV and is fully overwritten on each
// run. Two properties are ACE Setup Manager-specific, not general car-catalog data, so they're
// deliberately NOT sourced from the CSV at all - the tool preserves them from the existing
// AceCarInfoProvider.cs instead (keyed by ModelId), so re-running this can never silently wipe
// them: SetupSchema, and TyreCompounds (a "Tyre Compounds" CSV column existed briefly but was
// removed once schema work started - both properties are now permanently hand-maintained C# data
// only). If the CSV ever gains a non-empty "Tyre Compounds" cell for a row again, that value would
// still take priority over the preserved one - this tool doesn't special-case the column being
// gone, it just falls back the same way it would for any other still-empty cell.
// A car with neither a CSV value nor a prior value for these just gets the type's own default.
//
// Usage:
//   dotnet run --project SimRacingSdk.Tools.AceCarCatalogImport
//     Dry run - writes to AceCarInfoProvider.generated.cs next to the CSV for review.
//   dotnet run --project SimRacingSdk.Tools.AceCarCatalogImport -- --write
//     Overwrites AceCarInfoProvider.cs directly.

var repoRoot = FindRepoRoot();
var csvPath = Path.Combine(repoRoot, "ace-cars.csv");
var providerPath = Path.Combine(repoRoot, "src", "SimRacingSdk.Ace.Core", "AceCarInfoProvider.cs");
var write = args.Contains("--write");
var outputPath = write ? providerPath : Path.Combine(repoRoot, "AceCarInfoProvider.generated.cs");

var preservedSchemas = ExtractPreservedPropertyValues(providerPath, "SetupSchema", '{', '}');
var preservedTyreCompounds = ExtractPreservedPropertyValues(providerPath, "TyreCompounds", '[', ']');
var (headers, rows) = ParseCsv(csvPath);
var columnIndex = BuildColumnIndex(headers);

var entryLines = new List<string>();
var warnings = new List<string>();
foreach (var row in rows)
{
    var (line, rowWarnings) = BuildCarEntryLine(row, columnIndex, preservedSchemas, preservedTyreCompounds);
    entryLines.Add(line);
    warnings.AddRange(rowWarnings);
}

var originalText = ReadAllTextShared(providerPath);
var newText = ReplaceCarsList(originalText, entryLines);
File.WriteAllText(outputPath, newText);

Console.WriteLine($"Wrote {entryLines.Count} car entries to {outputPath}");
var preservedSchemaCount = rows.Count(r => preservedSchemas.ContainsKey(r[columnIndex["Model Id"]]));
Console.WriteLine($"Preserved SetupSchema for {preservedSchemaCount} of {entryLines.Count} cars.");
var preservedCompoundCount = rows.Count(r =>
    string.IsNullOrEmpty(GetCell(r, columnIndex, "Tyre Compounds"))
    && preservedTyreCompounds.ContainsKey(r[columnIndex["Model Id"]]));
Console.WriteLine($"Preserved TyreCompounds (no CSV value yet) for {preservedCompoundCount} of {entryLines.Count} cars.");
if (!write)
{
    Console.WriteLine("Dry run - review the output, then pass --write to overwrite AceCarInfoProvider.cs directly.");
}
foreach (var warning in warnings)
{
    Console.WriteLine($"WARNING: {warning}");
}

return;

static string FindRepoRoot()
{
    var dir = new DirectoryInfo(AppContext.BaseDirectory);
    while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "ace-cars.csv")))
    {
        dir = dir.Parent;
    }

    return dir?.FullName ?? throw new InvalidOperationException(
        "Could not find repo root (walked up from AppContext.BaseDirectory looking for ace-cars.csv).");
}

// Extracts a property's exact source text from every car entry in the current provider file,
// keyed by ModelId - used to carry values forward across a regeneration that aren't (or aren't
// yet) sourced from the CSV. propertyName's value must start with openChar and its matching
// closeChar must close it (works for both "{ ... }" object initializers and "[ ... ]" array
// literals).
static Dictionary<string, string> ExtractPreservedPropertyValues(
    string providerPath, string propertyName, char openChar, char closeChar)
{
    var text = ReadAllTextShared(providerPath);
    var result = new Dictionary<string, string>();
    var propertyKeyword = $"{propertyName} = ";

    // Trailing comma is optional - the last entry in a C# collection literal has none.
    foreach (Match lineMatch in Regex.Matches(text, @"^\s*new\(\)\s*\{.*\},?\s*$", RegexOptions.Multiline))
    {
        var line = lineMatch.Value;
        var modelIdMatch = Regex.Match(line, @"ModelId\s*=\s*""([^""]*)""");
        if (!modelIdMatch.Success)
        {
            continue;
        }

        var modelId = modelIdMatch.Groups[1].Value;

        var keywordIndex = line.IndexOf(propertyKeyword, StringComparison.Ordinal);
        if (keywordIndex < 0)
        {
            continue;
        }

        result[modelId] = ExtractBalancedExpression(line, keywordIndex + propertyKeyword.Length, openChar, closeChar);
    }

    return result;
}

// Starting just after "<Property> = ", captures the full value expression, correctly balancing
// nested occurrences of openChar/closeChar (e.g. the Tyres/Electronics/... sub-objects inside a
// SetupSchema value).
static string ExtractBalancedExpression(string line, int start, char openChar, char closeChar)
{
    var openStart = line.IndexOf(openChar, start);
    var depth = 0;
    var i = openStart;
    for (; i < line.Length; i++)
    {
        if (line[i] == openChar)
        {
            depth++;
        }
        else if (line[i] == closeChar)
        {
            depth--;
            if (depth == 0)
            {
                i++;
                break;
            }
        }
    }

    return line[start..i].Trim();
}

static (string[] Headers, List<string[]> Rows) ParseCsv(string csvPath)
{
    var lines = ReadAllLinesShared(csvPath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
    var headers = ParseCsvLine(lines[0]);
    var rows = lines.Skip(1).Select(ParseCsvLine).ToList();
    return (headers, rows);
}

// Tolerates the CSV being open in Excel (or the provider file open in Visual Studio) at the same
// time - both commonly hold a lock that still permits shared reads.
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

static string ReadAllTextShared(string path)
{
    using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    using var reader = new StreamReader(stream);
    return reader.ReadToEnd();
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

static string GetCell(string[] row, Dictionary<string, int> columnIndex, string columnName) =>
    columnIndex.TryGetValue(columnName, out var i) && i < row.Length ? row[i].Trim() : "";

static (string Line, List<string> Warnings) BuildCarEntryLine(
    string[] row,
    Dictionary<string, int> columnIndex,
    Dictionary<string, string> preservedSchemas,
    Dictionary<string, string> preservedTyreCompounds)
{
    var warnings = new List<string>();

    string Cell(string columnName) =>
        columnIndex.TryGetValue(columnName, out var i) && i < row.Length ? row[i].Trim() : "";

    string RequiredNumber(string columnName)
    {
        var value = Cell(columnName);
        if (string.IsNullOrEmpty(value))
        {
            warnings.Add($"{Cell("Model Id")}: missing required numeric value for '{columnName}', defaulting to 0");
            return "0";
        }

        return value;
    }

    string NullableValue(string columnName) =>
        Cell(columnName) is { Length: > 0 } value ? value : "null";

    string QuotedString(string columnName) => $"\"{Cell(columnName)}\"";

    var modelId = Cell("Model Id");

    var tyreCompoundsCell = Cell("Tyre Compounds");
    string tyreCompoundsProperty;
    if (tyreCompoundsCell.Length > 0)
    {
        var names = tyreCompoundsCell.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var arrayLiteral = string.Join(", ", names.Select(n => $"\"{n}\""));
        tyreCompoundsProperty = $"TyreCompounds = [{arrayLiteral}], ";
    }
    else if (preservedTyreCompounds.TryGetValue(modelId, out var preserved))
    {
        tyreCompoundsProperty = $"TyreCompounds = {preserved}, ";
    }
    else
    {
        tyreCompoundsProperty = "";
    }

    var setupSchemaProperty = preservedSchemas.TryGetValue(modelId, out var schema)
        ? $"SetupSchema = {schema}, "
        : "";

    var properties = new StringBuilder();
    properties.Append($"Body = {QuotedString("Body")}, ");
    properties.Append($"BatteryKwh = {NullableValue("Battery KWH")}, ");
    properties.Append($"Categories = {QuotedString("Categories")}, ");
    properties.Append($"Cylinders = {NullableValue("Cylinders")}, ");
    properties.Append($"DisplacementL = {NullableValue("Displacement L")}, ");
    properties.Append($"DriveLayout = {QuotedString("Drive Layout")}, ");
    properties.Append($"Engine = {QuotedString("Engine")}, ");
    properties.Append($"EngineLayout = {QuotedString("Engine Layout")}, ");
    properties.Append($"FuelTankL = {NullableValue("Fuel Tank L")}, ");
    properties.Append($"Gears = {NullableValue("Gears")}, ");
    properties.Append($"Manufacturer = {QuotedString("Manufacturer")}, ");
    properties.Append($"MaxRpm = {NullableValue("Max RPM")}, ");
    properties.Append($"MaxSpeedKmh = {RequiredNumber("Max Speed KMH")}, ");
    properties.Append($"ModelId = {QuotedString("Model Id")}, ");
    properties.Append($"Name = {QuotedString("Name")}, ");
    properties.Append($"PerformanceRating = {RequiredNumber("Performance Rating")}, ");
    properties.Append($"PowerPs = {RequiredNumber("Power PS/KW")}, ");
    properties.Append($"RacingClass = {QuotedString("Racing Class")}, ");
    properties.Append(setupSchemaProperty);
    properties.Append($"TorqueNm = {RequiredNumber("Torque NM")}, ");
    properties.Append(tyreCompoundsProperty);
    properties.Append($"Variant = {QuotedString("Variant")}, ");
    properties.Append($"WeightKg = {RequiredNumber("Weight KG")}, ");
    properties.Append($"Year = {RequiredNumber("Year")}, ");
    properties.Append($"ZeroToOneHundredKmh = {RequiredNumber("0-100 KMH")}");

    return ($"        new() {{ {properties} }},", warnings);
}

static string ReplaceCarsList(string originalText, List<string> entryLines)
{
    const string startMarker = "private readonly List<AceCarInfo> cars =\n    [\n";
    const string endMarker = "\n    ];\n";

    var startIndex = originalText.IndexOf(startMarker, StringComparison.Ordinal);
    if (startIndex < 0)
    {
        throw new InvalidOperationException("Could not find the start of the 'cars' list in AceCarInfoProvider.cs.");
    }

    var contentStart = startIndex + startMarker.Length;
    var endIndex = originalText.IndexOf(endMarker, contentStart, StringComparison.Ordinal);
    if (endIndex < 0)
    {
        throw new InvalidOperationException("Could not find the end of the 'cars' list in AceCarInfoProvider.cs.");
    }

    var newListContent = string.Join('\n', entryLines);
    return string.Concat(
        originalText.AsSpan(0, contentStart),
        newListContent,
        originalText.AsSpan(endIndex));
}
