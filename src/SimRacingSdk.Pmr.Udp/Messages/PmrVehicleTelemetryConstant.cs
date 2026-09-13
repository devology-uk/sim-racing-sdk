namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetryConstant
{
    public PmrVector3 ChassisBoundingBoxMin { get; init; } = new();
    public PmrVector3 ChassisBoundingBoxMax { get; init; } = new();
    public float StarterIdleRpm { get; init; }
    public float EngineTorquePeakRpm { get; init; }
    public float EnginePowerPeakRpm { get; init; }
    public float EngineMaxRpm { get; init; }
    public float EngineMaxTorque { get; init; }
    public float EngineMaxPower { get; init; }
    public float EngineMaxBoost { get; init; }
    public float FuelCapacity { get; init; }
    public float BatteryCapacity { get; init; }
    public float TrackWidthFront { get; init; }
    public float TrackWidthRear { get; init; }
    public float Wheelbase { get; init; }
    public byte NumberOfWheels { get; init; }
    public byte NumberOfForwardGears { get; init; }
    public byte NumberOfReverseGears { get; init; }
    public bool IsHybrid { get; init; }
}
