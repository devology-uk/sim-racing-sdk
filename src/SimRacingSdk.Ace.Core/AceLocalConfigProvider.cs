using System.Text;
using System.Text.Json;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Core.Models.Config;

namespace SimRacingSdk.Ace.Core;

public class AceLocalConfigProvider : IAceLocalConfigProvider
{
    private static AceLocalConfigProvider? singletonInstance;
    private readonly IAcePathProvider acePathProvider;
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    public AceLocalConfigProvider(IAcePathProvider acePathProvider)
    {
        this.acePathProvider = acePathProvider;
    }

    public static AceLocalConfigProvider Instance =>
        singletonInstance ??= new AceLocalConfigProvider(AcePathProvider.Instance);

    public Account? GetAccount()
    {
        return this.DeserialiseConfigEntity<Account>(this.acePathProvider.AccountFilePath);
    }

    public BroadcastingSettings? GetBroadcastingSettings()
    {
        return this.DeserialiseConfigEntity<BroadcastingSettings>(this.acePathProvider
                                                                       .BroadcastingSettingsFilePath);
    }

    public void SaveBroadcastingSettings(BroadcastingSettings settings)
    {
        var json = JsonSerializer.Serialize(settings, this.jsonSerializerOptions);
        File.WriteAllText(this.acePathProvider.BroadcastingSettingsFilePath, json, Encoding.UTF8);
    }

    private T? DeserialiseConfigEntity<T>(string filePath)
        where T: class
    {
        if(!File.Exists(filePath))
        {
            return null;
        }

        try
        {
            var content = this.GetContent(filePath, Encoding.UTF8);
            return JsonSerializer.Deserialize<T>(content, this.jsonSerializerOptions);
        }
        catch(Exception)
        {
            var content = this.GetContent(filePath, Encoding.Unicode);
            return JsonSerializer.Deserialize<T>(content, this.jsonSerializerOptions);
        }
    }

    private string GetContent(string filePath, Encoding encoding)
    {
        return File.ReadAllText(filePath, encoding);
    }
}
