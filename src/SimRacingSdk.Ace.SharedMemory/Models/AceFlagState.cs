using SimRacingSdk.Ace.SharedMemory.Enums;

namespace SimRacingSdk.Ace.SharedMemory.Models;

public record struct AceFlagState(AceFlagType Flag, AceFlagType GlobalFlag) { }
