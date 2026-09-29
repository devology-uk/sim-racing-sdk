namespace SimRacingSdk.Ace.SharedMemory.Models;

public record AceTelemetryFrame
{
    private readonly AceStaticData staticData;
    private readonly AceGraphicsData graphicsData;
    private readonly AcePhysicsData physicsData;

    public AceTelemetryFrame(AceStaticData staticData, AceGraphicsData graphicsData, AcePhysicsData physicsData)
    {
        this.staticData = staticData;
        this.graphicsData = graphicsData;
        this.physicsData = physicsData;

        this.Abs = physicsData.Abs;
        this.Accelerator = physicsData.Accelerator;
        this.Brake = physicsData.Brake;
        this.BrakeBias = physicsData.BrakeBias;
        this.BrakeTorqueFl = physicsData.BrakeTorque[0];
        this.BrakeTorqueFr = physicsData.BrakeTorque[1];
        this.BrakeTorqueRl = physicsData.BrakeTorque[2];
        this.BrakeTorqueRr = physicsData.BrakeTorque[3];
        this.BrakeTempFl = physicsData.BrakeTemperature[0];
        this.BrakeTempFr = physicsData.BrakeTemperature[1];
        this.BrakeTempRl = physicsData.BrakeTemperature[2];
        this.BrakeTempRr = physicsData.BrakeTemperature[3];
        this.CamberRadFl = physicsData.CamberRad[0];
        this.CamberRadFr = physicsData.CamberRad[1];
        this.CamberRadRl = physicsData.CamberRad[2];
        this.CamberRadRr = physicsData.CamberRad[3];
        this.CarModel = graphicsData.CarModel;
        this.Clutch = physicsData.Clutch;
        this.CurrentLapTimeMs = graphicsData.CurrentLapTimeMs;
        this.DriverFirstName = graphicsData.DriverName;
        this.DriverLastName = graphicsData.DriverSurname;
        this.DriverDisplayName = string.IsNullOrEmpty(graphicsData.DriverName)
            ? graphicsData.DriverSurname
            : $"{graphicsData.DriverName[..1]}. {graphicsData.DriverSurname}";
        this.DriverFullName = string.IsNullOrEmpty(graphicsData.DriverName)
            ? graphicsData.DriverSurname
            : $"{graphicsData.DriverName} {graphicsData.DriverSurname}";
        this.FrontBrakeCompound = physicsData.FrontBrakeCompound;
        this.FrontBumpDamperLevel = graphicsData.Electronics.FrontBumpDamperLevel;
        this.FrontReboundDamperLevel = graphicsData.Electronics.FrontReboundDamperLevel;
        this.Fuel = physicsData.Fuel;
        this.Gear = physicsData.Gear;
        this.Heading = physicsData.Heading;
        this.IsEngineRunning = physicsData.IsEngineRunning;
        this.IsIgnitionOn = physicsData.IgnitionOn;
        this.IsInvalid = !graphicsData.IsValidLap;
        this.IsPitLimiterOn = physicsData.PitLimiterOn;
        this.LateralG = physicsData.AccG[0];
        this.LocationX = physicsData.TyreContactPoints[0].X;
        this.LocationY = physicsData.TyreContactPoints[0].Z;
        this.LongitudinalG = physicsData.AccG[2];
        this.NormalisedCarPosition = graphicsData.NormalizedPosition;
        this.Pitch = physicsData.Pitch;
        this.RearBrakeCompound = physicsData.RearBrakeCompound;
        this.RearBumpDamperLevel = graphicsData.Electronics.RearBumpDamperLevel;
        this.RearReboundDamperLevel = graphicsData.Electronics.RearReboundDamperLevel;
        this.RideHeightFront = physicsData.RideHeight[0];
        this.RideHeightRear = physicsData.RideHeight[1];
        this.Roll = physicsData.Roll;
        this.Rpm = physicsData.Rpm;
        this.SlipAngleFl = physicsData.SlipAngle[0];
        this.SlipAngleFr = physicsData.SlipAngle[1];
        this.SlipAngleRl = physicsData.SlipAngle[2];
        this.SlipAngleRr = physicsData.SlipAngle[3];
        this.SlipRatioFl = physicsData.SlipRatio[0];
        this.SlipRatioFr = physicsData.SlipRatio[1];
        this.SlipRatioRl = physicsData.SlipRatio[2];
        this.SlipRatioRr = physicsData.SlipRatio[3];
        this.SpeedKmh = physicsData.SpeedKmh;
        this.SteeringAngle = physicsData.SteerAngle;
        this.SuspensionTravelFl = physicsData.SuspensionTravel[0];
        this.SuspensionTravelFr = physicsData.SuspensionTravel[1];
        this.SuspensionTravelRl = physicsData.SuspensionTravel[2];
        this.SuspensionTravelRr = physicsData.SuspensionTravel[3];
        this.TrackId = staticData.Track;
        this.TimeStamp = DateTime.UtcNow;
        this.TractionControl = physicsData.TractionControl;
        this.TurboBoost = physicsData.TurboBoost;
        this.TyreCoreTempFl = physicsData.TyreCoreTemperature[0];
        this.TyreCoreTempFr = physicsData.TyreCoreTemperature[1];
        this.TyreCoreTempRl = physicsData.TyreCoreTemperature[2];
        this.TyreCoreTempRr = physicsData.TyreCoreTemperature[3];
        this.TyrePressureFl = physicsData.WheelsPressure[0];
        this.TyrePressureFr = physicsData.WheelsPressure[1];
        this.TyrePressureRl = physicsData.WheelsPressure[2];
        this.TyrePressureRr = physicsData.WheelsPressure[3];
        this.TyreTempFl = physicsData.TyreTemp[0];
        this.TyreTempFr = physicsData.TyreTemp[1];
        this.TyreTempRl = physicsData.TyreTemp[2];
        this.TyreTempRr = physicsData.TyreTemp[3];
        this.TyreTreadTempFlInner = graphicsData.TyreLf.TyreTemperatureRight;
        this.TyreTreadTempFlMiddle = graphicsData.TyreLf.TyreTemperatureCenter;
        this.TyreTreadTempFlOuter = graphicsData.TyreLf.TyreTemperatureLeft;
        this.TyreTreadTempFrInner = graphicsData.TyreRf.TyreTemperatureLeft;
        this.TyreTreadTempFrMiddle = graphicsData.TyreRf.TyreTemperatureCenter;
        this.TyreTreadTempFrOuter = graphicsData.TyreRf.TyreTemperatureRight;
        this.TyreTreadTempRlInner = graphicsData.TyreLr.TyreTemperatureRight;
        this.TyreTreadTempRlMiddle = graphicsData.TyreLr.TyreTemperatureCenter;
        this.TyreTreadTempRlOuter = graphicsData.TyreLr.TyreTemperatureLeft;
        this.TyreTreadTempRrInner = graphicsData.TyreRr.TyreTemperatureLeft;
        this.TyreTreadTempRrMiddle = graphicsData.TyreRr.TyreTemperatureCenter;
        this.TyreTreadTempRrOuter = graphicsData.TyreRr.TyreTemperatureRight;
        this.VerticalG = physicsData.AccG[1];
        this.WaterTemp = physicsData.WaterTemp;
        this.WheelLoadFl = physicsData.WheelLoad[0];
        this.WheelLoadFr = physicsData.WheelLoad[1];
        this.WheelLoadRl = physicsData.WheelLoad[2];
        this.WheelLoadRr = physicsData.WheelLoad[3];
        this.WheelSlipFl = physicsData.WheelSlip[0];
        this.WheelSlipFr = physicsData.WheelSlip[1];
        this.WheelSlipRl = physicsData.WheelSlip[2];
        this.WheelSlipRr = physicsData.WheelSlip[3];
        this.YawRate = physicsData.LocalAngularVelocity[1];
    }

