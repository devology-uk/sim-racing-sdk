using SimRacingSdk.Core;

namespace SimRacingSdk.Acc.SharedMemory.Models;

public class AccTelemetryFrame
{
    public AccTelemetryFrame(StaticData staticData,
        GraphicsData graphicsData,
        PhysicsData physicsData,
        int actualSectorIndex)
    {
        this.Abs = physicsData.Abs;
        this.Accelerator = physicsData.Accelerator;
        this.Brake = physicsData.Brake;
        this.BrakeBias = physicsData.BrakeBias;
        this.BrakePressureFl = physicsData.BrakePressure[0];
        this.BrakePressureFr = physicsData.BrakePressure[1];
        this.BrakePressureRl = physicsData.BrakePressure[2];
        this.BrakePressureRr = physicsData.BrakePressure[3];
        this.BrakeTempFl = physicsData.BrakeTemperature[0];
        this.BrakeTempFr = physicsData.BrakeTemperature[1];
        this.BrakeTempRl = physicsData.BrakeTemperature[2];
        this.BrakeTempRr = physicsData.BrakeTemperature[3];
        this.CarId = staticData.CarModel;
        this.Clutch = physicsData.Clutch;
        this.DiscLifeFl = physicsData.DiscLife[0];
        this.DiscLifeFr = physicsData.DiscLife[1];
        this.DiscLifeRl = physicsData.DiscLife[2];
        this.DiscLifeRr = physicsData.DiscLife[3];
        this.DriverFirstName = staticData.PlayerName;
        this.DriverLastName = staticData.PlayerSurname;
        this.DriverDisplayName = $"{staticData.PlayerName[..1]}. {staticData.PlayerSurname}";
        this.DriverFullName = $"{staticData.PlayerName} {staticData.PlayerSurname}";
        this.FrontBrakeCompound = physicsData.FrontBrakeCompound;
        this.Fuel = physicsData.Fuel;
        this.Gear = physicsData.Gear;
        this.Heading = physicsData.Heading;
        this.IsEngineRunning = physicsData.IsEngineRunning;
        this.IsIgnitionOn = physicsData.IgnitionOn;
        this.IsInvalid = !graphicsData.IsValidLap;
        this.IsPitLimiterOn = physicsData.PitLimiterOn;
        this.LapTimeMs = graphicsData.CurrentTimeMs;
        this.LocationX = physicsData.TyreContactPoint[0].X;
        this.LocationY = physicsData.TyreContactPoint[0].Z;
        this.PadLifeFl = physicsData.PadLife[0];
        this.PadLifeFr = physicsData.PadLife[1];
        this.PadLifeRl = physicsData.PadLife[2];
        this.PadLifeRr = physicsData.PadLife[3];
        this.Pitch = physicsData.Pitch;
        this.RearBrakeCompound = physicsData.RearBrakeCompound;
        this.Roll = physicsData.Roll;
        this.Rpm = physicsData.Rpm;
        this.NormalisedCarPosition = graphicsData.NormalizedCarPosition;
        this.SectorIndex = actualSectorIndex;
        this.SlipAngleFl = physicsData.SlipAngle[0];
        this.SlipAngleFr = physicsData.SlipAngle[1];
        this.SlipAngleRl = physicsData.SlipAngle[2];
        this.SlipAngleRr = physicsData.SlipAngle[3];
        this.SlipRatioFl = physicsData.SlipRatio[0];
        this.SlipRatioFr = physicsData.SlipRatio[1];
        this.SlipRatioRl = physicsData.SlipRatio[2];
        this.SlipRatioRr = physicsData.SlipRatio[3];
        this.SpeedKmh = physicsData.SpeedKmh;
        this.SplitTimeMs = graphicsData.SplitTimeMs;
        this.SteeringAngle = physicsData.SteerAngle;
        this.TrackId = staticData.Track;
        this.TimeStamp = DateTime.UtcNow;
        this.TractionControl = physicsData.TractionControl;
        this.TurboBoost = physicsData.TurboBoost;
        this.TyreCoreTempFl = physicsData.TyreCoreTemperature[0];
        this.TyreCoreTempFr = physicsData.TyreCoreTemperature[1];
        this.TyreCoreTempRl = physicsData.TyreCoreTemperature[2];
        this.TyreCoreTempRr = physicsData.TyreCoreTemperature[3];
        this.TyrePressureFl = physicsData.WheelPressure[0];
        this.TyrePressureFr = physicsData.WheelPressure[1];
        this.TyrePressureRl = physicsData.WheelPressure[2];
        this.TyrePressureRr = physicsData.WheelPressure[3];
        this.TyreTempFl = physicsData.TyreTemp[0];
        this.TyreTempFr = physicsData.TyreTemp[1];
        this.TyreTempRl = physicsData.TyreTemp[2];
        this.TyreTempRr = physicsData.TyreTemp[3];
        this.WaterTemp = physicsData.WaterTemp;
        this.WheelSlipFl = physicsData.WheelSlip[0];
        this.WheelSlipFr = physicsData.WheelSlip[1];
        this.WheelSlipRl = physicsData.WheelSlip[2];
        this.WheelSlipRr = physicsData.WheelSlip[3];
    }

