using SimRacingSdk.Pmr.Core;
using SimRacingSdk.Pmr.Core.Abstractions;
using SimRacingSdk.Pmr.Core.Models.Config;
using SimRacingSdk.Pmr.Setups.Abstractions;
using SimRacingSdk.Pmr.Setups.Models.Presentation;

namespace SimRacingSdk.Pmr.Setups.Services;

public class PmrUnitPreferencesProvider : IPmrUnitPreferencesProvider
{
    private static PmrUnitPreferencesProvider? singletonInstance;

    private readonly IPmrLocalConfigProvider localConfigProvider;

    public PmrUnitPreferencesProvider(IPmrLocalConfigProvider localConfigProvider)
    {
        this.localConfigProvider = localConfigProvider;
    }

    public static PmrUnitPreferencesProvider Instance =>
        singletonInstance ??= new PmrUnitPreferencesProvider(PmrLocalConfigProvider.Instance);

    public PmrUnitPreferences GetGameUnitPreferences()
    {
        var settings = this.localConfigProvider.GetLocalSettings();
        return settings is null ? PmrUnitPreferences.Metric : FromSettings(settings);
    }

    // Spring rates follow the game's Weight setting (lb/in in Imperial).
    private static PmrUnitPreferences FromSettings(PmrLocalSettings settings)
    {
        return new PmrUnitPreferences
        {
            Length = UnitSystemFor(settings.IsImperialDistance),
            Pressure = UnitSystemFor(settings.IsImperialPressure),
            SpringRate = UnitSystemFor(settings.IsImperialWeight),
            Volume = UnitSystemFor(settings.IsImperialFluid)
        };
    }

    private static PmrUnitSystem UnitSystemFor(bool isImperial)
    {
        return isImperial ? PmrUnitSystem.Imperial : PmrUnitSystem.Metric;
    }
}
