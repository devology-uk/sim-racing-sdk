namespace SimRacingSdk.Acc.SharedMemory.Models;

public record struct AccFlagState(
    bool IsWhiteFlagActive,
    bool IsYellowFlagActive,
    bool IsYellFlagActiveInSector1,
    bool IsYellFlagActiveInSector2,
    bool IsYellFlagActiveInSector3) { }