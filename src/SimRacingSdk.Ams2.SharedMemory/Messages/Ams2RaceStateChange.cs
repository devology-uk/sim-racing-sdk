using SimRacingSdk.Ams2.SharedMemory.Enums;

namespace SimRacingSdk.Ams2.SharedMemory.Messages;

public record Ams2RaceStateChange(Ams2RaceState From, Ams2RaceState To)
{ }