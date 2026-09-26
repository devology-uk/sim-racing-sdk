namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

// How the game writes a numeric value on screen, where it isn't simply the number and its unit.
public enum SetupFieldDisplayFormat
{
    Standard,

    // Brake Bias: the front share as "56.0F / 44.0R".
    FrontRearSplit,

    // Gear ratios and Final Drive: "2.385 : 1". For an Enum, enter just the numbers - the ": 1" is added.
    Ratio
}
