using System.Globalization;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SimRacingSdk.Abstractions;
using SimRacingSdk.Core.Services;
using SimRacingSdk.Pmr.Core.Abstractions;
using SimRacingSdk.Pmr.Core.Models.Config;

namespace SimRacingSdk.Pmr.Core;

public class PmrLocalConfigProvider : IPmrLocalConfigProvider
{
    // The dropdown cycles 10 -> 20 -> 30 -> 60 -> 10 ... and settings are 1-based, confirmed by
    // toggling the in-game option and re-reading the saved index (index 4 measured against a
    // UI-displayed "60").
    private static readonly int[] UdpFrequencyOptionsHz = [10, 20, 30, 60];

    // The game's unit dropdowns are 1-based: Metric is 1, Imperial 2.
    private const int ImperialUnitsOption = 2;
    private const int UdpEnabledOption = 2;

    private const string UdpEnabledParameter = "SettingsPreferencesUDPEnabled";
    private const string UdpFrequencyParameter = "SettingsPreferencesUDPFrequency";
    private const string UdpHostParameter = "SettingsPreferencesUDPHost";
    private const string UdpPortParameter = "SettingsPreferencesUDPPort";
    private const string NumberType = "number";
    private const string StringType = "string";
    private const string ParametersClosingTag = "</parameters>";

    private static PmrLocalConfigProvider? singletonInstance;

    private readonly IConfigFileBackup configFileBackup;
    private readonly IPmrPathProvider pmrPathProvider;

    public PmrLocalConfigProvider(IPmrPathProvider pmrPathProvider, IConfigFileBackup configFileBackup)
    {
        this.pmrPathProvider = pmrPathProvider;
        this.configFileBackup = configFileBackup;
    }

    public static PmrLocalConfigProvider Instance =>
        singletonInstance ??= new PmrLocalConfigProvider(PmrPathProvider.Instance, ConfigFileBackup.Instance);

    // PMR rewrites settings.xml while it's running, so only call this with the game closed. The file is edited
    // as text rather than re-serialised so everything else in it, including the game's invalid closing tag,
    // stays exactly as the game wrote it.
    public void EnableUdp(PmrUdpSettings settings)
    {
        var filePath = this.pmrPathProvider.SettingsFilePath;
        var (content, encoding) = ReadText(filePath);

        content = SetParameter(content, UdpEnabledParameter, NumberType, UdpEnabledOption.ToString(CultureInfo.InvariantCulture));
        content = SetParameter(content, UdpHostParameter, StringType, SecurityElement.Escape(settings.Host));
        content = SetParameter(content, UdpPortParameter, NumberType, settings.Port.ToString(CultureInfo.InvariantCulture));
        content = SetParameter(content, UdpFrequencyParameter, NumberType, ToFrequencyOption(settings.FrequencyHz));

        this.configFileBackup.Backup(filePath);
        File.WriteAllText(filePath, content, encoding);
    }

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

        var frequencyIndex = GetInt(parameters, UdpFrequencyParameter);

        return new PmrLocalSettings
        {
            IsImperialDistance = GetInt(parameters, "SettingsPreferencesDistanceUnits") == ImperialUnitsOption,
            IsImperialFluid = GetInt(parameters, "SettingsPreferencesFluidUnits") == ImperialUnitsOption,
            IsImperialPressure = GetInt(parameters, "SettingsPreferencesPressureUnits") == ImperialUnitsOption,
            IsImperialWeight = GetInt(parameters, "SettingsPreferencesWeightUnits") == ImperialUnitsOption,
            UdpEnabled = GetInt(parameters, UdpEnabledParameter) == UdpEnabledOption,
            UdpFrequencyHz = frequencyIndex >= 1 && frequencyIndex <= UdpFrequencyOptionsHz.Length
                ? UdpFrequencyOptionsHz[frequencyIndex - 1]
                : 0,
            UdpHost = GetString(parameters, UdpHostParameter),
            UdpPort = GetInt(parameters, UdpPortParameter)
        };
    }

    private static (string Content, Encoding Encoding) ReadText(string filePath)
    {
        using var reader = new StreamReader(filePath, new UTF8Encoding(false), true);
        var content = reader.ReadToEnd();
        return (content, reader.CurrentEncoding);
    }

    private static string SetParameter(string content, string name, string type, string value)
    {
        var existing = new Regex($"(<param name=\"{Regex.Escape(name)}\" type=\"[^\"]*\" value=\")[^\"]*(\")");
        return existing.IsMatch(content)
            ? existing.Replace(content, match => match.Groups[1].Value + value + match.Groups[2].Value, 1)
            : InsertParameter(content, name, type, value);
    }

    private static string InsertParameter(string content, string name, string type, string value)
    {
        var closingTagIndex = content.IndexOf(ParametersClosingTag, StringComparison.Ordinal);
        if(closingTagIndex < 0)
        {
            throw new InvalidDataException($"No {ParametersClosingTag} element in PMR's settings file.");
        }

        var lineStart = content.LastIndexOf('\n', closingTagIndex) + 1;
        var newLine = content.Contains("\r\n") ? "\r\n" : "\n";
        var parameter = $"        <param name=\"{name}\" type=\"{type}\" value=\"{value}\"/>{newLine}";
        return content.Insert(lineStart, parameter);
    }

    private static string ToFrequencyOption(int frequencyHz)
    {
        var index = Array.IndexOf(UdpFrequencyOptionsHz, frequencyHz);
        if(index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(frequencyHz),
                frequencyHz,
                $"PMR's UDP frequency must be one of {string.Join(", ", UdpFrequencyOptionsHz)} Hz.");
        }

        return (index + 1).ToString(CultureInfo.InvariantCulture);
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
