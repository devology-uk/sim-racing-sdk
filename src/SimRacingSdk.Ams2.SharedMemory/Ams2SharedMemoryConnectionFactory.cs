using SimRacingSdk.Ams2.SharedMemory.Abstractions;

namespace SimRacingSdk.Ams2.SharedMemory;

public class Ams2SharedMemoryConnectionFactory : IAms2SharedMemoryConnectionFactory
{
    private static Ams2SharedMemoryConnectionFactory? singletonInstance;
    private readonly IAms2SharedMemoryProvider ams2SharedMemoryProvider;

    public Ams2SharedMemoryConnectionFactory(IAms2SharedMemoryProvider ams2SharedMemoryProvider)
    {
        this.ams2SharedMemoryProvider = ams2SharedMemoryProvider;
    }

    public static Ams2SharedMemoryConnectionFactory Instance =>
        singletonInstance ??= new Ams2SharedMemoryConnectionFactory(Ams2SharedMemoryProvider.Instance);

    public IAms2SharedMemoryConnection Create()
    {
        return new Ams2SharedMemoryConnection(this.ams2SharedMemoryProvider);
    }
}