namespace SimRacingSdk.Ace.Core.Enums;

// Same broadcasting protocol as Acc (confirmed 2026-07-27), so the same session-type codes apply.
public enum RaceSessionType
{
    NONE = -1,
    Practice = 0,
    Qualifying = 4,
    Superpole = 9,
    Race = 10,
    Hotlap = 11,
    Hotstint = 12,
    HotlapSuperpole = 13,
    Replay = 14
};
