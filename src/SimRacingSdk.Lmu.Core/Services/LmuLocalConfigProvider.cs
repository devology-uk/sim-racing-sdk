using System.Text;
using System.Text.RegularExpressions;
using SimRacingSdk.Abstractions;
using SimRacingSdk.Core.Services;
using SimRacingSdk.Lmu.Core.Abstractions;

namespace SimRacingSdk.Lmu.Core.Services;

public class LmuLocalConfigProvider : ILmuLocalConfigProvider
{
    private static readonly Regex DisabledExternalPlugins = new("(\"Enable external plugins\"\\s*:\\s*)false");

    private static LmuLocalConfigProvider? singletonInstance;

    private readonly IConfigFileBackup configFileBackup;
    private readonly ILmuPathProvider pathProvider;

    public LmuLocalConfigProvider(ILmuPathProvider pathProvider, IConfigFileBackup configFileBackup)
    {
        this.pathProvider = pathProvider;
        this.configFileBackup = configFileBackup;
    }

    public static LmuLocalConfigProvider Instance =>
        singletonInstance ??= new LmuLocalConfigProvider(LmuPathProvider.Instance, ConfigFileBackup.Instance);

    // LMU owns Settings.JSON while it's running and can overwrite it, so only call this with the game closed. The file is edited as text
    // rather than re-serialised so the rest of it, including the "#" description entries, stays as the game wrote it.
    public void EnableExternalPlugins()
    {
        var filePath = this.pathProvider.SettingsFilePath;
        var (content, encoding) = ReadText(filePath);
        if(!DisabledExternalPlugins.IsMatch(content))
        {
            return;
        }

        var updatedContent = DisabledExternalPlugins.Replace(content, match => match.Groups[1].Value + "true", 1);
        this.configFileBackup.Backup(filePath);
        File.WriteAllText(filePath, updatedContent, encoding);
    }

    private static (string Content, Encoding Encoding) ReadText(string filePath)
    {
        using var reader = new StreamReader(filePath, new UTF8Encoding(false), true);
        var content = reader.ReadToEnd();
        return (content, reader.CurrentEncoding);
    }
}
