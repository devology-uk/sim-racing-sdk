using SimRacingSdk.R3e.Core.Abstractions;
using SimRacingSdk.R3e.Core.Enums;
using SimRacingSdk.R3e.Core.Models.Config;

namespace SimRacingSdk.R3e.Core;

public class R3eLocalConfigProvider : IR3eLocalConfigProvider
{
    private const string LanguageCodeKey = "GameLanguage.languageCode";
    private const string SpeedInKphKey = "menuSpeedInKph";
    private const string UnitSystemKey = "menuUnitSystem";

    private static R3eLocalConfigProvider? singletonInstance;
    private readonly IR3eGameOptionsReader gameOptionsReader;
    private readonly IR3ePathProvider r3ePathProvider;

    public R3eLocalConfigProvider(IR3ePathProvider r3ePathProvider, IR3eGameOptionsReader gameOptionsReader)
    {
        this.r3ePathProvider = r3ePathProvider;
        this.gameOptionsReader = gameOptionsReader;
    }

    public static R3eLocalConfigProvider Instance =>
        singletonInstance ??= new R3eLocalConfigProvider(R3ePathProvider.Instance, R3eGameOptionsReader.Instance);

    public R3eGameOptions? GetGameOptions()
    {
        var filePath = this.r3ePathProvider.GameOptionsFilePath;
        if(!File.Exists(filePath))
        {
            return null;
        }

        var values = this.gameOptionsReader.Read(filePath);
        return new R3eGameOptions
        {
            LanguageCode = values.GetValueOrDefault(LanguageCodeKey) as string ?? string.Empty,
            IsSpeedInKph = this.GetInteger(values, SpeedInKphKey) != 0,
            UnitSystem = (R3eUnitSystem)this.GetInteger(values, UnitSystemKey)
        };
    }

    private int GetInteger(IReadOnlyDictionary<string, object> values, string key)
    {
        return values.GetValueOrDefault(key) is int value ? value : 0;
    }
}
