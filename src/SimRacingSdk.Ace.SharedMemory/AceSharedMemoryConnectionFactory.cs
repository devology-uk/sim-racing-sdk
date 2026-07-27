using SimRacingSdk.Ace.SharedMemory.Abstractions;

namespace SimRacingSdk.Ace.SharedMemory;

public class AceSharedMemoryConnectionFactory : IAceSharedMemoryConnectionFactory
{
    private static AceSharedMemoryConnectionFactory? singletonInstance;
    private readonly IAceSharedMemoryProvider aceSharedMemoryProvider;

    public AceSharedMemoryConnectionFactory(IAceSharedMemoryProvider aceSharedMemoryProvider)
    {
        this.aceSharedMemoryProvider = aceSharedMemoryProvider;
    }

    public static AceSharedMemoryConnectionFactory Instance =>
        singletonInstance ??= new AceSharedMemoryConnectionFactory(AceSharedMemoryProvider.Instance);

    public IAceSharedMemoryConnection Create()
    {
        return new AceSharedMemoryConnection(this.aceSharedMemoryProvider);
    }
}