    public float Abs { get; }
    public float Accelerator { get; }
    public float Brake { get; }
    public float BrakeBias { get; }
    public float BrakeTorqueFl { get; }
    public float BrakeTorqueFr { get; }
    public float BrakeTorqueRl { get; }
    public float BrakeTorqueRr { get; }
    public float BrakeTempFl { get; }
    public float BrakeTempFr { get; }
    public float BrakeTempRl { get; }
    public float BrakeTempRr { get; }
    public float CamberRadFl { get; }
    public float CamberRadFr { get; }
    public float CamberRadRl { get; }
    public float CamberRadRr { get; }
    public string CarModel { get; }
    public float Clutch { get; }
    public int CurrentLapTimeMs { get; }
    public string DriverFirstName { get; }
    public string DriverLastName { get; }
    public string DriverDisplayName { get; }
    public string DriverFullName { get; }
    public int FrontBrakeCompound { get; }
    public int FrontBumpDamperLevel { get; }
    public int FrontReboundDamperLevel { get; }
    public float Fuel { get; }
    public int Gear { get; }
    public float Heading { get; }
    public bool IsEngineRunning { get; }
    public bool IsIgnitionOn { get; }
    public bool IsInvalid { get; }
    public bool IsPitLimiterOn { get; }
    public float LateralG { get; }
    public float LocationX { get; }
    public float LocationY { get; }
    public float LongitudinalG { get; }
    public float NormalisedCarPosition { get; }
    public float Pitch { get; }
    public int RearBrakeCompound { get; }
    public int RearBumpDamperLevel { get; }
    public int RearReboundDamperLevel { get; }
    public float RideHeightFront { get; }
    public float RideHeightRear { get; }
    public float Roll { get; }
    public int Rpm { get; }
    public float SlipAngleFl { get; }
    public float SlipAngleFr { get; }
    public float SlipAngleRl { get; }
    public float SlipAngleRr { get; }
    public float SlipRatioFl { get; }
    public float SlipRatioFr { get; }
    public float SlipRatioRl { get; }
    public float SlipRatioRr { get; }
    public float SpeedKmh { get; }
    public float SteeringAngle { get; }
    public float SuspensionTravelFl { get; }
    public float SuspensionTravelFr { get; }
    public float SuspensionTravelRl { get; }
    public float SuspensionTravelRr { get; }
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
    public float TyreTreadTempFlInner { get; }
    public float TyreTreadTempFlMiddle { get; }
    public float TyreTreadTempFlOuter { get; }
    public float TyreTreadTempFrInner { get; }
    public float TyreTreadTempFrMiddle { get; }
    public float TyreTreadTempFrOuter { get; }
    public float TyreTreadTempRlInner { get; }
    public float TyreTreadTempRlMiddle { get; }
    public float TyreTreadTempRlOuter { get; }
    public float TyreTreadTempRrInner { get; }
    public float TyreTreadTempRrMiddle { get; }
    public float TyreTreadTempRrOuter { get; }
    public float VerticalG { get; }
    public float WaterTemp { get; }
    public float WheelLoadFl { get; }
    public float WheelLoadFr { get; }
    public float WheelLoadRl { get; }
    public float WheelLoadRr { get; }
    public float WheelSlipFl { get; }
    public float WheelSlipFr { get; }
    public float WheelSlipRl { get; }
    public float WheelSlipRr { get; }
    public float YawRate { get; }
}
