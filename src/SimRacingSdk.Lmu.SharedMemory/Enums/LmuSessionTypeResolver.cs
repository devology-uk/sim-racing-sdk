namespace SimRacingSdk.Lmu.SharedMemory.Enums;

public static class LmuSessionTypeResolver
{
    public static LmuSessionType Resolve(int session)
    {
        return session switch
        {
            0 => LmuSessionType.TestDay,
            >= 1 and <= 4 => LmuSessionType.Practice,
            >= 5 and <= 8 => LmuSessionType.Qualify,
            9 => LmuSessionType.Warmup,
            >= 10 and <= 13 => LmuSessionType.Race,
            _ => LmuSessionType.Unknown
        };
    }
}
