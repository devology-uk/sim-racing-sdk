using SimRacingSdk.Ace.SharedMemory.Enums;

namespace SimRacingSdk.Ace.SharedMemory.Models;

// Evo only exposes a single per-driver Flag plus a track-wide GlobalFlag - it has no per-sector
// yellow-flag booleans like Acc's AccFlagState, so this shape is intentionally simpler.
public record struct AceFlagState(AceFlagType Flag, AceFlagType GlobalFlag) { }
