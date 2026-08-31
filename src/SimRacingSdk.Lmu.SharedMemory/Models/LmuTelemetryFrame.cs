#nullable disable

using SimRacingSdk.Lmu.SharedMemory.Messages;

namespace SimRacingSdk.Lmu.SharedMemory.Models;

// Merges the player's Rf2VehicleTelemetry (physics, $rFactor2SMMP_Telemetry$) and Rf2VehicleScoring (lap/session
// timing, $rFactor2SMMP_Scoring$) - matched by Id, the shared slot-ID space both plugins report - with the latest
// LmuExtendedBuffer ($LMU_SMMP_Extended$, fuel/energy/battery/penalties/TC) into one frame per update.
public record LmuTelemetryFrame
{
    internal LmuTelemetryFrame(Rf2VehicleTelemetry telemetry, Rf2VehicleScoring scoring, LmuExtendedBuffer extended)
    {
        this.Id = telemetry.Id;
        this.VehicleName = telemetry.VehicleName;
        this.TrackName = telemetry.TrackName;
        this.DriverName = scoring.DriverName;
        this.VehicleClass = scoring.VehicleClass;

        this.DeltaTime = telemetry.DeltaTime;
        this.ElapsedTime = telemetry.ElapsedTime;
        this.LapNumber = telemetry.LapNumber;
        this.LapStartEt = telemetry.LapStartEt;
        this.CurrentSector = telemetry.CurrentSector;

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

        this.Wheels = telemetry.Wheels?.Select(wheel => new LmuWheelData(wheel)).ToList();

        // Scoring (official lap/session timing) - authoritative source of lap boundaries/times so live capture
        // stays keyed off the same source as the physics data (see Logging pattern refactor session's discussion
        // of the ACC UDP-vs-SharedMemory lap-linking problem this is meant to avoid).
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
        this.PitState = scoring.PitState;
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

        // LMU Extended (fuel/energy/battery/penalties/TC) - carried forward between rF2 Telemetry frames, since
        // this buffer updates far less often (see AceSharedMemory-style forward-fill pattern in LmuSharedMemoryConnection).
        this.DirectMemoryAccessEnabled = extended.DirectMemoryAccessEnabled;
        this.BrakeMigration = extended.BrakeMigration;
        this.BrakeMigrationMax = extended.BrakeMigrationMax;
        this.TractionControl = extended.TractionControl;
        this.MotorMap = extended.MotorMap;
        this.FrontAbr = extended.FrontAbr;
        this.RearAbr = extended.RearAbr;
        this.PenaltyType = extended.PenaltyType;
        this.PenaltyCount = extended.PenaltyCount;
        this.PenaltyLeftLaps = extended.PenaltyLeftLaps;
        this.PendingPenaltyType1 = extended.PendingPenaltyType1;
        this.PendingPenaltyType2 = extended.PendingPenaltyType2;
        this.PendingPenaltyType3 = extended.PendingPenaltyType3;
        this.Cuts = extended.Cuts;
        this.CutsPoints = extended.CutsPoints;
        this.CurrentBatteryValue = extended.CurrentBatteryValue;
        this.MaxBatteryValue = extended.MaxBatteryValue;
        this.CurrentEnergyValue = extended.CurrentEnergyValue;
        this.MaxEnergyValue = extended.MaxEnergyValue;
        this.CurrentFuelValue = extended.CurrentFuelValue;
        this.MaxFuelValue = extended.MaxFuelValue;
        this.EnergyLastLap = extended.EnergyLastLap;
        this.FuelLastLap = extended.FuelLastLap;
    }

