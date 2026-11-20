namespace SimRacingSdk.R3e.Setups.Models;

public enum R3eSetupValueKind
{
    // One value with a min, step and max.
    Number,

    // A pick from a list, e.g. tyre compound, or final gear, whose ratios aren't evenly spaced.
    Choice,

    // One value per in-car level (six for TC and ABS), each with the same min, step and max.
    // The in-car TC/ABS switch picks a row; the setup adjusts the rows.
    LevelTable,

    // Each gear's ratio, shown as tooth counts ("20:33 (1.650)"), so it's a set of gear pairs, not a range.
    GearSet
}
