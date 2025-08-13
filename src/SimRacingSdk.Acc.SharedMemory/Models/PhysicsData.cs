using SimRacingSdk.Acc.SharedMemory.Messages;

namespace SimRacingSdk.Acc.SharedMemory.Models;

public class PhysicsData(PhysicsPage physicsPage)
{
    public DateTime TimeStamp { get; } = DateTime.UtcNow;
    public float Abs { get; set; } = physicsPage.Abs;
    public float AbsVibrations { get; set; } = physicsPage.AbsVibrations;
    public float Accelerator { get; set; } = physicsPage.Gas;
    public float[] AccG { get; set; } = physicsPage.AccG;
    public float AirTemp { get; set; } = physicsPage.AirTemp;
    public bool AutoShiftOn { get; set; } = physicsPage.AutoShifterOn;
    public float Brake { get; set; } = physicsPage.Brake;
    public float BrakeBias { get; set; } = physicsPage.BrakeBias;
    public float[] BrakePressure { get; set; } = physicsPage.BrakePressure;
    public float[] BrakeTemperature { get; set; } = physicsPage.BrakeTemperature;
    public float[] CarDamage { get; set; } = physicsPage.CarDamage;
    public float Clutch { get; set; } = physicsPage.Clutch;
    public float[] DiscLife { get; set; } = physicsPage.DiscLife;
    public float FinalFf { get; set; } = physicsPage.FinalFF;
    public int FrontBrakeCompound { get; set; } = physicsPage.FrontBrakeCompound;
    public float Fuel { get; set; } = physicsPage.Fuel;
    public int Gear { get; set; } = physicsPage.Gear;
    public float GearVibrations { get; set; } = physicsPage.GVibrations;
    public float Heading { get; set; } = physicsPage.Heading;
    public bool IgnitionOn { get; set; } = physicsPage.IgnitionOn;
    public bool IsAiControlled { get; set; } = physicsPage.IsAiControlled;
    public bool IsEngineRunning { get; set; } = physicsPage.IsEngineRunning;
    public float KerbVibration { get; set; } = physicsPage.KerbVibration;
    public float[] LocalAngularVelocity { get; set; } = physicsPage.LocalAngularVelocity;
    public float[] LocalVelocity { get; set; } = physicsPage.LocalVelocity;
    public int PacketId { get; set; } = physicsPage.PacketId;
    public float[] PadLife { get; set; } = physicsPage.PadLife;
    public float Pitch { get; set; } = physicsPage.Pitch;
    public bool PitLimiterOn { get; set; } = physicsPage.PitLimiterOn;
    public int RearBrakeCompound { get; set; } = physicsPage.RearBrakeCompound;
    public float Roll { get; set; } = physicsPage.Roll;
    public int Rpm { get; set; } = physicsPage.Rpm;
    public float[] SlipAngle { get; set; } = physicsPage.SlipAngle;
    public float[] SlipRatio { get; set; } = physicsPage.SlipRatio;
    public float SlipVibrations { get; set; } = physicsPage.SlipVibrations;
    public float SpeedKmh { get; set; } = physicsPage.SpeedKmh;
    public bool StarterEngineOn { get; set; } = physicsPage.StarterEngineOn;
    public float SteerAngle { get; set; } = physicsPage.SteerAngle;
    public float[] SuspensionDamage { get; set; } = physicsPage.SuspensionDamage;
    public float[] SuspensionTravel { get; set; } = physicsPage.SuspensionTravel;
    public float TrackTemp { get; set; } = physicsPage.RoadTemp;
    public float TractionControl { get; set; } = physicsPage.TC;
    public float TurboBoost { get; set; } = physicsPage.TurboBoost;
    public AccRtVector3d[] TyreContactHeading { get; set; } = physicsPage.TyreContactHeading;
    public AccRtVector3d[] TyreContactNormal { get; set; } = physicsPage.TyreContactNormal;
    public AccRtVector3d[] TyreContactPoint { get; set; } = physicsPage.TyreContactPoint;
    public float[] TyreCoreTemperature { get; set; } = physicsPage.TyreCoreTemperature;
    public float[] TyreTemp { get; set; } = physicsPage.TyreTemp;
    public float[] Velocity { get; set; } = physicsPage.Velocity;
    public float WaterTemp { get; set; } = physicsPage.WaterTemp;
    public float[] WheelAngularSpeed { get; set; } = physicsPage.WheelAngularSpeed;
    public float[] WheelPressure { get; set; } = physicsPage.WheelPressure;
    public float[] WheelSlip { get; set; } = physicsPage.WheelSlip;

    public override string ToString()
    {
        return
            $"Physics Data Update: Time Stamp: {this.TimeStamp:hh:mm:ss.ffff}, Accelerator: {this.Accelerator}, Brake: {this.Brake}, Fuel: {this.Fuel}, Gear: {this.Gear}, RPM: {this.Rpm}, Steering Angle: {this.SteerAngle}, Speed KMH: {this.SpeedKmh}, X: {this.TyreContactPoint[0].X}, Y: {this.TyreContactPoint[0].Z}";
    }
}