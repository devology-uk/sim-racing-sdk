namespace SimRacingSdk.R3e.Setups.Models;

// How one control appears on one car. Min, Step and Max are Metric display values, as the game shows them
// with its Measurement Units setting on Metric (brake bias as the front share), and are optional: a map is
// useful with availability alone.
public record R3eSetupControlSetting
{
    public required R3eSetupControl Control { get; init; }
    public required R3eSetupControlAvailability Availability { get; init; }

    // How many forward gears a GearSet control has.
    public int? GearCount { get; init; }

    public double? Max { get; init; }
    public double? Min { get; init; }

    // A Choice control's options in the game's order, e.g. ["Medium", "Hard"]. For a GearSet, every tooth pair the
    // gears can choose from ("18:34"), in the order the game steps through them. The list is shared by all the gears,
    // and each gear is held between its neighbours, so it's longer than the gear count. A fixed gear set lists one
    // pair per gear.
    public IReadOnlyList<string>? Options { get; init; }

    public double? Step { get; init; }
}
