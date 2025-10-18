namespace SimRacingSdk.Ams2.SharedMemory.Enums;

public enum Ams2RaceState : uint
{
    Invalid = 0,
    NotStarted,
    Racing,
    Finished,
    Disqualified,
    Retired,
    DidNotFinish
}