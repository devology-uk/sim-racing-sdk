namespace SimRacingSdk.Ams2.SharedMemory.Enums;

public enum Ams2CarFlags : uint
{
    HeadLight = (1<<0),
    EngineActive = (1<<1),
    EngineWarning = (1<<2),
    SpeedLimiter = (1<<3),
    Abs = (1<<4),
    HandBrake = (1<<5),
    Tcs = (1<<6),
    Scs = (1<<7)
}