namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

// One file per car (data/pmr/setupmaps/{CarId}.json), matching the game's own "Vehicle Setup"
// screen's tab layout - Essentials is a telemetry summary with no real setup input, so it has no
// list here. Pit Setup is also left out: the game saves it to its own <GameId>-<name>.pset file
// (1-based indexes into each option list, including the refuel amount) rather than the .vset, so it
// needs its own handling, still to be designed.
public record PmrSetupMap
{
    public required string CarId { get; init; }
    public required List<SetupFieldInfo> EngineAndDrivetrain { get; init; } = [];
    public required List<SetupFieldInfo> SteeringWheel { get; init; } = [];
    public required List<SetupFieldInfo> Suspension { get; init; } = [];
    public required List<SetupFieldInfo> TyresAndChassis { get; init; } = [];
}
