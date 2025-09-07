#nullable disable

using SimRacingSdk.Acc.SharedMemory.Messages;

namespace SimRacingSdk.Acc.SharedMemory.Models;

public record PhysicsData
{
    internal PhysicsData() { }

    internal PhysicsData(PhysicsPage physicsPage)
    {
        this.Abs = physicsPage.Abs;
        this.AbsVibrations = physicsPage.AbsVibrations;
        this.Accelerator = physicsPage.Gas;
        this.AccG = physicsPage.AccelerationVector;
        this.AirTemp = physicsPage.AirTemp;
        this.AutoShiftOn = physicsPage.AutoShifterOn;
        this.Brake = physicsPage.Brake;
        this.BrakeBias = physicsPage.BrakeBias;
        this.BrakePressure = physicsPage.BrakePressure;
        this.BrakeTemperature = physicsPage.BrakeTemperature;
        this.CarDamage = physicsPage.CarDamage;
        this.Clutch = physicsPage.Clutch;
        this.DiscLife = physicsPage.DiscLife;
        this.FinalFf = physicsPage.ForceFeedbackSignal;
        this.FrontBrakeCompound = physicsPage.FrontBrakeCompound;
        this.Fuel = physicsPage.Fuel;
        this.Gear = physicsPage.Gear;
        this.GearVibrations = physicsPage.GearVibrations;
        this.Heading = physicsPage.Heading;
        this.IgnitionOn = physicsPage.IgnitionOn;
        this.IsAiControlled = physicsPage.IsAiControlled;
        this.IsEngineRunning = physicsPage.IsEngineRunning;
        this.KerbVibration = physicsPage.KerbVibrations;
        this.LocalAngularVelocity = physicsPage.LocalAngularVelocity;
        this.LocalVelocity = physicsPage.LocalVelocity;
        this.PacketId = physicsPage.PacketId;
        this.PadLife = physicsPage.PadLife;
        this.Pitch = physicsPage.Pitch;
        this.PitLimiterOn = physicsPage.PitLimiterOn;
        this.RearBrakeCompound = physicsPage.RearBrakeCompound;
        this.Roll = physicsPage.Roll;
        this.Rpm = physicsPage.Rpm;
        this.SlipAngle = physicsPage.SlipAngle;
        this.SlipRatio = physicsPage.SlipRatio;
        this.SlipVibrations = physicsPage.SlipVibrations;
        this.SpeedKmh = physicsPage.SpeedKmh;
        this.StarterEngineOn = physicsPage.StarterEngineOn;
        this.SteerAngle = physicsPage.SteerAngle;
        this.SuspensionDamage = physicsPage.SuspensionDamage;
        this.SuspensionTravel = physicsPage.SuspensionTravel;
        this.TrackTemp = physicsPage.RoadTemp;
        this.TractionControl = physicsPage.TC;
        this.TurboBoost = physicsPage.TurboBoost;
        this.TyreContactHeading = physicsPage.TyreContactHeadings;
        this.TyreContactNormal = physicsPage.TyreContactNormals;
        this.TyreContactPoint = physicsPage.TyreContactPoints;
        this.TyreCoreTemperature = physicsPage.TyreCoreTemperature;
        this.TyreTemp = physicsPage.TyreTemp;
        this.Velocity = physicsPage.Velocity;
        this.WaterTemp = physicsPage.WaterTemp;
        this.WheelAngularSpeed = physicsPage.WheelAngularSpeed;
        this.WheelPressure = physicsPage.WheelPressure;
        this.WheelSlip = physicsPage.WheelSlip;
    }

    public float Abs { get; }
    public float AbsVibrations { get; }
    public float Accelerator { get; }
    public float[] AccG { get; }
    public float AirTemp { get; }
    public bool AutoShiftOn { get; }
    public float Brake { get; }
    public float BrakeBias { get; }
    public float[] BrakePressure { get; }
    public float[] BrakeTemperature { get; }
    public float[] CarDamage { get; }
    public float Clutch { get; }
    public float[] DiscLife { get; }
    public float FinalFf { get; }
    public int FrontBrakeCompound { get; }
    public float Fuel { get; }
    public int Gear { get; }
    public float GearVibrations { get; }
    public float Heading { get; }
    public bool IgnitionOn { get; }
    public bool IsAiControlled { get; }
    public bool IsEngineRunning { get; }
    public float KerbVibration { get; }
    public float[] LocalAngularVelocity { get; }
    public float[] LocalVelocity { get; }
    public int PacketId { get; }
    public float[] PadLife { get; }
    public float Pitch { get; }
    public bool PitLimiterOn { get; }
    public int RearBrakeCompound { get; }
    public float Roll { get; }
    public int Rpm { get; }
    public float[] SlipAngle { get; }
    public float[] SlipRatio { get; }
    public float SlipVibrations { get; }
    public float SpeedKmh { get; }
    public bool StarterEngineOn { get; }
    public float SteerAngle { get; }
    public float[] SuspensionDamage { get; }
    public float[] SuspensionTravel { get; }
    public DateTime TimeStamp { get; } = DateTime.UtcNow;
    public float TrackTemp { get; }
    public float TractionControl { get; }
    public float TurboBoost { get; }
    public AccCoordinate3d[] TyreContactHeading { get; }
    public AccCoordinate3d[] TyreContactNormal { get; }
    public AccCoordinate3d[] TyreContactPoint { get; }
    public float[] TyreCoreTemperature { get; }
    public float[] TyreTemp { get; }
    public float[] Velocity { get; }
    public float WaterTemp { get; }
    public float[] WheelAngularSpeed { get; }
    public float[] WheelPressure { get; }
    public float[] WheelSlip { get; }
}