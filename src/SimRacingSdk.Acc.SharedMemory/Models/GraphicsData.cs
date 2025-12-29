#nullable disable

using SimRacingSdk.Acc.SharedMemory.Enums;
using SimRacingSdk.Acc.SharedMemory.Messages;

namespace SimRacingSdk.Acc.SharedMemory.Models;

public record GraphicsData
{
    private readonly GraphicsPage graphicsPage;

    internal GraphicsData()
    {
        this.IsEmpty = true;
    }

    internal GraphicsData(GraphicsPage graphicsPage)
    {
        this.graphicsPage = graphicsPage;
        this.Abs = graphicsPage.ABS;
        this.ActiveCars = graphicsPage.ActiveCars;
        this.BestTime = graphicsPage.BestTime;
        this.BestTimeMs = graphicsPage.BestTimeMs;
        this.CarCoordinates = graphicsPage.CarCoordinates;
        this.CarIds = graphicsPage.CarIds;
        this.CompletedLaps = graphicsPage.CompletedLaps;
        this.CurrentSectorIndex = graphicsPage.CurrentSectorIndex;
        this.CurrentTime = graphicsPage.CurrentTime;
        this.CurrentTimeMs = graphicsPage.CurrentTimeMs;
        this.CurrentTyreSet = graphicsPage.CurrentTyreSet;
        this.DeltaLapTime = graphicsPage.DeltaLapTime;
        this.DeltaLapTimeMs = graphicsPage.DeltaLapTimeMillis;
        this.DistanceTravelled = graphicsPage.DistanceTraveled;
        this.DriverStintTimeLeft = graphicsPage.DriverStintTimeLeft;
        this.DriverStintTotalTimeLeft = graphicsPage.DriverStintTotalTimeLeft;
        this.EngineMap = graphicsPage.EngineMap;
        this.EstimatedLapTime = graphicsPage.EstimatedLapTime;
        this.EstimatedLapTimeMs = graphicsPage.EstimatedLapTimeMs;
        this.ExhaustTemperature = graphicsPage.ExhaustTemperature;
        this.Flag = graphicsPage.Flag;
        this.FlashingLights = graphicsPage.FlashingLights;
        this.FuelEstimatedLaps = graphicsPage.FuelEstimatedLaps;
        this.FuelPerLap = graphicsPage.AverageFuelPerLap;
        this.GapAheadMs = graphicsPage.GapAheadMs;
        this.GapBehindMs = graphicsPage.GapBehindMs;
        this.IsIdealLineOn = graphicsPage.IsIdealLineOn;
        this.IsDeltaPositive = graphicsPage.IsDeltaPositive;
        this.IsInPitLane = graphicsPage.IsInPitLane;
        this.IsInPits = graphicsPage.IsInPits;
        this.IsLeftIndicatorOn = graphicsPage.IsLeftIndicatorOn;
        this.IsMandatoryPitDone = graphicsPage.IsMandatoryPitStopComplete;
        this.IsRightIndicatorOn = graphicsPage.IsRightIndicatorOn;
        this.IsSetupMenuVisible = graphicsPage.IsSetupMenuVisible;
        this.IsValidLap = graphicsPage.IsValidLap;
        this.IsWhiteFlagActive = graphicsPage.IsWhiteFlagActive;
        this.IsYellowFlagActive = graphicsPage.IsYellowFlagActive;
        this.IsYellowFlagActiveInSector1 = graphicsPage.IsYellowFlagActiveInSector1;
        this.IsYellowFlagActiveInSector2 = graphicsPage.IsYellowFlagActiveInSector2;
        this.IsYellowFlagActiveInSector3 = graphicsPage.IsYellowActiveInSector3;
        this.LastSectorTime = graphicsPage.LastSectorTime;
        this.LastTime = graphicsPage.LastTime;
        this.LastTimeMs = graphicsPage.LastTimeMs;
        this.LightsStage = graphicsPage.LightsStage;
        this.MainDisplayIndex = graphicsPage.MainDisplayIndex;
        this.MandatoryPitStopsLeft = graphicsPage.MandatoryPitStopsLeft;
        this.MfdFuelToAdd = graphicsPage.MfdFuelToAdd;
        this.MfdTyrePressureLf = graphicsPage.MfdTyrePressureLF;
        this.MfdTyrePressureLr = graphicsPage.MfdTyrePressureLR;
        this.MfdTyrePressureRf = graphicsPage.MfdTyrePressureRF;
        this.MfdTyrePressureRr = graphicsPage.MfdTyrePressureRR;
        this.MfdTyreSet = graphicsPage.MfdTyreSet;
        this.NormalizedCarPosition = graphicsPage.NormalizedCarPosition;
        this.NumberOfLaps = graphicsPage.NumberOfLaps;
        this.PacketId = graphicsPage.PacketId;
        this.PenaltyTime = graphicsPage.PenaltyTime;
        this.PenaltyType = graphicsPage.PenaltyType;
        this.PlayerCarId = graphicsPage.PlayerCarID;
        this.Position = graphicsPage.Position;
        this.RainIntensity = graphicsPage.RainIntensity;
        this.RainIntensityIn10Min = graphicsPage.RainIntensityIn10Minutes;
        this.RainIntensityIn30Min = graphicsPage.RainIntensityIn30Minutes;
        this.RainLights = graphicsPage.RainLights;
        this.RainTyres = graphicsPage.RainTyres;
        this.SecondaryDisplayIndex = graphicsPage.SecondaryDisplayIndex;
        this.SessionIndex = graphicsPage.SessionIndex;
        this.SessionTimeLeft = graphicsPage.SessionTimeLeft;
        this.SessionType = graphicsPage.SessionType;
        this.Split = graphicsPage.Split;
        this.SplitTimeMs = graphicsPage.SplitTimeMillis;
        this.Status = graphicsPage.Status;
        this.StrategyTyreSet = graphicsPage.StrategyTyreSet;
        this.SurfaceGrip = graphicsPage.SurfaceGrip;
        this.TrackGripStatus = graphicsPage.TrackGripStatus;
        this.TrackStatus = graphicsPage.TrackStatus;
        this.TractionControl = graphicsPage.TC;
        this.TractionControlCut = graphicsPage.TCCut;
        this.TyreCompound = graphicsPage.TyreCompound;
        this.UsedFuelSinceRefuel = graphicsPage.FuelUsedSinceRefuel;
        this.WindDirection = graphicsPage.WindDirection;
        this.WindSpeed = graphicsPage.WindSpeed;
        this.WiperLevel = graphicsPage.WiperStage;
    }

