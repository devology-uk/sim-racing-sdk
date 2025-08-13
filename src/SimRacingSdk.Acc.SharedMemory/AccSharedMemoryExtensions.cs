using SimRacingSdk.Acc.SharedMemory.Enums;

namespace SimRacingSdk.Acc.SharedMemory;

public static class AccSharedMemoryExtensions
{
    public static string ToFriendlyName(this AccRtFlagType flagType)
    {
        return flagType switch
        {
            AccRtFlagType.NoFlag => "Green", AccRtFlagType.BlueFlag => "Blue",
            AccRtFlagType.YellowFlag => "Yellow", AccRtFlagType.BlackFlag => "Black",
            AccRtFlagType.WhiteFlag => "White", AccRtFlagType.ChequeredFlag => "Chequered",
            AccRtFlagType.PenaltyFlag => "Penalty", AccRtFlagType.GreenFlag => "Green",
            AccRtFlagType.BlackFlagWithOrangeCircle => "Orange", _ => flagType.ToString()
        };
    }

    public static string ToFriendlyName(this AccRtSessionType sessionType)
    {
        return sessionType switch
        {
            AccRtSessionType.HotlapSuperpole => "Hotlap Superpole",
            AccRtSessionType.TimeAttack => "Time Attack", _ => sessionType.ToString()
        };
    }
}