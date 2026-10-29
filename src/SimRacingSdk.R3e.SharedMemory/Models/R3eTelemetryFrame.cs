using SimRacingSdk.R3e.SharedMemory.Enums;
using SimRacingSdk.R3e.SharedMemory.Messages;
using SimRacingSdk.R3e.SharedMemory.Services;

namespace SimRacingSdk.R3e.SharedMemory.Models;

// The player's car only. Axis conventions are the game's and still to confirm on the rig: CarOrientation is
// pitch/yaw/roll in radians, and the vehicle-state LocalAcceleration is +X left, +Y up, +Z back.
public record R3eTelemetryFrame
{
    internal R3eTelemetryFrame(R3eShared data, Guid sessionId)
    {
        var player = data.Player;
        var vehicle = data.VehicleInfo;

        this.SessionId = sessionId;
        this.SlotId = vehicle.SlotId;
        this.UserId = R3eValues.Available(vehicle.UserId);
        this.ModelId = vehicle.ModelId;
        this.ClassId = vehicle.ClassId;
        this.TrackId = data.TrackId;
        this.LayoutId = data.LayoutId;
        this.ControlType = (R3eControlType)data.ControlType;
        this.SessionPhase = (R3eSessionPhase)data.SessionPhase;
        this.GameSimulationTimeSeconds = player.GameSimulationTime;

        this.CompletedLaps = data.CompletedLaps;
        this.CurrentLapValid = R3eValues.AvailableFlag(data.CurrentLapValid);
        this.CurrentLapTimeSeconds = R3eValues.Available(data.LapTimeCurrentSelf);
        this.LapDistanceMeters = data.LapDistance;
        this.LapDistanceFraction = R3eValues.Available(data.LapDistanceFraction);
        this.TrackSector = data.TrackSector;
        this.Position = data.Position;
        this.PositionClass = data.PositionClass;
        this.IsInPitlane = R3eValues.AvailableFlag(data.InPitlane);
        this.IsPitLimiterActive = R3eValues.AvailableFlag(data.PitLimiter);

        this.SpeedMetersPerSecond = data.CarSpeed;
        this.EngineRpm = R3eValues.ToRpm(data.EngineRps);
        this.MaxEngineRpm = R3eValues.ToRpm(data.MaxEngineRps);
        this.UpshiftRpm = R3eValues.ToRpm(data.UpshiftRps);
        this.Gear = data.Gear == -2 ? null : data.Gear;
        this.NumberOfGears = R3eValues.Available(data.NumGears);
        this.Throttle = R3eValues.Available(data.Throttle);
        this.ThrottleRaw = R3eValues.Available(data.ThrottleRaw);
        this.Brake = R3eValues.Available(data.Brake);
        this.BrakeRaw = R3eValues.Available(data.BrakeRaw);
        this.Clutch = R3eValues.Available(data.Clutch);
        this.ClutchRaw = R3eValues.Available(data.ClutchRaw);
        this.SteerInputRaw = data.SteerInputRaw;
        this.SteerLockDegrees = data.SteerLockDegrees;
        this.EngineTorque = player.EngineTorque;

        this.FuelLeftLiters = R3eValues.Available(data.FuelLeft);
        this.FuelCapacityLiters = R3eValues.Available(data.FuelCapacity);
        this.FuelPerLapLiters = R3eValues.Available(data.FuelPerLap);
        this.VirtualEnergyLeftMegajoules = R3eValues.Available(data.VirtualEnergyLeft);
        this.VirtualEnergyCapacityMegajoules = R3eValues.Available(data.VirtualEnergyCapacity);
        this.EngineTempCelsius = R3eValues.Available(data.EngineTemp);
        this.EngineOilTempCelsius = R3eValues.Available(data.EngineOilTemp);
        this.EngineOilPressureKpa = R3eValues.Available(data.EngineOilPressure);
        this.FuelPressureKpa = R3eValues.Available(data.FuelPressure);
        this.TurboPressureBar = R3eValues.Available(data.TurboPressure);

        this.RearBrakeBias = R3eValues.Available(data.BrakeBias);
        this.AbsSetting = R3eValues.Available(data.AbsSetting);
        this.TractionControlSetting = R3eValues.Available(data.TractionControlSetting);
        this.TractionControlPercent = R3eValues.Available(data.TractionControlPercent);
        this.EngineMapSetting = R3eValues.Available(data.EngineMapSetting);
        this.EngineBrakeSetting = R3eValues.Available(data.EngineBrakeSetting);
        this.AbsState = R3eValues.Available(data.AidSettings.Abs);
        this.TractionControlState = R3eValues.Available(data.AidSettings.Tc);
        this.TyreTypeFront = data.TireTypeFront;
        this.TyreTypeRear = data.TireTypeRear;
        this.TyreSubtypeFront = data.TireSubtypeFront;
        this.TyreSubtypeRear = data.TireSubtypeRear;

        this.WorldPositionMeters = player.Position;
        this.LocalVelocityMetersPerSecond = player.LocalVelocity;
        this.LocalAccelerationMetersPerSecondSquared = player.LocalAcceleration;
        this.LocalGForce = player.LocalGforce;
        this.LocalAngularVelocityRadiansPerSecond = player.LocalAngularVelocity;
        this.CarOrientationRadians = data.CarOrientation;
        this.CurrentDownforceNewtons = player.CurrentDownforce;
        this.FrontWingHeightMeters = player.FrontWingHeight;
        this.FrontRollAngle = player.FrontRollAngle;
        this.RearRollAngle = player.RearRollAngle;
        this.ThirdSpringDeflectionFrontMeters = player.ThirdSpringSuspensionDeflectionFront;
        this.ThirdSpringDeflectionRearMeters = player.ThirdSpringSuspensionDeflectionRear;

        this.FrontLeft = new R3eWheelData(data, R3eCorner.FrontLeft);
        this.FrontRight = new R3eWheelData(data, R3eCorner.FrontRight);
        this.RearLeft = new R3eWheelData(data, R3eCorner.RearLeft);
        this.RearRight = new R3eWheelData(data, R3eCorner.RearRight);

        this.EngineDamage = R3eValues.Available(data.CarDamage.Engine);
        this.TransmissionDamage = R3eValues.Available(data.CarDamage.Transmission);
        this.AerodynamicsDamage = R3eValues.Available(data.CarDamage.Aerodynamics);
        this.SuspensionDamage = R3eValues.Available(data.CarDamage.Suspension);
    }

