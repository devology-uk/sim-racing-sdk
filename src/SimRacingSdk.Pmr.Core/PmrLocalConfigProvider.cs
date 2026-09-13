using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SimRacingSdk.Pmr.Core.Abstractions;
using SimRacingSdk.Pmr.Core.Models.Config;

namespace SimRacingSdk.Pmr.Core;

public class PmrLocalConfigProvider : IPmrLocalConfigProvider
{
    // The dropdown cycles 10 -> 20 -> 30 -> 60 -> 10 ... and settings are 1-based, confirmed by
    // toggling the in-game option and re-reading the saved index (index 4 measured against a
    // UI-displayed "60").
    private static readonly int[] UdpFrequencyOptionsHz = [10, 20, 30, 60];

    private static PmrLocalConfigProvider? singletonInstance;

    private readonly IPmrPathProvider pmrPathProvider;

    public PmrLocalConfigProvider(IPmrPathProvider pmrPathProvider)
    {
        this.pmrPathProvider = pmrPathProvider;
    }

    public static PmrLocalConfigProvider Instance =>
        singletonInstance ??= new PmrLocalConfigProvider(PmrPathProvider.Instance);

    public PmrDriverProfile? GetDriverProfile()
    {
        var parameters = LoadParameters(this.pmrPathProvider.DriverProfileFilePath);
        if(parameters == null)
        {
            return null;
        }

        return new PmrDriverProfile
        {
            Nationality = GetString(parameters, "Nationality")
        };
    }

    public PmrLocalSettings? GetLocalSettings()
    {
        var parameters = LoadParameters(this.pmrPathProvider.SettingsFilePath);
        if(parameters == null)
        {
            return null;
        }

        var frequencyIndex = GetInt(parameters, "SettingsPreferencesUDPFrequency");

        return new PmrLocalSettings
        {
            UdpEnabled = GetInt(parameters, "SettingsPreferencesUDPEnabled") == 2,
            UdpFrequencyHz = frequencyIndex >= 1 && frequencyIndex <= UdpFrequencyOptionsHz.Length
                ? UdpFrequencyOptionsHz[frequencyIndex - 1]
                : 0,
            UdpHost = GetString(parameters, "SettingsPreferencesUDPHost"),
            UdpPort = GetInt(parameters, "SettingsPreferencesUDPPort")
        };
    }

    private static string GetString(XElement parameters, string name)
    {
        return parameters.Elements("param")
                          .FirstOrDefault(p => (string?)p.Attribute("name") == name)
                          ?.Attribute("value")
                          ?.Value
                          ?? string.Empty;
    }

    private static int GetInt(XElement parameters, string name)
    {
        var raw = GetString(parameters, name);
        return int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) ? value : 0;
    }

    private static XElement? LoadParameters(string filePath)
    {
        if(!File.Exists(filePath))
        {
            return null;
        }

        // Every one of the game's own settings/save XML files closes its root with
        // "</table name=".." type=".."/>" - not valid XML (attributes on a closing tag), but
        // it's the format the game itself writes, so its own loader must be lenient about it.
        // Strip the attributes back off before parsing - same fix as PmrCatalogImport uses for
        // the equally-malformed .vset files.
        var xml = Regex.Replace(File.ReadAllText(filePath), "</table[^>]*>", "</table>");
        var document = XDocument.Parse(xml);
        return document.Root?.Element("parameters");
    }
}
