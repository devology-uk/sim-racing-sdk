using System.Text;
using System.Text.Json;
using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Acc.Core.Models.Config;

namespace SimRacingSdk.Acc.Core;

public class AccLocalConfigProvider: IAccLocationConfigProvider
{
    private static AccLocalConfigProvider? singletonInstance;
    private readonly IAccPathProvider accPathProvider;
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    public AccLocalConfigProvider(IAccPathProvider accPathProvider)
    {
        this.accPathProvider = accPathProvider;
    }

    public static AccLocalConfigProvider Instance =>
        singletonInstance ??= new AccLocalConfigProvider(AccPathProvider.Instance);

    public Account? GetAccount()
    {
        return this.DeserialiseConfigEntity<Account>(this.accPathProvider.AccountFilePath);
    }

    public BroadcastingSettings? GetBroadcastingSettings()
    {
        return this.DeserialiseConfigEntity<BroadcastingSettings>(this.accPathProvider
                                                                      .BroadcastingSettingsFilePath);
    }

    // public SeasonSettings? GetSeasonSettings()
    // {
    //     return DeserialiseConfigEntity<SeasonSettings>(this.accPathProvider.SeasonSettingsFilePath);
    // }

    public void SaveBroadcastingSettings(BroadcastingSettings settings)
    {
        var json = JsonSerializer.Serialize(settings, this.jsonSerializerOptions);
        File.WriteAllText(this.accPathProvider.BroadcastingSettingsFilePath, json, Encoding.UTF8);
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

public interface IAccLocationConfigProvider
{
    Account? GetAccount();
    BroadcastingSettings? GetBroadcastingSettings();
    void SaveBroadcastingSettings(BroadcastingSettings settings);
}