using SimRacingSdk.Pmr.Setups.Models.Presentation;

namespace SimRacingSdk.Pmr.Setups.Abstractions;

public interface IPmrUnitPreferencesProvider
{
    // Metric throughout when the game's settings can't be read.
    PmrUnitPreferences GetGameUnitPreferences();
}
