using SimRacingSdk.Acc.SharedMemory.Abstractions;

namespace SimRacingSdk.Acc.SharedMemory;

public class AccSharedMemoryConnectionFactory : IAccSharedMemoryConnectionFactory
{
    private static AccSharedMemoryConnectionFactory? singletonInstance;
    private readonly IAccSharedMemoryProvider accSharedMemoryProvider;

    public AccSharedMemoryConnectionFactory(IAccSharedMemoryProvider accSharedMemoryProvider)
    {
        this.accSharedMemoryProvider = accSharedMemoryProvider;
    }

    public static AccSharedMemoryConnectionFactory Instance =>
        singletonInstance ??= new AccSharedMemoryConnectionFactory(AccSharedMemoryProvider.Instance);

    public IAccSharedMemoryConnection Create()
    {
        return new AccSharedMemoryConnection(this.accSharedMemoryProvider);
    }
}