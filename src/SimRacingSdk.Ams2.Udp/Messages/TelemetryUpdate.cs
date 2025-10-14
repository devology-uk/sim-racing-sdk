#nullable disable

using SimRacingSdk.Ams2.Udp.Abstractions;

namespace SimRacingSdk.Ams2.Udp.Messages;

public record TelemetryUpdate : MessageBase
{
    public TelemetryUpdate(MessageHeader header)
        : base(header) { }

    public byte AeroDamage { get; internal set; }
    public short[] AirPressure { get; internal set; }
    public int[] AngularVelocity { get; internal set; }
    public int AvailableGears { get; internal set; }
    public byte BoostAmount { get; internal set; }
    public byte Brake { get; internal set; }
    public byte BrakeBias { get; internal set; }
    public byte[] BrakeDamage { get; internal set; }
    public short[] BrakeTempC { get; internal set; }
    public byte CarFlags { get; internal set; }
    public byte Clutch { get; internal set; }
    public byte CrashState { get; internal set; }
    public byte DPad { get; internal set; }
    public byte EngineDamage { get; internal set; }
    public int EngineSpeed { get; internal set; }
    public int EngineTorque { get; internal set; }
    public int[] ExtentsCentre { get; internal set; }
    public byte FuelCapacity { get; internal set; }
    public int FuelLevel { get; internal set; }
    public short FuelPressureKPa { get; internal set; }
    public int[] FullPosition { get; internal set; }
    public int Gear { get; internal set; }
    public byte Handbrake { get; internal set; }
    public int JoyPad0 { get; internal set; }
    public int[] LocalAcceleration { get; internal set; }
    public int[] LocalVelocity { get; internal set; }
    public short MaxRpm { get; internal set; }
    public int OdometerKmh { get; internal set; }
    public short OilPressureKPa { get; internal set; }
    public short OilTempC { get; internal set; }
    public int[] Orientation { get; internal set; }
    public byte ParticipantIndex { get; internal set; }
    public int[] RideHeight { get; internal set; }
    public short Rpm { get; internal set; }
    public int Speed { get; internal set; }
    public byte Steering { get; internal set; }
    public byte[] SuspensionDamage { get; internal set; }
    public short[] SuspensionRideHeight { get; internal set; }
    public int[] SuspensionTravel { get; internal set; }
    public int[] SuspensionVelocity { get; internal set; }
    public byte[] Terrain { get; internal set; }
    public byte Throttle { get; internal set; }
    public int TickCount { get; internal set; }
    public int TurboBoostPressure { get; internal set; }
    public short[] TyreCarcassTempC { get; internal set; }
    public string TyreCompound { get; internal set; }
    public byte[] TyreFlags { get; internal set; }
    public int[] TyreHeightAboveGround { get; internal set; }
    public short[] TyreInternalAirTempC { get; internal set; }
    public short[] TyreLayerTempC { get; internal set; }
    public short[] TyreRimTempC { get; internal set; }
    public int[] TyreRps { get; internal set; }
    public byte[] TyreTemp { get; internal set; }
    public short[] TyreTempCentreC { get; internal set; }
    public short[] TyreTempLeftC { get; internal set; }
    public short[] TyreTempRightC { get; internal set; }
    public short[] TyreTreadTempC { get; internal set; }
    public byte[] TyreWear { get; internal set; }
    public int[] TyreY { get; internal set; }
    public byte UnfilteredBrake { get; internal set; }
    public byte UnfilteredClutch { get; internal set; }
    public byte UnfilteredSteering { get; internal set; }
    public byte UnfilteredThrottle { get; internal set; }
    public short WaterTempC { get; internal set; }
    public int[] WheelLocalPositionY { get; internal set; }
    public byte[] Wings { get; internal set; }
    public int[] WorldAcceleration { get; internal set; }
    public int[] WorldVelocity { get; internal set; }
    public short WaterPressureKPa { get; set; }
}