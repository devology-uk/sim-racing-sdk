namespace SimRacingSdk.R3e.DataManager.Cars;

// The only drive types the game's car info line shows.
public static class Drivetrains
{
    public const string Default = "RWD";

    public static IReadOnlyList<string> All { get; } = ["FWD", "RWD", "AWD"];
}
