namespace SimRacingSdk.Pmr.Setups.Models.Presentation;

public enum PmrSetupValueStatus
{
    // DisplayText is exactly what the game shows.
    Shown,

    // The game calculates what it shows and never saves it; show Explanation instead of a number.
    ShownInGameOnly,

    // This car's setup map doesn't yet cover this value; show Explanation instead of a number.
    NotMapped,

    // The map expects a value this setup file doesn't contain.
    NotInSetupFile
}
