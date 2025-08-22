using SimRacingSdk.Acc.SharedMemory.Enums;
using SimRacingSdk.Acc.SharedMemory.Messages;

namespace SimRacingSdk.Acc.SharedMemory.Models;

public class GraphicsData(GraphicsPage graphicsPage)
{
    public int Abs { get; } = graphicsPage.ABS;

    public int ActiveCars { get; } = graphicsPage.ActiveCars;

    public string BestTime { get; } = graphicsPage.BestTime;

    public int BestTimeMs { get; } = graphicsPage.BestTimeMs;

    public AccCoordinate3d[] CarCoordinates { get; } = graphicsPage.CarCoordinates;

    public int[] CarIds { get; } = graphicsPage.CarIds;

    public int CompletedLaps { get; } = graphicsPage.CompletedLaps;

    public int CurrentSectorIndex { get; } = graphicsPage.CurrentSectorIndex;

    public string CurrentTime { get; } = graphicsPage.CurrentTime;

    public int CurrentTimeMs { get; } = graphicsPage.CurrentTimeMs;

    public int CurrentTyreSet { get; } = graphicsPage.CurrentTyreSet;

    public string DeltaLapTime { get; } = graphicsPage.DeltaLapTime;

    public int DeltaLapTimeMs { get; } = graphicsPage.DeltaLapTimeMillis;

    public float DistanceTravelled { get; } = graphicsPage.DistanceTraveled;

    public int DriverStintTimeLeft { get; } = graphicsPage.DriverStintTimeLeft;

    public int DriverStintTotalTimeLeft { get; } = graphicsPage.DriverStintTotalTimeLeft;

    public int EngineMap { get; } = graphicsPage.EngineMap;

    public string EstimatedLapTime { get; } = graphicsPage.EstimatedLapTime;

    public int EstimatedLapTimeMs { get; } = graphicsPage.EstimatedLapTimeMs;

    public float ExhaustTemperature { get; } = graphicsPage.ExhaustTemperature;

    public AccFlagType Flag { get; } = graphicsPage.Flag;

    public int FlashingLights { get; } = graphicsPage.FlashingLights;

    public float FuelEstimatedLaps { get; } = graphicsPage.FuelEstimatedLaps;

    public float FuelPerLap { get; } = graphicsPage.AverageFuelPerLap;

    public int GapAheadMs { get; } = graphicsPage.GapAheadMs;

    public int GapBehindMs { get; } = graphicsPage.GapBehindMs;

    public bool GlobalWhite { get; } = graphicsPage.IsWhiteFlagActive;

    public bool GlobalYellow { get; } = graphicsPage.IsYellowFlagActive;

    public bool GlobalYellowSector1 { get; } = graphicsPage.IsYellowFlagActiveInSector1;

    public bool GlobalYellowSector2 { get; } = graphicsPage.IsYellowFlagActiveInSector2;

    public bool GlobalYellowSector3 { get; } = graphicsPage.IsYellowActiveInSector3;

    public bool IdealLineOn { get; } = graphicsPage.IsIdealLineOn;

    public bool IsDeltaPositive { get; } = graphicsPage.IsDeltaPositive;

    public bool IsInPitLane { get; } = graphicsPage.IsInPitLane;

    public bool IsInPits { get; } = graphicsPage.IsInPits;

    public bool IsLeftIndicatorOn { get; } = graphicsPage.IsLeftIndicatorOn;

    public bool IsRightIndicatorOn { get; } = graphicsPage.IsRightIndicatorOn;

    public bool IsSetupMenuVisible { get; } = graphicsPage.IsSetupMenuVisible;

    public bool IsValidLap { get; } = graphicsPage.IsValidLap;

    public int LastSectorTime { get; } = graphicsPage.LastSectorTime;

    public string LastTime { get; } = graphicsPage.LastTime;

    public int LastTimeMs { get; } = graphicsPage.LastTimeMs;

