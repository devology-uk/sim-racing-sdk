namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetryGeneral
{
    public PmrVector3 CenterOfGravityMeters { get; init; } = new();
    public float SteeringWheelAngleDegrees { get; init; }
    public float TotalMassKg { get; init; }
    public float DrivenWheelAngularVelocityRadiansPerSecond { get; init; }
    public float NonDrivenWheelAngularVelocityRadiansPerSecond { get; init; }
    public float EstimatedRollingSpeedMetersPerSecond { get; init; }
    public float EstimatedLinearSpeedMetersPerSecond { get; init; }
    public float TotalBrakeForceNewtons { get; init; }
    public bool AbsActive { get; init; }
}
