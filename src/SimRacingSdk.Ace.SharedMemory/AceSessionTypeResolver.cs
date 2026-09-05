namespace SimRacingSdk.Ace.SharedMemory;

// Evo's static-page Session/SessionName fields are written once at the start of a Race Weekend
// and never refresh as it progresses through Practice/Qualify/Race sub-sessions, so a genuine
// Race can report "TimeAttack" for its entire duration (confirmed against a captured race with
// AI opponents, grid start, and overtime - the static page never left EventId=0/SessionId=0/
// Session=TimeAttack). SessionState.PhaseName, on the per-frame graphics page, does update live
// though, and these particular phase names are a reliable race-only signal - confirmed against a
// real AI race weekend capture (2026-09-05) where the actual 10-minute race hit both
// Overtime_Waiting_For_Leader/Overtime_Waiting_For_Others at its finish, while a 5-minute
// Qualifying sub-session in the same weekend never did.
//
// Waiting_Last_Lap was originally in this list too, on the assumption it was race-exclusive -
// the same 2026-09-05 capture proved that wrong: Qualifying hit it too at its own natural,
// time-based end, wrongly resolving that Qualifying session to "Race". Removed rather than kept
// as a weaker signal, since a false positive here mislabels a real session, not just misses one.
public static class AceSessionTypeResolver
{
    private static readonly string[] raceFinishPhaseNames =
    [
        "Overtime_Waiting_For_Leader",
        "Overtime_Waiting_For_Others"
    ];

    public static string Resolve(string reportedSessionType, IReadOnlySet<string> observedPhaseNames)
    {
        return observedPhaseNames.Overlaps(raceFinishPhaseNames) ? "Race" : reportedSessionType;
    }
}
