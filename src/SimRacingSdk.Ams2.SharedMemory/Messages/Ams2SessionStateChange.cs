using SimRacingSdk.Ams2.SharedMemory.Enums;

namespace SimRacingSdk.Ams2.SharedMemory.Messages;

public record Ams2SessionStateChange(Ams2SessionState From, Ams2SessionState To)
{ }