    public int? AbsSetting { get; }
    public int? AbsState { get; }
    public float? AerodynamicsDamage { get; }
    public float? Brake { get; }
    public float? BrakeRaw { get; }
    public R3eOrientation CarOrientationRadians { get; }
    public int ClassId { get; }
    public float? Clutch { get; }
    public float? ClutchRaw { get; }
    public int CompletedLaps { get; }
    public R3eControlType ControlType { get; }
    public double CurrentDownforceNewtons { get; }
    public float? CurrentLapTimeSeconds { get; }
    public bool? CurrentLapValid { get; }
    public int? EngineBrakeSetting { get; }
    public float? EngineDamage { get; }
    public int? EngineMapSetting { get; }
    public float? EngineOilPressureKpa { get; }
    public float? EngineOilTempCelsius { get; }
    public float? EngineRpm { get; }
    public float? EngineTempCelsius { get; }
    public double EngineTorque { get; }
    public R3eWheelData FrontLeft { get; }
    public R3eWheelData FrontRight { get; }
    public double FrontRollAngle { get; }
    public double FrontWingHeightMeters { get; }
    public float? FuelCapacityLiters { get; }
    public float? FuelLeftLiters { get; }
    public float? FuelPerLapLiters { get; }
    public float? FuelPressureKpa { get; }
    public double GameSimulationTimeSeconds { get; }
    public int? Gear { get; }
    public bool? IsInPitlane { get; }
    public bool? IsPitLimiterActive { get; }
    public float? LapDistanceFraction { get; }
    public float LapDistanceMeters { get; }
    public int LayoutId { get; }
    public R3eVector3Double LocalAccelerationMetersPerSecondSquared { get; }
    public R3eVector3Double LocalAngularVelocityRadiansPerSecond { get; }
    public R3eVector3Double LocalGForce { get; }
    public R3eVector3Double LocalVelocityMetersPerSecond { get; }
    public float? MaxEngineRpm { get; }
    public int ModelId { get; }
    public int? NumberOfGears { get; }
    public int Position { get; }
    public int PositionClass { get; }
    public float? RearBrakeBias { get; }
    public R3eWheelData RearLeft { get; }
    public R3eWheelData RearRight { get; }
    public double RearRollAngle { get; }
    public Guid SessionId { get; }
    public R3eSessionPhase SessionPhase { get; }
    public int SlotId { get; }
    public float SpeedMetersPerSecond { get; }
    public float SteerInputRaw { get; }
    public int SteerLockDegrees { get; }
    public float? SuspensionDamage { get; }
    public double ThirdSpringDeflectionFrontMeters { get; }
    public double ThirdSpringDeflectionRearMeters { get; }
    public float? Throttle { get; }
    public float? ThrottleRaw { get; }
    public int TrackId { get; }
    public int TrackSector { get; }
    public float? TractionControlPercent { get; }
    public int? TractionControlSetting { get; }
    public int? TractionControlState { get; }
    public float? TransmissionDamage { get; }
    public float? TurboPressureBar { get; }
    public int TyreSubtypeFront { get; }
    public int TyreSubtypeRear { get; }
    public int TyreTypeFront { get; }
    public int TyreTypeRear { get; }
    public float? UpshiftRpm { get; }
    public int? UserId { get; }
    public float? VirtualEnergyCapacityMegajoules { get; }
    public float? VirtualEnergyLeftMegajoules { get; }
    public R3eVector3Double WorldPositionMeters { get; }
}
