using SimRacingSdk.R3e.SharedMemory.Abstractions;

namespace SimRacingSdk.R3e.SharedMemory;

public class R3eSharedMemoryConnectionFactory : IR3eSharedMemoryConnectionFactory
{
    private static R3eSharedMemoryConnectionFactory? singletonInstance;

    private readonly IR3eSharedMemoryProvider sharedMemoryProvider;

    public R3eSharedMemoryConnectionFactory(IR3eSharedMemoryProvider sharedMemoryProvider)
    {
        this.sharedMemoryProvider = sharedMemoryProvider;
    }

    public static R3eSharedMemoryConnectionFactory Instance =>
        singletonInstance ??= new R3eSharedMemoryConnectionFactory(R3eSharedMemoryProvider.Instance);

    public IR3eSharedMemoryConnection Create()
    {
        return new R3eSharedMemoryConnection(this.sharedMemoryProvider);
    }
}