    public byte AntiStallActivated { get; }
    public double BatteryChargeFraction { get; }
    public double BestLapSector1 { get; }
    public double BestLapSector2 { get; }
    public double BestLapTime { get; }
    public double BestSector1 { get; }
    public double BestSector2 { get; }
    public int BrakeMigration { get; }
    public int BrakeMigrationMax { get; }
    public double ClutchRpm { get; }
    public sbyte Control { get; }
    public byte CountLapFlag { get; }
    public int CurrentSector { get; }
    public double CurrentBatteryValue { get; }
    public double CurrentEnergyValue { get; }
    public double CurrentFuelValue { get; }
    public double CurSector1 { get; }
    public double CurSector2 { get; }
    public float Cuts { get; }
    public int CutsPoints { get; }
    public double DeltaTime { get; }
    public byte[] DentSeverity { get; }
    public bool Detached { get; }
    public bool DirectMemoryAccessEnabled { get; }
    public double Drag { get; }
    public string DriverName { get; }
    public byte ElectricBoostMotorState { get; }
    public double ElectricBoostMotorRpm { get; }
    public double ElectricBoostMotorTemperature { get; }
    public double ElectricBoostMotorTorque { get; }
    public double ElectricBoostWaterTemperature { get; }
    public double ElapsedTime { get; }
    public float EnergyLastLap { get; }
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
    public byte Flag { get; }
    public sbyte FinishStatus { get; }
    public double Front3rdDeflection { get; }
    public int FrontAbr { get; }
    public double FrontDownforce { get; }
    public byte FrontFlapActivated { get; }
    public double FrontRideHeight { get; }
    public byte FrontTireCompoundIndex { get; }
    public string FrontTireCompoundName { get; }
    public double FrontWingHeight { get; }
    public double Fuel { get; }
    public double FuelCapacity { get; }
    public float FuelLastLap { get; }
    public int Gear { get; }
    public bool Headlights { get; }
    public int Id { get; }
    public byte IgnitionStarter { get; }
    public bool InGarageStall { get; }
    public bool InPits { get; }
    public bool IsPlayer { get; }
    public double LapDist { get; }
    public int LapNumber { get; }
    public double LapStartEt { get; }
    public int LapsBehindLeader { get; }
    public int LapsBehindNext { get; }
    public double LastImpactEt { get; }
    public double LastImpactMagnitude { get; }
    public Rf2Vec3 LastImpactPos { get; }
    public double LastLapTime { get; }
    public double LastSector1 { get; }
    public double LastSector2 { get; }
    public Rf2Vec3 LocalAccel { get; }
    public Rf2Vec3 LocalRot { get; }
    public Rf2Vec3 LocalRotAccel { get; }
    public Rf2Vec3 LocalVel { get; }
    public double MaxBatteryValue { get; }
    public double MaxEnergyValue { get; }
    public double MaxFuelValue { get; }
    public byte MaxGears { get; }
    public string MotorMap { get; }
    public int NumPenalties { get; }
    public int NumPitstops { get; }
    public Rf2Vec3[] Ori { get; }
    public bool Overheating { get; }
    public string PitGroup { get; }
    public float PitLapDist { get; }
    public byte PitState { get; }
    public double PathLateral { get; }
    public int PendingPenaltyType1 { get; }
    public int PendingPenaltyType2 { get; }
    public int PendingPenaltyType3 { get; }
    public int PenaltyCount { get; }
    public int PenaltyLeftLaps { get; }
    public int PenaltyType { get; }
    public double PhysicalSteeringWheelRange { get; }
    public float[] PhysicsToGraphicsOffset { get; }
    public byte Place { get; }
    public Rf2Vec3 Pos { get; }
    public int Qualification { get; }
    public double RearAbr { get; }
    public double RearBrakeBias { get; }
    public byte RearFlapActivated { get; }
    public byte RearFlapLegalStatus { get; }
    public double RearDownforce { get; }
    public double RearRideHeight { get; }
    public byte RearTireCompoundIndex { get; }
    public string RearTireCompoundName { get; }
    public double Rear3rdDeflection { get; }
    public byte ScheduledStops { get; }
    public sbyte Sector { get; }
    public byte SpeedLimiter { get; }
    public byte SpeedLimiterAvailable { get; }
    public double SteeringShaftTorque { get; }
    public string TrackName { get; }
    public double TrackEdge { get; }
    public int TractionControl { get; }
    public double TimeBehindLeader { get; }
    public double TimeBehindNext { get; }
    public double TimeIntoLap { get; }
    public short TotalLaps { get; }
    public double TurboBoostPressure { get; }
    public double UnfilteredBrake { get; }
    public double UnfilteredClutch { get; }
    public double UnfilteredSteering { get; }
    public double UnfilteredThrottle { get; }
    public bool UnderYellow { get; }
    public string VehicleClass { get; }
    public string VehicleName { get; }
    public float VisualSteeringWheelRange { get; }
    public IReadOnlyList<LmuWheelData> Wheels { get; }
}
