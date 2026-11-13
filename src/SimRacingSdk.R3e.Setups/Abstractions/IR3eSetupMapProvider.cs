using SimRacingSdk.R3e.Setups.Models;

namespace SimRacingSdk.R3e.Setups.Abstractions;

public interface IR3eSetupMapProvider
{
    // Null for a car nobody has mapped yet: consumers fall back to R3eSetupControls.All, not knowing which are fixed.
    R3eSetupMap? FindByCarGameId(int carGameId);

    IReadOnlyList<R3eSetupMap> GetSetupMaps();
}
