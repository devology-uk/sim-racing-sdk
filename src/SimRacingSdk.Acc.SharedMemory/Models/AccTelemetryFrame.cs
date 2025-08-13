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
        this.BrakeTempFl = physicsData.BrakeTemperature[0];
        this.BrakeTempFr = physicsData.BrakeTemperature[1];
        this.BrakeTempRl = physicsData.BrakeTemperature[2];
        this.BrakeTempRr = physicsData.BrakeTemperature[3];
        this.CarId = staticData.CarModel;
        this.DriverDisplayName = $"{staticData.PlayerName[..1]}. {staticData.PlayerSurname}";
        this.DriverDisplayName = $"{staticData.PlayerName} {staticData.PlayerSurname}";
        this.Fuel = physicsData.Fuel;
        this.Gear = physicsData.Gear;
        this.IsInvalid = !graphicsData.IsValidLap;
        this.LapTimeMs = graphicsData.CurrentTimeMs;
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
        this.LocationX = physicsData.TyreContactPoint[0]
                                    .X;
        this.LocationY = physicsData.TyreContactPoint[0]
                                    .Z;
        this.IsEngineRunning = physicsData.IsEngineRunning;
        this.TractionControl = physicsData.TractionControl;
    }

    public float Abs { get; }
    public float Accelerator { get; }
    public float Brake { get; }
    public float BrakeTempFl { get; }
    public float BrakeTempFr { get; }
    public float BrakeTempRl { get; }
    public float BrakeTempRr { get; }
    public string CarId { get; }
    public string DriverDisplayName { get; }
    public float Fuel { get; }
    public int Gear { get; }
    public bool IsEngineRunning { get; }
    public bool IsInvalid { get; }
    public int LapTimeMs { get; }
    public float NormalisedCarPosition { get; }
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
    public float LocationX { get; set; }
    public float LocationY { get; set; }

    public override string ToString()
    {
        return
            $"ACC Telemetry Frame: Accelerator: {this.Accelerator} Brake: {this.Brake} RPM: {this.Rpm} Speed KMH: {this.SpeedKmh} Lap Time: {this.LapTimeMs.ToTimingStringFromMilliseconds()} Sector Time: {this.SplitTimeMs.ToTimingStringFromMilliseconds()}";
    }
}