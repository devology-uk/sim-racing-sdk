namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

// One file per car (data/pmr/setupmaps/{CarId}.json), matching the game's own "Vehicle Setup"
// screen's tab layout - Essentials is a telemetry summary with no real setup input, so it has no
// list here.
public record PmrSetupMap
{
    public required string CarId { get; init; }
    public required List<SetupFieldInfo> EngineAndDrivetrain { get; init; } = [];
    public required List<SetupFieldInfo> PitSetup { get; init; } = [];
    public required List<SetupFieldInfo> SteeringWheel { get; init; } = [];
    public required List<SetupFieldInfo> Suspension { get; init; } = [];
    public required List<SetupFieldInfo> TyresAndChassis { get; init; } = [];
}
