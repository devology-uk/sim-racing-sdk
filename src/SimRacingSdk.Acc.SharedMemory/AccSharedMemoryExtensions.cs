using SimRacingSdk.Acc.SharedMemory.Enums;

namespace SimRacingSdk.Acc.SharedMemory;

public static class AccSharedMemoryExtensions
{
    public static string ToFriendlyName(this AccFlagType flagType)
    {
        return flagType switch
        {
            AccFlagType.NoFlag => "Green", AccFlagType.Blue => "Blue",
            AccFlagType.Yellow => "Yellow", AccFlagType.Black => "Black",
            AccFlagType.White => "White", AccFlagType.Chequered => "Chequered",
            AccFlagType.Penalty => "Penalty", AccFlagType.Green => "Green",
            AccFlagType.UnsafeVehicle => "Orange", _ => flagType.ToString()
        };
    }

    public static string ToFriendlyName(this AccSessionType sessionType)
    {
        return sessionType switch
        {
            AccSessionType.HotlapSuperpole => "Hotlap Superpole",
            AccSessionType.TimeAttack => "Time Attack", _ => sessionType.ToString()
        };
    }
}