    public int LightsStage { get; } = graphicsPage.LightsStage;

    public int MainDisplayIndex { get; } = graphicsPage.MainDisplayIndex;

    public bool MandatoryPitDone { get; } = graphicsPage.IsMandatoryPitStopComplete;

    public int MandatoryPitStopsLeft { get; } = graphicsPage.MandatoryPitStopsLeft;

    public float MfdFuelToAdd { get; } = graphicsPage.MfdFuelToAdd;

    public float MfdTyrePressureLf { get; } = graphicsPage.MfdTyrePressureLF;

    public float MfdTyrePressureLr { get; } = graphicsPage.MfdTyrePressureLR;

    public float MfdTyrePressureRf { get; } = graphicsPage.MfdTyrePressureRF;

    public float MfdTyrePressureRr { get; } = graphicsPage.MfdTyrePressureRR;

    public int MfdTyreSet { get; } = graphicsPage.MfdTyreSet;

    public float NormalizedCarPosition { get; } = graphicsPage.NormalizedCarPosition;

    public int NumberOfLaps { get; } = graphicsPage.NumberOfLaps;

    public int PacketId { get; } = graphicsPage.PacketId;

    public float PenaltyTime { get; } = graphicsPage.PenaltyTime;

    public AccPenaltyType PenaltyType { get; } = graphicsPage.PenaltyType;

    public int PlayerCarId { get; } = graphicsPage.PlayerCarID;

    public int Position { get; } = graphicsPage.Position;

    public AccRainIntensity RainIntensity { get; } = graphicsPage.RainIntensity;

    public AccRainIntensity RainIntensityIn10Min { get; } = graphicsPage.RainIntensityIn10Minutes;

    public AccRainIntensity RainIntensityIn30Min { get; } = graphicsPage.RainIntensityIn30Minutes;

    public int RainLights { get; } = graphicsPage.RainLights;

    public int RainTyres { get; } = graphicsPage.RainTyres;

    public int SecondaryDisplayIndex { get; } = graphicsPage.SecondaryDisplayIndex;

    public int SessionIndex { get; } = graphicsPage.SessionIndex;

    public float SessionTimeLeft { get; } = graphicsPage.SessionTimeLeft;

    public AccSessionType SessionType { get; } = graphicsPage.SessionType;

    public string Split { get; } = graphicsPage.Split;

    public int SplitTimeMs { get; } = graphicsPage.SplitTimeMillis;

    public AccAppStatus Status { get; } = graphicsPage.Status;

    public int StrategyTyreSet { get; } = graphicsPage.StrategyTyreSet;

    public float SurfaceGrip { get; } = graphicsPage.SurfaceGrip;

    public DateTime TimeStamp { get; } = DateTime.UtcNow;

    public AccTrackGripStatus TrackGripStatus { get; } = graphicsPage.TrackGripStatus;

    public string TrackStatus { get; } = graphicsPage.TrackStatus;

    public int TractionControl { get; } = graphicsPage.TC;

    public int TractionControlCut { get; } = graphicsPage.TCCut;

    public string TyreCompound { get; } = graphicsPage.TyreCompound;

    public float UsedFuelSinceRefuel { get; } = graphicsPage.FuelUsedSinceRefuel;

    public float WindDirection { get; } = graphicsPage.WindDirection;

    public float WindSpeed { get; } = graphicsPage.WindSpeed;

    public int WiperLevel { get; } = graphicsPage.WiperStage;

    public override string ToString()
    {
        return
            $"Graphics Data: Time Stamp: {this.TimeStamp:hh:mm:ss.ffff}, Current Time MS: {this.CurrentTimeMs}, Last Time MS: {this.LastTimeMs}, Current Sector Index: {this.CurrentSectorIndex}, Split Time Ms: {this.SplitTimeMs}, In Pit lane: {this.IsInPitLane}, In Pits: {this.IsInPits}";
    }
}