    public int Abs { get; }
    public int ActiveCars { get; }
    public string BestTime { get; }
    public int BestTimeMs { get; }
    public AccCoordinate3d[] CarCoordinates { get; }
    public int[] CarIds { get; }
    public int CompletedLaps { get; }
    public int CurrentSectorIndex { get; }
    public string CurrentTime { get; }
    public int CurrentTimeMs { get; }
    public int CurrentTyreSet { get; }
    public string DeltaLapTime { get; }
    public int DeltaLapTimeMs { get; }
    public float DistanceTravelled { get; }
    public int DriverStintTimeLeft { get; }
    public int DriverStintTotalTimeLeft { get; }
    public int EngineMap { get; }
    public string EstimatedLapTime { get; }
    public int EstimatedLapTimeMs { get; }
    public float ExhaustTemperature { get; }
    public AccFlagType Flag { get; }
    public int FlashingLights { get; }
    public float FuelEstimatedLaps { get; }
    public float FuelPerLap { get; }
    public int GapAheadMs { get; }
    public int GapBehindMs { get; }
    public bool IsDeltaPositive { get; }
    public bool IsEmpty { get; }
    public bool IsIdealLineOn { get; }
    public bool IsInPitLane { get; }
    public bool IsInPits { get; }
    public bool IsLeftIndicatorOn { get; }
    public bool IsMandatoryPitDone { get; }
    public bool IsRightIndicatorOn { get; }
    public bool IsSetupMenuVisible { get; }
    public bool IsValidLap { get; }
    public bool IsWhiteFlagActive { get; }
    public bool IsYellowFlagActive { get; }
    public bool IsYellowFlagActiveInSector1 { get; }
    public bool IsYellowFlagActiveInSector2 { get; }
    public bool IsYellowFlagActiveInSector3 { get; }
    public int LastSectorTime { get; }
    public string LastTime { get; }
    public int LastTimeMs { get; }
    public int LightsStage { get; }
    public int MainDisplayIndex { get; }
    public int MandatoryPitStopsLeft { get; }
    public float MfdFuelToAdd { get; }
    public float MfdTyrePressureLf { get; }
    public float MfdTyrePressureLr { get; }
    public float MfdTyrePressureRf { get; }
    public float MfdTyrePressureRr { get; }
    public int MfdTyreSet { get; }
    public float NormalizedCarPosition { get; }
    public int NumberOfLaps { get; }
    public int PacketId { get; }
    public float PenaltyTime { get; }
    public AccPenaltyType PenaltyType { get; }
    public int PlayerCarId { get; }
    public int Position { get; }
    public AccRainIntensity RainIntensity { get; }
    public AccRainIntensity RainIntensityIn10Min { get; }
    public AccRainIntensity RainIntensityIn30Min { get; }
    public int RainLights { get; }
    public int RainTyres { get; }
    public int SecondaryDisplayIndex { get; }
    public int SessionIndex { get; }
    public float SessionTimeLeft { get; }
    public AccSessionType SessionType { get; }
    public string Split { get; }
    public int SplitTimeMs { get; }
    public AccAppStatus Status { get; }
    public int StrategyTyreSet { get; }
    public float SurfaceGrip { get; }
    public DateTime TimeStamp { get; } = DateTime.UtcNow;
    public AccTrackGripStatus TrackGripStatus { get; }
    public string TrackStatus { get; }
    public int TractionControl { get; }
    public int TractionControlCut { get; }
    public string TyreCompound { get; }
    public float UsedFuelSinceRefuel { get; }
    public float WindDirection { get; }
    public float WindSpeed { get; }
    public int WiperLevel { get; }

    public bool IsSameSession(GraphicsData graphicsData)
    {
        return this.SessionIndex == graphicsData.SessionIndex && this.SessionType == graphicsData.SessionType
                                                              && this.Status == graphicsData.Status;
    }
}