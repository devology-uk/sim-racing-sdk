using System.IO;
using System.Text.RegularExpressions;

namespace SimRacingSdk.Lmu.Setups.Models;

// Parses LMU's rFactor2-derived .svm setup text format into raw section/key data. Deliberately
// generic (index + comment text per field) rather than typed per-field - the per-field value
// representation (comment-text-verbatim vs. typed-numeric) is still pending Mike's own comparison
// against LMU's in-game setup screens, see the LMU Setup Manager plan.
public class LmuSetupFile
{
    private static readonly Regex VehiclesPathSegmentRegex =
        new(@"[\\/]Vehicles[\\/]([^\\/]+)[\\/]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    // The game's own "Quick Setup" convenience view - a simplified view of values shown in full
    // elsewhere, dropped from v1. See the LMU Setup Manager plan.
    private static readonly HashSet<string> DroppedSections = new(StringComparer.OrdinalIgnoreCase) { "BASIC" };

    public string? VehicleClassSetting { get; private set; }

    // The vehicle-folder segment from the file's own //VEH= comment (e.g. "911GT3R_2024") - used to
    // resolve the car via LmuCarInfo.ModelId. Null if the file has no //VEH= line, or that line has
    // no "Vehicles" path segment.
    public string? VehicleFolderId { get; private set; }

    public Dictionary<string, Dictionary<string, LmuSetupValue>> Sections { get; } =
        new(StringComparer.OrdinalIgnoreCase);

    public static LmuSetupFile Parse(string filePath)
    {
        var result = new LmuSetupFile();
        string? currentSection = null;

        foreach(var rawLine in File.ReadLines(filePath))
        {
            var line = rawLine.Trim();
            if(line.Length == 0)
            {
                continue;
            }

            if(line.StartsWith("//VEH=", StringComparison.OrdinalIgnoreCase))
            {
                result.VehicleFolderId = ExtractVehicleFolderId(line[6..]);
                continue;
            }

            if(line.StartsWith("//", StringComparison.Ordinal))
            {
                continue;
            }

            if(line.StartsWith('[') && line.EndsWith(']'))
            {
                currentSection = line[1..^1];
                continue;
            }

            var equalsIndex = line.IndexOf('=');
            if(equalsIndex < 0)
            {
                continue;
            }

            var key = line[..equalsIndex].Trim();
            var valueAndComment = line[(equalsIndex + 1)..];

            if(currentSection is null)
            {
                if(key.Equals("VehicleClassSetting", StringComparison.OrdinalIgnoreCase))
                {
                    result.VehicleClassSetting = valueAndComment.Trim().Trim('"');
                }

                continue;
            }

            if(DroppedSections.Contains(currentSection))
            {
                continue;
            }

            AddSectionValue(result, currentSection, key, valueAndComment);
        }

        return result;
    }

    private static void AddSectionValue(LmuSetupFile result, string currentSection, string key, string valueAndComment)
    {
        var(value, comment) = SplitValueAndComment(valueAndComment);
        if(!int.TryParse(value, out var index) && comment.Length == 0)
        {
            comment = value.Trim('"');
        }

        if(!result.Sections.TryGetValue(currentSection, out var section))
        {
            section = new Dictionary<string, LmuSetupValue>(StringComparer.OrdinalIgnoreCase);
            result.Sections[currentSection] = section;
        }

        section[key] = new LmuSetupValue(index, comment);
    }

    private static (string Value, string Comment) SplitValueAndComment(string valueAndComment)
    {
        var commentIndex = valueAndComment.IndexOf("//", StringComparison.Ordinal);
        return commentIndex < 0
                   ? (valueAndComment.Trim(), "")
                   : (valueAndComment[..commentIndex].Trim(), valueAndComment[(commentIndex + 2)..].Trim());
    }

    private static string? ExtractVehicleFolderId(string vehPath)
    {
        var match = VehiclesPathSegmentRegex.Match(vehPath);
        return match.Success ? match.Groups[1].Value : null;
    }
}
