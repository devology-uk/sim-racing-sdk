namespace SimRacingSdk.Pmr.Setups.Models.Maps;

// A car's Vehicle Setup screen: one field list per tab. Essentials (read-only telemetry) and Pit
// Setup (saved separately as a .pset) aren't part of it.
public record PmrSetupMap
{
    public required string CarId { get; init; }
    public IReadOnlyList<PmrSetupFieldMap> EngineAndDrivetrain { get; init; } = [];
    public IReadOnlyList<PmrSetupFieldMap> SteeringWheel { get; init; } = [];
    public IReadOnlyList<PmrSetupFieldMap> Suspension { get; init; } = [];
    public IReadOnlyList<PmrSetupFieldMap> TyresAndChassis { get; init; } = [];
    public string? VehicleGameId { get; init; }
}
