namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetryChassis
{
    public PmrVector3 PositionWorldSpace { get; init; } = new();
    public PmrQuaternion Orientation { get; init; } = new();
    public PmrVector3 AngularVelocityWorldSpace { get; init; } = new();
    public PmrVector3 AngularVelocityLocalSpace { get; init; } = new();
    public PmrVector3 VelocityWorldSpace { get; init; } = new();
    public PmrVector3 VelocityLocalSpace { get; init; } = new();
    public PmrVector3 AccelerationWorldSpace { get; init; } = new();
    public PmrVector3 AccelerationLocalSpace { get; init; } = new();
    public float OverallSpeed { get; init; }
    public float ForwardSpeed { get; init; }
    public float Sideslip { get; init; }
}
