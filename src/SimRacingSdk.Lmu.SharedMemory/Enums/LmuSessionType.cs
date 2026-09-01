namespace SimRacingSdk.Lmu.SharedMemory.Enums;

// Matches ScoringInfoV01::mSession's value convention (InternalsPlugin.hpp) - the same ISI/rFactor2 Internals API
// numbering LMU inherits: 0 = test day, 1-4 = practice, 5-8 = qualify, 9 = warmup, 10-13 = race. Sourced from the
// standard rF2-family convention (also used by TinyPedal's pyRfactor2SharedMemory, referenced during the native-
// interface investigation - see CLAUDE.md) rather than confirmed against a live multi-session weekend - verify
// Session's raw value against the game's own displayed session type on the next rig test.
public enum LmuSessionType
{
    Unknown = -1,
    TestDay = 0,
    Practice = 1,
    Qualify = 2,
    Warmup = 3,
    Race = 4
}
