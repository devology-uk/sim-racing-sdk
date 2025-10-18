namespace SimRacingSdk.Ams2.SharedMemory.Enums;

public enum Ams2DrsState : uint
{
    Installed = (1 << 0),
    ZoneRules = (1 << 1),
    AvailableNext = (1 << 2),
    AvailableNow = (1 << 3),
    Active = (1 << 4)
}