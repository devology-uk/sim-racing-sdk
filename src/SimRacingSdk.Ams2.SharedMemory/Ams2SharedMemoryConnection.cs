using SimRacingSdk.Ams2.SharedMemory.Abstractions;

namespace SimRacingSdk.Ams2.SharedMemory;

public class Ams2SharedMemoryConnection : IAms2SharedMemoryConnection
{
    private readonly IAms2SharedMemoryProvider ams2SharedMemoryProvider;

    public Ams2SharedMemoryConnection(IAms2SharedMemoryProvider ams2SharedMemoryProvider)
    {
        this.ams2SharedMemoryProvider = ams2SharedMemoryProvider;
    }
}