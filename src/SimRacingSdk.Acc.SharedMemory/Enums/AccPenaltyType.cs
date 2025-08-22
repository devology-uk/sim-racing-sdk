namespace SimRacingSdk.Acc.SharedMemory.Enums;

public enum AccPenaltyType
{
    None,
    DriveThroughForCutting,
    TenSecondStopAndGoForCutting,
    TwentySecondStopAndGoForCutting,
    ThirtySecondStopAndGoForCutting,
    DisqualifiedForExcessiveCutting,
    LapTimeDeletedForCutting,
    DriveThroughSpeedingInPitLane,
    TenSecondStopAndGoForSpeedingInPitLane,
    TwentySecondStopAndGoForSpeedingInPitLane,
    ThirtySecondStopAndGoForSpeedingInPitLane,
    DisqualifiedForSpeedingInPitLane,
    BestLapTimeDeletedForSpeedingInPitLane,
    DisqualifiedForFailingToCompleteMandatoryPitstop,
    ExceedingPostRaceTime,
    DisqualifiedForTrollingInChat,
    DisqualifiedForPitEntryInfringement,
    DisqualifiedForPitExitInfringement,
    DisqualifiedForDrivingWrongWay,
    DriveThroughForFailingToCompleteMinimumDriverStintTime,
    DisqualifiedForFailingToCompleteMinimumDriverStintTime,
    DisqualifiedForExceedingDriverStintTimeLimit
};