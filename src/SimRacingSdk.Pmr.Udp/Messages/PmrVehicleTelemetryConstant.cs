namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetryConstant
{
    public PmrVector3 ChassisBoundingBoxMinMeters { get; init; } = new();
    public PmrVector3 ChassisBoundingBoxMaxMeters { get; init; } = new();
    public float StarterIdleRpm { get; init; }
    public float EngineTorquePeakRpm { get; init; }
    public float EnginePowerPeakRpm { get; init; }
    public float EngineMaxRpm { get; init; }
    public float EngineMaxTorqueNm { get; init; }
    public float EngineMaxPowerKw { get; init; }

    // Not empirically confirmed - see PmrVehicleTelemetryWheel.PressureKpa.
    public float EngineMaxBoostKpa { get; init; }

    public float FuelCapacityLitres { get; init; }

    // Not empirically confirmed - see PmrVehicleTelemetryDrivetrain.BatteryRemainingJoules.
    public float BatteryCapacityJoules { get; init; }

    public float TrackWidthFrontMeters { get; init; }
    public float TrackWidthRearMeters { get; init; }
    public float WheelbaseMeters { get; init; }
    public byte NumberOfWheels { get; init; }
    public byte NumberOfForwardGears { get; init; }
    public byte NumberOfReverseGears { get; init; }
    public bool IsHybrid { get; init; }
}
