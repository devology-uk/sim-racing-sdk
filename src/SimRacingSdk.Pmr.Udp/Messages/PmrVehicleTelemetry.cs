namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetry
{
    public ushort PacketVersion { get; init; }
    public int VehicleId { get; init; }
    public IReadOnlyList<PmrVehicleTelemetryWheel> Wheels { get; init; } = [];
    public PmrVehicleTelemetryChassis Chassis { get; init; } = new();
    public PmrVehicleTelemetryDrivetrain Drivetrain { get; init; } = new();
    public PmrVehicleTelemetrySuspension Suspension { get; init; } = new();
    public PmrVehicleTelemetryInput Input { get; init; } = new();
    public PmrVehicleTelemetrySetup Setup { get; init; } = new();
    public PmrVehicleTelemetryGeneral General { get; init; } = new();
    public PmrVehicleTelemetryConstant Constant { get; init; } = new();
}
