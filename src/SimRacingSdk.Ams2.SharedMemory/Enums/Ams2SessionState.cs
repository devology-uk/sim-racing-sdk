namespace SimRacingSdk.Ams2.SharedMemory.Enums;

public enum Ams2SessionState : uint
{
    Invalid = 0,
    Practice,
    Test,
    Qualify,
    FormationLap,
    Race,
    TimeAttack
}