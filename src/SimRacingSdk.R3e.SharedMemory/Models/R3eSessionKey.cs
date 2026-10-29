using SimRacingSdk.R3e.SharedMemory.Messages;

namespace SimRacingSdk.R3e.SharedMemory.Models;

// SessionIteration is part of the key because R3E can repeat a session type within an event (Qualify 1, Qualify 2).
internal readonly record struct R3eSessionKey(int TrackId, int LayoutId, int EventIndex, int SessionType,
    int SessionIteration)
{
    public static R3eSessionKey From(R3eShared data)
    {
        return new R3eSessionKey(data.TrackId, data.LayoutId, data.EventIndex, data.SessionType,
            data.SessionIteration);
    }
}
