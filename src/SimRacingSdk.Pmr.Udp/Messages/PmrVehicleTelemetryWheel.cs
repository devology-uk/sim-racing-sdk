namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetryWheel
{
    public int ContactMaterialHash { get; init; }
    public float AngularVelocityRadiansPerSecond { get; init; }
    public float LinearSpeedMetersPerSecond { get; init; }
    public PmrVector3 SlideLocalSpaceMetersPerSecond { get; init; } = new();
    public PmrVector3 ForceLocalSpaceNewtons { get; init; } = new();
    public PmrVector3 MomentLocalSpaceNm { get; init; } = new();
    public float ContactRadiusMeters { get; init; }

    // Not empirically confirmed - no captured session has shown a value to sanity-check
    // magnitude against (unlike Torque/Power/Mass/Meters below, all cross-checked against real
    // MX-5 specs from the 2026-09-13 rig log).
    public float PressureKpa { get; init; }

    public float InclinationRadians { get; init; }
    public float SlipRatio { get; init; }
    public float SlipAngleRadians { get; init; }
    public PmrVector3 TreadTemperatureCelsius { get; init; } = new();
    public float CarcassTemperatureCelsius { get; init; }
    public float InternalAirTemperatureCelsius { get; init; }
    public float WellAirTemperatureCelsius { get; init; }
    public float RimTemperatureCelsius { get; init; }
    public float BrakeTemperatureCelsius { get; init; }
    public float SpringStrainFraction { get; init; }
    public float DamperVelocityMetersPerSecond { get; init; }
    public float HubTorqueNm { get; init; }
    public float HubPowerKw { get; init; }
    public float WheelTorqueNm { get; init; }
    public float WheelPowerKw { get; init; }
}
