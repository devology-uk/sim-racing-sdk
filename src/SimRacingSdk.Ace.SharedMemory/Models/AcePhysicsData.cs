#nullable disable

using SimRacingSdk.Ace.SharedMemory.Messages;

namespace SimRacingSdk.Ace.SharedMemory.Models;

public record AcePhysicsData
{
    private readonly AcePhysicsPage physicsPage;

    internal AcePhysicsData()
    {
        this.IsEmpty = true;
    }

    internal AcePhysicsData(AcePhysicsPage physicsPage)
    {
        this.physicsPage = physicsPage;
        this.PacketId = physicsPage.PacketId;
        this.Accelerator = physicsPage.Gas;
        this.Brake = physicsPage.Brake;
        this.Fuel = physicsPage.Fuel;
        this.Gear = physicsPage.Gear;
        this.Rpm = physicsPage.Rpm;
        this.SteerAngle = physicsPage.SteerAngle;
        this.SpeedKmh = physicsPage.SpeedKmh;
        this.Velocity = physicsPage.Velocity;
        this.AccG = physicsPage.AccG;
        this.WheelSlip = physicsPage.WheelSlip;
        this.WheelLoad = physicsPage.WheelLoad;
        this.WheelsPressure = physicsPage.WheelsPressure;
        this.WheelAngularSpeed = physicsPage.WheelAngularSpeed;
        this.TyreWear = physicsPage.TyreWear;
        this.TyreCoreTemperature = physicsPage.TyreCoreTemperature;
        this.SuspensionTravel = physicsPage.SuspensionTravel;
        this.Drs = physicsPage.Drs;
        this.TractionControl = physicsPage.Tc;
        this.Heading = physicsPage.Heading;
        this.Pitch = physicsPage.Pitch;
        this.Roll = physicsPage.Roll;
        this.CarDamage = physicsPage.CarDamage;
        this.PitLimiterOn = physicsPage.PitLimiterOn != 0;
        this.Abs = physicsPage.Abs;
        this.TurboBoost = physicsPage.TurboBoost;
        this.AirTemp = physicsPage.AirTemp;
        this.RoadTemp = physicsPage.RoadTemp;
        this.BrakeTemperature = physicsPage.BrakeTemperature;
        this.Clutch = physicsPage.Clutch;
        this.TyreTempI = physicsPage.TyreTempI;
        this.TyreTempM = physicsPage.TyreTempM;
        this.TyreTempO = physicsPage.TyreTempO;
        this.IsAiControlled = physicsPage.IsAiControlled != 0;
        this.TyreContactPoints = physicsPage.TyreContactPoints;
        this.BrakeBias = physicsPage.BrakeBias;
        this.LocalVelocity = physicsPage.LocalVelocity;
        this.SlipRatio = physicsPage.SlipRatio;
        this.SlipAngle = physicsPage.SlipAngle;
        this.SuspensionDamage = physicsPage.SuspensionDamage;
        this.TyreTemp = physicsPage.TyreTemp;
        this.WaterTemp = physicsPage.WaterTemp;
        this.BrakeTorque = physicsPage.BrakeTorque;
        this.FrontBrakeCompound = physicsPage.FrontBrakeCompound;
        this.RearBrakeCompound = physicsPage.RearBrakeCompound;
        this.PadLife = physicsPage.PadLife;
        this.DiscLife = physicsPage.DiscLife;
        this.IgnitionOn = physicsPage.IgnitionOn != 0;
        this.IsEngineRunning = physicsPage.IsEngineRunning != 0;
    }

    public int PacketId { get; }
    public float Accelerator { get; }
    public float Brake { get; }
    public float Fuel { get; }
    public int Gear { get; }
    public int Rpm { get; }
    public float SteerAngle { get; }
    public float SpeedKmh { get; }
    public float[] Velocity { get; }
    public float[] AccG { get; }
    public float[] WheelSlip { get; }
    public float[] WheelLoad { get; }
    public float[] WheelsPressure { get; }
    public float[] WheelAngularSpeed { get; }
    public float[] TyreWear { get; }
    public float[] TyreCoreTemperature { get; }
    public float[] SuspensionTravel { get; }
    public float Drs { get; }
    public float TractionControl { get; }
    public float Heading { get; }
    public float Pitch { get; }
    public float Roll { get; }
    public float[] CarDamage { get; }
    public bool IsEmpty { get; }
    public bool PitLimiterOn { get; }
    public float Abs { get; }
    public float TurboBoost { get; }
    public float AirTemp { get; }
    public float RoadTemp { get; }
    public float[] BrakeTemperature { get; }
    public float Clutch { get; }
    public float[] TyreTempI { get; }
    public float[] TyreTempM { get; }
    public float[] TyreTempO { get; }
    public bool IsAiControlled { get; }
    public AceCoordinate3d[] TyreContactPoints { get; }
    public float BrakeBias { get; }
    public float[] LocalVelocity { get; }
    public float[] SlipRatio { get; }
    public float[] SlipAngle { get; }
    public float[] SuspensionDamage { get; }
    public float[] TyreTemp { get; }
    public float WaterTemp { get; }
    public float[] BrakeTorque { get; }
    public int FrontBrakeCompound { get; }
    public int RearBrakeCompound { get; }
    public float[] PadLife { get; }
    public float[] DiscLife { get; }
    public bool IgnitionOn { get; }
    public bool IsEngineRunning { get; }
    public DateTime TimeStamp { get; } = DateTime.UtcNow;
}
