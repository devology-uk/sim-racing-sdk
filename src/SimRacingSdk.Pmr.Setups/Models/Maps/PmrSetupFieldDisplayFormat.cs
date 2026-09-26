namespace SimRacingSdk.Pmr.Setups.Models.Maps;

public enum PmrSetupFieldDisplayFormat
{
    Standard,

    // Brake Bias: the front share as "56.0F / 44.0R".
    FrontRearSplit,

    // Gear ratios and Final Drive: "2.385 : 1". An Enum's entries are just the numbers.
    Ratio
}
