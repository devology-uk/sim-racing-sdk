using SimRacingSdk.Acc.SharedMemory.Abstractions;

namespace SimRacingSdk.Acc.SharedMemory;

public class AccSharedMemoryConnectionFactory : IAccSharedMemoryConnectionFactory
{
    private static AccSharedMemoryConnectionFactory? singletonInstance;
    public static AccSharedMemoryConnectionFactory Instance =>
        singletonInstance ??= new AccSharedMemoryConnectionFactory();

    public IAccSharedMemoryConnection Create()
    {
        return new AccSharedMemoryConnection(new AccSharedMemoryProvider());
    }
}