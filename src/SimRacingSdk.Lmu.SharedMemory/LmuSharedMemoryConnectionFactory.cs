using SimRacingSdk.Lmu.SharedMemory.Abstractions;

namespace SimRacingSdk.Lmu.SharedMemory;

public class LmuSharedMemoryConnectionFactory : ILmuSharedMemoryConnectionFactory
{
    private static LmuSharedMemoryConnectionFactory? singletonInstance;

    private readonly ILmuSharedMemoryProvider sharedMemoryProvider;

    public LmuSharedMemoryConnectionFactory(ILmuSharedMemoryProvider sharedMemoryProvider)
    {
        this.sharedMemoryProvider = sharedMemoryProvider;
    }

    public static LmuSharedMemoryConnectionFactory Instance =>
        singletonInstance ??= new LmuSharedMemoryConnectionFactory(LmuSharedMemoryProvider.Instance);

    public ILmuSharedMemoryConnection Create()
    {
        return new LmuSharedMemoryConnection(this.sharedMemoryProvider);
    }
}
