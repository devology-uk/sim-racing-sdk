using SimRacingSdk.Ams2.SharedMemory.Enums;

namespace SimRacingSdk.Ams2.SharedMemory.Messages;

public record Ams2GameStatusChange(Ams2GameState From, Ams2GameState To)
{
}