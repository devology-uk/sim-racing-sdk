using SimRacingSdk.Pmr.Setups.Models.Files;

namespace SimRacingSdk.Pmr.Setups.Models.Presentation;

// A saved setup laid out like the game's Vehicle Setup screen: tab, section, field, then one value
// per position.
public record PmrSetupView
{
    public required PmrSetupFileName FileName { get; init; }

    // False when the car has no setup map at all; Tabs is then empty and MapExplanation says why.
    public required bool HasMap { get; init; }

    // False when any value is NotMapped - the map for this car is still being captured.
    public required bool IsFullyMapped { get; init; }

    public string? MapExplanation { get; init; }
    public required IReadOnlyList<PmrSetupTabView> Tabs { get; init; }
    public required PmrUnitSystem UnitSystem { get; init; }
}
