namespace SimRacingSdk.R3e.Setups.Models;

// How one control appears on one car. Min, Step and Max are Metric display values, as the game shows them
// with its Measurement Units setting on Metric (brake bias as the front share), and are optional: a map is
// useful with availability alone.
public record R3eSetupControlSetting
{
    public required R3eSetupControl Control { get; init; }
    public required R3eSetupControlAvailability Availability { get; init; }
    public double? Max { get; init; }
    public double? Min { get; init; }

    // A Choice control's options in the game's order, e.g. ["Medium", "Hard"].
    public IReadOnlyList<string>? Options { get; init; }

    public double? Step { get; init; }
}
