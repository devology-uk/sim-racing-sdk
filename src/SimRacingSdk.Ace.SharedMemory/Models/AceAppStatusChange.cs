using SimRacingSdk.Ace.SharedMemory.Enums;

namespace SimRacingSdk.Ace.SharedMemory.Models;

public record AceAppStatusChange(AceStatus From, AceStatus To)
{ }
