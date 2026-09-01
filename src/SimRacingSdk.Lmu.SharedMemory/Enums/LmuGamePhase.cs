namespace SimRacingSdk.Lmu.SharedMemory.Enums;

// Matches ScoringInfoV01::mGamePhase's value convention (InternalsPlugin.hpp) - the same ISI/rFactor2 Internals API
// numbering LMU inherits (also documented by TinyPedal's pyRfactor2SharedMemory - see CLAUDE.md). Not yet confirmed
// against a live session that actually reaches SessionStopped/SessionOver - verify on the next rig test that ends
// a session normally (checkered flag / session-timer expiry) rather than via a mid-session garage visit.
public enum LmuGamePhase : byte
{
    Garage = 0,
    WarmUp = 1,
    GridWalk = 2,
    Formation = 3,
    Countdown = 4,
    GreenFlag = 5,
    FullCourseYellow = 6,
    SessionStopped = 7,
    SessionOver = 8
}
