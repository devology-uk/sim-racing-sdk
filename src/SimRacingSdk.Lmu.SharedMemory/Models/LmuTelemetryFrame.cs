#nullable disable

using SimRacingSdk.Lmu.SharedMemory.Enums;
using SimRacingSdk.Lmu.SharedMemory.Messages;

namespace SimRacingSdk.Lmu.SharedMemory.Models;

// Merges the player's LmuVehicleTelemetry (physics, ~50FPS) and LmuVehicleScoring (lap/session timing, ~5FPS) into
// one frame per update - both come from the same "LMU_Data" buffer read, so (unlike the ACC UDP-vs-SharedMemory
// case) there's no cross-source lap/session-key linking problem to worry about. Position/orientation fields are
// taken from Telemetry only (Scoring repeats them at its own, lower refresh rate); Scoring contributes lap/session
// timing plus the handful of fields (driver name, Steam ID, pit/flag state) Telemetry doesn't carry.
public record LmuTelemetryFrame
{
    internal LmuTelemetryFrame(LmuVehicleTelemetry telemetry, LmuVehicleScoring scoring)
    {
        this.Id = telemetry.Id;
        this.VehicleName = telemetry.VehicleName;
        this.TrackName = telemetry.TrackName;
        this.DriverName = scoring.DriverName;
        this.VehicleClass = telemetry.VehicleClass;
        this.VehicleClassName = scoring.VehicleClass;
        this.VehicleChampionship = telemetry.VehicleChampionship;
        this.VehicleModel = telemetry.VehicleModel;
        this.VehFilename = scoring.VehFilename;
        this.SteamId = scoring.SteamId;

        this.DeltaTime = telemetry.DeltaTime;
        this.ElapsedTime = telemetry.ElapsedTime;
        this.LapNumber = telemetry.LapNumber;
        this.LapStartEt = telemetry.LapStartEt;
        this.CurrentSector = telemetry.CurrentSector;
        this.DeltaBest = telemetry.DeltaBest;

        this.Pos = telemetry.Pos;
        this.LocalVel = telemetry.LocalVel;
        this.LocalAccel = telemetry.LocalAccel;
        this.Ori = telemetry.Ori;
        this.LocalRot = telemetry.LocalRot;
        this.LocalRotAccel = telemetry.LocalRotAccel;

        this.Gear = telemetry.Gear;
        this.EngineRpm = telemetry.EngineRpm;
        this.EngineMaxRpm = telemetry.EngineMaxRpm;
        this.EngineWaterTemp = telemetry.EngineWaterTemp;
        this.EngineOilTemp = telemetry.EngineOilTemp;
        this.EngineTorque = telemetry.EngineTorque;
        this.ClutchRpm = telemetry.ClutchRpm;

        this.UnfilteredThrottle = telemetry.UnfilteredThrottle;
        this.UnfilteredBrake = telemetry.UnfilteredBrake;
        this.UnfilteredSteering = telemetry.UnfilteredSteering;
        this.UnfilteredClutch = telemetry.UnfilteredClutch;
        this.FilteredThrottle = telemetry.FilteredThrottle;
        this.FilteredBrake = telemetry.FilteredBrake;
        this.FilteredSteering = telemetry.FilteredSteering;
        this.FilteredClutch = telemetry.FilteredClutch;

        this.SteeringShaftTorque = telemetry.SteeringShaftTorque;
        this.Front3rdDeflection = telemetry.Front3rdDeflection;
        this.Rear3rdDeflection = telemetry.Rear3rdDeflection;

        this.FrontWingHeight = telemetry.FrontWingHeight;
        this.FrontRideHeight = telemetry.FrontRideHeight;
        this.RearRideHeight = telemetry.RearRideHeight;
        this.Drag = telemetry.Drag;
        this.FrontDownforce = telemetry.FrontDownforce;
        this.RearDownforce = telemetry.RearDownforce;

        this.Fuel = telemetry.Fuel;
        this.FuelCapacity = telemetry.FuelCapacity;
        this.FuelFraction = scoring.FuelFraction;
        this.ScheduledStops = telemetry.ScheduledStops;
        this.Overheating = telemetry.Overheating;
        this.Detached = telemetry.Detached;
        this.Headlights = telemetry.Headlights;
        this.DentSeverity = telemetry.DentSeverity;
        this.LastImpactEt = telemetry.LastImpactEt;
        this.LastImpactMagnitude = telemetry.LastImpactMagnitude;
        this.LastImpactPos = telemetry.LastImpactPos;

        this.SpeedLimiter = telemetry.SpeedLimiter;
        this.SpeedLimiterAvailable = telemetry.SpeedLimiterAvailable;
        this.SpeedLimiterActive = telemetry.SpeedLimiterActive;
        this.MaxGears = telemetry.MaxGears;
        this.FrontTireCompoundIndex = telemetry.FrontTireCompoundIndex;
        this.RearTireCompoundIndex = telemetry.RearTireCompoundIndex;
        this.FrontTireCompoundName = telemetry.FrontTireCompoundName;
        this.RearTireCompoundName = telemetry.RearTireCompoundName;
        this.FrontFlapActivated = telemetry.FrontFlapActivated;
        this.RearFlapActivated = telemetry.RearFlapActivated;
        this.RearFlapLegalStatus = telemetry.RearFlapLegalStatus;
        this.IgnitionStarter = telemetry.IgnitionStarter;
        this.AntiStallActivated = telemetry.AntiStallActivated;
        this.VisualSteeringWheelRange = telemetry.VisualSteeringWheelRange;
        this.PhysicalSteeringWheelRange = telemetry.PhysicalSteeringWheelRange;

        this.RearBrakeBias = telemetry.RearBrakeBias;
        this.TurboBoostPressure = telemetry.TurboBoostPressure;
        this.PhysicsToGraphicsOffset = telemetry.PhysicsToGraphicsOffset;

        this.BatteryChargeFraction = telemetry.BatteryChargeFraction;
        this.ElectricBoostMotorTorque = telemetry.ElectricBoostMotorTorque;
        this.ElectricBoostMotorRpm = telemetry.ElectricBoostMotorRpm;
        this.ElectricBoostMotorTemperature = telemetry.ElectricBoostMotorTemperature;
        this.ElectricBoostWaterTemperature = telemetry.ElectricBoostWaterTemperature;
        this.ElectricBoostMotorState = telemetry.ElectricBoostMotorState;
        this.Regen = telemetry.Regen;
        this.SoC = telemetry.SoC;
        this.VirtualEnergy = telemetry.VirtualEnergy;

        this.LapInvalidated = telemetry.LapInvalidated;
        this.AbsActive = telemetry.AbsActive;
        this.TcActive = telemetry.TcActive;
        this.WiperState = telemetry.WiperState;
        this.Tc = telemetry.Tc;
        this.TcMax = telemetry.TcMax;
        this.TcSlip = telemetry.TcSlip;
        this.TcSlipMax = telemetry.TcSlipMax;
        this.TcCut = telemetry.TcCut;
        this.TcCutMax = telemetry.TcCutMax;
        this.Abs = telemetry.Abs;
        this.AbsMax = telemetry.AbsMax;
        this.MotorMap = telemetry.MotorMap;
        this.MotorMapMax = telemetry.MotorMapMax;
        this.Migration = telemetry.Migration;
        this.MigrationMax = telemetry.MigrationMax;
        this.FrontAntiSway = telemetry.FrontAntiSway;
        this.FrontAntiSwayMax = telemetry.FrontAntiSwayMax;
        this.RearAntiSway = telemetry.RearAntiSway;
        this.RearAntiSwayMax = telemetry.RearAntiSwayMax;
        this.LiftAndCoastProgress = telemetry.LiftAndCoastProgress;
        this.TrackLimitsSteps = telemetry.TrackLimitsSteps;
        this.DrsState = scoring.DrsState;

        this.TimeGapCarAhead = telemetry.TimeGapCarAhead;
        this.TimeGapCarBehind = telemetry.TimeGapCarBehind;
        this.TimeGapPlaceAhead = telemetry.TimeGapPlaceAhead;
        this.TimeGapPlaceBehind = telemetry.TimeGapPlaceBehind;

        this.Wheels = telemetry.Wheel?.Select(wheel => new LmuWheelData(wheel)).ToList();

        // Scoring - official lap/session timing, from the same "LMU_Data" read as Telemetry.
        this.TotalLaps = scoring.TotalLaps;
        this.Sector = scoring.Sector;
        this.FinishStatus = scoring.FinishStatus;
        this.LapDist = scoring.LapDist;
        this.PathLateral = scoring.PathLateral;
        this.TrackEdge = scoring.TrackEdge;
        this.BestSector1 = scoring.BestSector1;
        this.BestSector2 = scoring.BestSector2;
        this.BestLapTime = scoring.BestLapTime;
        this.LastSector1 = scoring.LastSector1;
        this.LastSector2 = scoring.LastSector2;
        this.LastLapTime = scoring.LastLapTime;
        this.CurSector1 = scoring.CurSector1;
        this.CurSector2 = scoring.CurSector2;
        this.NumPitstops = scoring.NumPitstops;
        this.NumPenalties = scoring.NumPenalties;
        this.IsPlayer = scoring.IsPlayer;
        this.Control = scoring.Control;
        this.InPits = scoring.InPits;
        this.Place = scoring.Place;
        this.TimeBehindNext = scoring.TimeBehindNext;
        this.LapsBehindNext = scoring.LapsBehindNext;
        this.TimeBehindLeader = scoring.TimeBehindLeader;
        this.LapsBehindLeader = scoring.LapsBehindLeader;
        this.HeadlightsStatus = scoring.Headlights;
        this.PitState = scoring.PitState;
        this.ServerScored = scoring.ServerScored;
        this.IndividualPhase = scoring.IndividualPhase;
        this.Qualification = scoring.Qualification;
        this.TimeIntoLap = scoring.TimeIntoLap;
        this.EstimatedLapTime = scoring.EstimatedLapTime;
        this.PitGroup = scoring.PitGroup;
        this.Flag = scoring.Flag;
        this.UnderYellow = scoring.UnderYellow;
        this.CountLapFlag = scoring.CountLapFlag;
        this.InGarageStall = scoring.InGarageStall;
        this.PitLapDist = scoring.PitLapDist;
        this.BestLapSector1 = scoring.BestLapSector1;
        this.BestLapSector2 = scoring.BestLapSector2;
        this.AttackMode = scoring.AttackMode;
    }

