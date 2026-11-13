using SimRacingSdk.R3e.Setups.Models;

namespace SimRacingSdk.R3e.DataManager.SetupMaps;

public record AvailabilityOption(R3eSetupControlAvailability Value, string Label)
{
    public static IReadOnlyList<AvailabilityOption> All { get; } =
    [
        new(R3eSetupControlAvailability.Adjustable, "Adjustable"),
        new(R3eSetupControlAvailability.Fixed, "Fixed"),
        new(R3eSetupControlAvailability.NotFitted, "Not fitted")
    ];
}
