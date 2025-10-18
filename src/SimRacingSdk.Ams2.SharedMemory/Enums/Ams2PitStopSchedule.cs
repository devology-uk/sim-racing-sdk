namespace SimRacingSdk.Ams2.SharedMemory.Enums;

public enum Ams2PitStopSchedule : uint
{
    None = 0,
    PlayerRequested,
    EngineerRequested,
    DamageRequested,
    Mandatory,
    DriveThrough,
    StopAndGo,
    PitStopOccupied
}