    public byte Abs { get; }
    public bool AbsActive { get; }
    public byte AbsMax { get; }
    public byte AntiStallActivated { get; }
    public short AttackMode { get; }
    public double BatteryChargeFraction { get; }
    public double BestLapSector1 { get; }
    public double BestLapSector2 { get; }
    public double BestLapTime { get; }
    public double BestSector1 { get; }
    public double BestSector2 { get; }
    public sbyte Control { get; }
    public byte CountLapFlag { get; }
    public int CurrentSector { get; }
    public double CurSector1 { get; }
    public double CurSector2 { get; }
    public double ClutchRpm { get; }
    public double DeltaBest { get; }
    public double DeltaTime { get; }
    public byte[] DentSeverity { get; }
    public bool Detached { get; }
    public double Drag { get; }
    public string DriverName { get; }
    public bool DrsState { get; }
    public double ElapsedTime { get; }
    public byte ElectricBoostMotorState { get; }
    public double ElectricBoostMotorRpm { get; }
    public double ElectricBoostMotorTemperature { get; }
    public double ElectricBoostMotorTorque { get; }
    public double ElectricBoostWaterTemperature { get; }
    public double EngineMaxRpm { get; }
    public double EngineOilTemp { get; }
    public double EngineRpm { get; }
    public double EngineTorque { get; }
    public double EngineWaterTemp { get; }
    public double EstimatedLapTime { get; }
    public double FilteredBrake { get; }
    public double FilteredClutch { get; }
    public double FilteredSteering { get; }
    public double FilteredThrottle { get; }
    public sbyte FinishStatus { get; }
    public byte Flag { get; }
    public double Front3rdDeflection { get; }
    public byte FrontAntiSway { get; }
    public byte FrontAntiSwayMax { get; }
    public double FrontDownforce { get; }
    public byte FrontFlapActivated { get; }
    public double FrontRideHeight { get; }
    public byte FrontTireCompoundIndex { get; }
    public string FrontTireCompoundName { get; }
    public double FrontWingHeight { get; }
    public double Fuel { get; }
    public double FuelCapacity { get; }
    public byte FuelFraction { get; }
    public int Gear { get; }
    public bool Headlights { get; }
    public byte HeadlightsStatus { get; }
    public int Id { get; }
    public byte IgnitionStarter { get; }
    public byte IndividualPhase { get; }
    public bool InGarageStall { get; }
    public bool InPits { get; }
    public bool IsPlayer { get; }
    public double LapDist { get; }
    public bool LapInvalidated { get; }
    public int LapNumber { get; }
    public double LapStartEt { get; }
    public int LapsBehindLeader { get; }
    public int LapsBehindNext { get; }
    public double LastImpactEt { get; }
    public double LastImpactMagnitude { get; }
    public LmuVect3 LastImpactPos { get; }
    public double LastLapTime { get; }
    public double LastSector1 { get; }
    public double LastSector2 { get; }
    public byte LiftAndCoastProgress { get; }
    public LmuVect3 LocalAccel { get; }
    public LmuVect3 LocalRot { get; }
    public LmuVect3 LocalRotAccel { get; }
    public LmuVect3 LocalVel { get; }
    public byte MaxGears { get; }
    public byte Migration { get; }
    public byte MigrationMax { get; }
    public byte MotorMap { get; }
    public byte MotorMapMax { get; }
    public int NumPenalties { get; }
    public int NumPitstops { get; }
    public LmuVect3[] Ori { get; }
    public bool Overheating { get; }
    public double PathLateral { get; }
    public double PhysicalSteeringWheelRange { get; }
    public float[] PhysicsToGraphicsOffset { get; }
    public string PitGroup { get; }
    public float PitLapDist { get; }
    public byte PitState { get; }
    public byte Place { get; }
    public LmuVect3 Pos { get; }
    public int Qualification { get; }
    public byte RearAntiSway { get; }
    public byte RearAntiSwayMax { get; }
    public double RearBrakeBias { get; }
    public byte RearFlapActivated { get; }
    public byte RearFlapLegalStatus { get; }
    public double RearDownforce { get; }
    public double RearRideHeight { get; }
    public byte RearTireCompoundIndex { get; }
    public string RearTireCompoundName { get; }
    public double Rear3rdDeflection { get; }
    public float Regen { get; }
    public byte ScheduledStops { get; }
    public sbyte Sector { get; }
    public byte ServerScored { get; }
    public float SoC { get; }
    public byte SpeedLimiter { get; }
    public bool SpeedLimiterActive { get; }
    public byte SpeedLimiterAvailable { get; }
    public double SteeringShaftTorque { get; }
    public ulong SteamId { get; }
    public byte Tc { get; }
    public bool TcActive { get; }
    public byte TcCut { get; }
    public byte TcCutMax { get; }
    public byte TcMax { get; }
    public byte TcSlip { get; }
    public byte TcSlipMax { get; }
    public double TimeBehindLeader { get; }
    public double TimeBehindNext { get; }
    public float TimeGapCarAhead { get; }
    public float TimeGapCarBehind { get; }
    public float TimeGapPlaceAhead { get; }
    public float TimeGapPlaceBehind { get; }
    public double TimeIntoLap { get; }
    public short TotalLaps { get; }
    public double TrackEdge { get; }
    public byte TrackLimitsSteps { get; }
    public string TrackName { get; }
    public double TurboBoostPressure { get; }
    public bool UnderYellow { get; }
    public double UnfilteredBrake { get; }
    public double UnfilteredClutch { get; }
    public double UnfilteredSteering { get; }
    public double UnfilteredThrottle { get; }
    public string VehFilename { get; }
    public LmuVehicleChampionship VehicleChampionship { get; }
    public LmuVehicleClass VehicleClass { get; }
    public string VehicleClassName { get; }
    public string VehicleModel { get; }
    public string VehicleName { get; }
    public float VirtualEnergy { get; }
    public float VisualSteeringWheelRange { get; }
    public byte WiperState { get; }
    public IReadOnlyList<LmuWheelData> Wheels { get; }
}
