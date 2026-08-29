namespace SimRacingSdk.Ace.SharedMemory;

// Evo's static-page Session/SessionName fields are written once at the start of a Race Weekend
// and never refresh as it progresses through Practice/Qualify/Race sub-sessions, so a genuine
// Race can report "TimeAttack" for its entire duration (confirmed against a captured race with
// AI opponents, grid start, and overtime - the static page never left EventId=0/SessionId=0/
// Session=TimeAttack). SessionState.PhaseName, on the per-frame graphics page, does update live
// though, and these particular phase names only ever occur once a session has reached its
// finish/last-lap logic - a concept Practice/Qualify/TimeAttack sessions don't have - so seeing
// any of them during a session is a reliable signal it was actually a race.
public static class AceSessionTypeResolver
{
    private static readonly string[] raceFinishPhaseNames =
    [
        "Waiting_Last_Lap",
        "Overtime_Waiting_For_Leader",
        "Overtime_Waiting_For_Others"
    ];

    public static string Resolve(string reportedSessionType, IReadOnlySet<string> observedPhaseNames)
    {
        return observedPhaseNames.Overlaps(raceFinishPhaseNames) ? "Race" : reportedSessionType;
    }
}
