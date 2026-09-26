using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SimRacingSdk.Pmr.Setups.Models.Files;

// A .vset's raw values: one <param name=".." type="number" value=".."/> per setting.
public record PmrSetupFile
{
    // The game closes the root with its attributes repeated ("</table name=.. type=..>"), which isn't
    // valid XML; the attributes are stripped before parsing.
    private static readonly Regex MalformedClosingTag = new(@"</table[^>]*>", RegexOptions.Compiled);

    public required PmrSetupFileName Name { get; init; }
    public required IReadOnlyDictionary<string, double> Values { get; init; }

    public static PmrSetupFile Parse(PmrSetupFileName name, string content)
    {
        var document = XDocument.Parse(MalformedClosingTag.Replace(content, "</table>"));

        return new PmrSetupFile
        {
            Name = name,
            Values = ReadNumericParameters(document)
        };
    }

    public double? FindValue(string rawKey)
    {
        return this.Values.TryGetValue(rawKey, out var value) ? value : null;
    }

    private static Dictionary<string, double> ReadNumericParameters(XDocument document)
    {
        var values = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
        foreach(var parameter in document.Descendants("param"))
        {
            var key = parameter.Attribute("name")?.Value;
            var text = parameter.Attribute("value")?.Value;
            if(key is not null && double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
            {
                values[key] = value;
            }
        }

        return values;
    }
}
