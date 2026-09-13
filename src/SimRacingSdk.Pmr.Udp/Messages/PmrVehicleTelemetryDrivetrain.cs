namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetryDrivetrain
{
    public float EngineRpm { get; init; }
    public float EngineRevRatio { get; init; }
    public float EngineTorque { get; init; }
    public float EnginePower { get; init; }
    public float EngineLoad { get; init; }
    public float EngineTurboRpm { get; init; }
    public float EngineTurboBoostPressure { get; init; }
    public float FuelRemaining { get; init; }
    public float FuelUseRate { get; init; }
    public float EngineOilPressure { get; init; }
    public float EngineOilTemperature { get; init; }
    public float EngineCoolantTemperature { get; init; }
    public float ExhaustGasTemperature { get; init; }
    public float MotorRpm { get; init; }
    public float BatteryRemaining { get; init; }
    public float BatteryUseRate { get; init; }
    public float TransmissionRpm { get; init; }
    public float GearboxInputRpm { get; init; }
    public float GearboxOutputRpm { get; init; }
    public float GearboxTorque { get; init; }
    public float GearboxPower { get; init; }
    public float GearboxLoadIn { get; init; }
    public float GearboxLoadOut { get; init; }
    public float TimeSinceShift { get; init; }
    public float EstimatedDrivenSpeed { get; init; }
    public float OutputTorque { get; init; }
    public float OutputPower { get; init; }
    public float OutputEfficiency { get; init; }
    public bool StarterActive { get; init; }
    public bool EngineRunning { get; init; }
    public bool EngineFanRunning { get; init; }
    public bool RevLimiterActive { get; init; }
    public bool TractionControlActive { get; init; }
    public bool SpeedLimiterEnabled { get; init; }
    public bool SpeedLimiterActive { get; init; }
    public IReadOnlyList<PmrVehicleTelemetryGear> Gears { get; init; } = [];
}
