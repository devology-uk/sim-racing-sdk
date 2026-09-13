namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrVehicleTelemetryWheel
{
    public int ContactMaterialHash { get; init; }
    public float AngularVelocity { get; init; }
    public float LinearSpeed { get; init; }
    public PmrVector3 SlideLocalSpace { get; init; } = new();
    public PmrVector3 ForceLocalSpace { get; init; } = new();
    public PmrVector3 MomentLocalSpace { get; init; } = new();
    public float ContactRadius { get; init; }
    public float Pressure { get; init; }
    public float Inclination { get; init; }
    public float SlipRatio { get; init; }
    public float SlipAngle { get; init; }
    public PmrVector3 Tread { get; init; } = new();
    public float Carcass { get; init; }
    public float InternalAir { get; init; }
    public float WellAir { get; init; }
    public float Rim { get; init; }
    public float Brake { get; init; }
    public float SpringStrain { get; init; }
    public float DamperVelocity { get; init; }
    public float HubTorque { get; init; }
    public float HubPower { get; init; }
    public float WheelTorque { get; init; }
    public float WheelPower { get; init; }
}
