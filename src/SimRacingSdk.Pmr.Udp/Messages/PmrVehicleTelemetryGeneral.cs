namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetryGeneral
{
    public PmrVector3 CenterOfGravity { get; init; } = new();
    public float SteeringWheelAngle { get; init; }
    public float TotalMass { get; init; }
    public float DrivenWheelAngularVelocity { get; init; }
    public float NonDrivenWheelAngularVelocity { get; init; }
    public float EstimatedRollingSpeed { get; init; }
    public float EstimatedLinearSpeed { get; init; }
    public float TotalBrakeForce { get; init; }
    public bool AbsActive { get; init; }
}
