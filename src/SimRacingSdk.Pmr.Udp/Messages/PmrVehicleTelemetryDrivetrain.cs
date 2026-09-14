namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetryDrivetrain
{
    public float EngineRpm { get; init; }
    public float EngineRevRatio { get; init; }
    public float EngineTorqueNm { get; init; }
    public float EnginePowerKw { get; init; }
    public float EngineLoadFraction { get; init; }
    public float EngineTurboRpm { get; init; }

    // Not empirically confirmed - see PmrVehicleTelemetryWheel.PressureKpa.
    public float EngineTurboBoostPressureKpa { get; init; }

    public float FuelRemainingLitres { get; init; }
    public float FuelUseRateLitresPerSecond { get; init; }

    // Not empirically confirmed - see PmrVehicleTelemetryWheel.PressureKpa.
    public float EngineOilPressureKpa { get; init; }

    public float EngineOilTemperatureCelsius { get; init; }
    public float EngineCoolantTemperatureCelsius { get; init; }
    public float ExhaustGasTemperatureCelsius { get; init; }
    public float MotorRpm { get; init; }

    // Not empirically confirmed - no hybrid/EV car has been captured yet to sanity-check
    // magnitude against. Joules is a best guess inferred from the similarly large raw number
    // seen in a hybrid car's default.vset "ev-battery-remaining" value while building the car
    // catalog importer (~3.88 million for one LMDh car).
    public float BatteryRemainingJoules { get; init; }

    public float BatteryUseRateJoulesPerSecond { get; init; }

    public float TransmissionRpm { get; init; }
    public float GearboxInputRpm { get; init; }
    public float GearboxOutputRpm { get; init; }
    public float GearboxTorqueNm { get; init; }
    public float GearboxPowerKw { get; init; }
    public float GearboxLoadInFraction { get; init; }
    public float GearboxLoadOutFraction { get; init; }
    public float TimeSinceShiftSeconds { get; init; }
    public float EstimatedDrivenSpeedMetersPerSecond { get; init; }
    public float OutputTorqueNm { get; init; }
    public float OutputPowerKw { get; init; }
    public float OutputEfficiencyFraction { get; init; }
    public bool StarterActive { get; init; }
    public bool EngineRunning { get; init; }
    public bool EngineFanRunning { get; init; }
    public bool RevLimiterActive { get; init; }
    public bool TractionControlActive { get; init; }
    public bool SpeedLimiterEnabled { get; init; }
    public bool SpeedLimiterActive { get; init; }
    public IReadOnlyList<PmrVehicleTelemetryGear> Gears { get; init; } = [];
}
