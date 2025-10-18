namespace SimRacingSdk.Ams2.SharedMemory.Enums;

public enum Ams2TyreFlag : uint
{
    Attached = (1<<0),
    Inflated = (1<<1),
    OnGround = (1<<2),
}