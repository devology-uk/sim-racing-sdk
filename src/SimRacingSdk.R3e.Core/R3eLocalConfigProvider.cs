using SimRacingSdk.R3e.Core.Abstractions;
using SimRacingSdk.R3e.Core.Enums;
using SimRacingSdk.R3e.Core.Models.Config;

namespace SimRacingSdk.R3e.Core;

public class R3eLocalConfigProvider : IR3eLocalConfigProvider
{
    private const string LanguageCodeKey = "GameLanguage.languageCode";

    // The in-game units option writes this key; menuUnitSystem stays 0 for Metric and Imperial alike.
    private const string MetricUnitsKey = "menuSpeedInKph";

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
            UnitSystem = this.IsMetric(values) ? R3eUnitSystem.Metric : R3eUnitSystem.Imperial
        };
    }

    private bool IsMetric(IReadOnlyDictionary<string, object> values)
    {
        return values.GetValueOrDefault(MetricUnitsKey) is not 0;
    }
}