    public float Abs { get; }
    public float Accelerator { get; }
    public float Brake { get; }
    public float BrakeBias { get; }
    public float BrakePressureFl { get; }
    public float BrakePressureFr { get; }
    public float BrakePressureRl { get; }
    public float BrakePressureRr { get; }
    public float BrakeTempFl { get; }
    public float BrakeTempFr { get; }
    public float BrakeTempRl { get; }
    public float BrakeTempRr { get; }
    public string CarId { get; }
    public float Clutch { get; }
    public float DiscLifeFl { get; }
    public float DiscLifeFr { get; }
    public float DiscLifeRl { get; }
    public float DiscLifeRr { get; }
    public string DriverFirstName { get; }
    public string DriverLastName { get; }
    public string DriverDisplayName { get; }
    public string DriverFullName { get; }
    public int FrontBrakeCompound { get; }
    public float Fuel { get; }
    public int Gear { get; }
    public float Heading { get; }
    public bool IsEngineRunning { get; }
    public bool IsIgnitionOn { get; }
    public bool IsInvalid { get; }
    public bool IsPitLimiterOn { get; }
    public int LapTimeMs { get; }
    public float LocationX { get; }
    public float LocationY { get; }
    public float NormalisedCarPosition { get; }
    public float PadLifeFl { get; }
    public float PadLifeFr { get; }
    public float PadLifeRl { get; }
    public float PadLifeRr { get; }
    public float Pitch { get; }
    public int RearBrakeCompound { get; }
    public float Roll { get; }
    public int Rpm { get; }
    public int SectorIndex { get; }
    public float SlipAngleFl { get; }
    public float SlipAngleFr { get; }
    public float SlipAngleRl { get; }
    public float SlipAngleRr { get; }
    public float SlipRatioFl { get; }
    public float SlipRatioFr { get; }
    public float SlipRatioRl { get; }
    public float SlipRatioRr { get; }
    public float SpeedKmh { get; }
    public int SplitTimeMs { get; }
    public float SteeringAngle { get; }
    public DateTime TimeStamp { get; }
    public string TrackId { get; }
    public float TractionControl { get; }
    public float TurboBoost { get; }
    public float TyreCoreTempFl { get; }
    public float TyreCoreTempFr { get; }
    public float TyreCoreTempRl { get; }
    public float TyreCoreTempRr { get; }
    public float TyrePressureFl { get; }
    public float TyrePressureFr { get; }
    public float TyrePressureRl { get; }
    public float TyrePressureRr { get; }
    public float TyreTempFl { get; }
    public float TyreTempFr { get; }
    public float TyreTempRl { get; }
    public float TyreTempRr { get; }
    public float WaterTemp { get; }
    public float WheelSlipFl { get; }
    public float WheelSlipFr { get; }
    public float WheelSlipRl { get; }
    public float WheelSlipRr { get; }

    public override string ToString()
    {
        return
            $"ACC Telemetry Frame: Accelerator: {this.Accelerator} Brake: {this.Brake} RPM: {this.Rpm} Speed KMH: {this.SpeedKmh} Lap Time: {this.LapTimeMs.ToTimingStringFromMilliseconds()} Sector Time: {this.SplitTimeMs.ToTimingStringFromMilliseconds()}";
    }
}