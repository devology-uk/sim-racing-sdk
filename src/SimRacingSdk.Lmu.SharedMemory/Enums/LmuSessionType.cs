namespace SimRacingSdk.Lmu.SharedMemory.Enums;

// Matches ScoringInfoV01::mSession's value convention (InternalsPlugin.hpp) - the same ISI/rFactor2 Internals API
// numbering LMU inherits: 0 = test day, 1-4 = practice, 5-8 = qualify, 9 = warmup, 10-13 = race.
public enum LmuSessionType
{
    Unknown = -1,
    TestDay = 0,
    Practice = 1,
    Qualify = 2,
    Warmup = 3,
    Race = 4
}
