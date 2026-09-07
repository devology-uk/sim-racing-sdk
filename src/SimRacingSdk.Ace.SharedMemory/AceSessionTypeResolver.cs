namespace SimRacingSdk.Ace.SharedMemory;

public static class AceSessionTypeResolver
{
    private static readonly string[] raceFinishPhaseNames =
    [
        "Overtime_Waiting_For_Leader",
        "Overtime_Waiting_For_Others"
    ];

    public static string Resolve(string reportedSessionType, IReadOnlySet<string> observedPhaseNames)
    {
        return IsInRaceFinishSequence(observedPhaseNames) ? "Race" : reportedSessionType;
    }

    public static bool IsInRaceFinishSequence(IReadOnlySet<string> observedPhaseNames)
    {
        return observedPhaseNames.Overlaps(raceFinishPhaseNames);
    }
}
