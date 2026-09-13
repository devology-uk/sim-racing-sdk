namespace SimRacingSdk.Pmr.Udp.Enums;

[Flags]
public enum PmrRaceFlags : uint
{
    None = 0,
    Chequered = 1 << 0,
    Yellow = 1 << 1,
    White = 1 << 2,
    Blue = 1 << 3
}
