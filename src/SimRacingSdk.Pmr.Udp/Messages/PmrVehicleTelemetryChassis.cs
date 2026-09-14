namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetryChassis
{
    public PmrVector3 PositionWorldSpaceMeters { get; init; } = new();
    public PmrQuaternion Orientation { get; init; } = new();
    public PmrVector3 AngularVelocityWorldSpaceRadiansPerSecond { get; init; } = new();
    public PmrVector3 AngularVelocityLocalSpaceRadiansPerSecond { get; init; } = new();
    public PmrVector3 VelocityWorldSpaceMetersPerSecond { get; init; } = new();
    public PmrVector3 VelocityLocalSpaceMetersPerSecond { get; init; } = new();
    public PmrVector3 AccelerationWorldSpaceMetersPerSecondSquared { get; init; } = new();
    public PmrVector3 AccelerationLocalSpaceMetersPerSecondSquared { get; init; } = new();
    public float OverallSpeedMetersPerSecond { get; init; }
    public float ForwardSpeedMetersPerSecond { get; init; }
    public float SideslipRadians { get; init; }
}
