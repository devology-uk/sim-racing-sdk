using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.Core.Services;
using SimRacingSdk.Lmu.SharedMemory.Abstractions;

namespace SimRacingSdk.Lmu.SharedMemory;

public class LmuSharedMemoryConnectionFactory : ILmuSharedMemoryConnectionFactory
{
    private static LmuSharedMemoryConnectionFactory? singletonInstance;

    private readonly ILmuSharedMemoryPluginInstaller lmuSharedMemoryPluginInstaller;
    private readonly IRfactor2SharedMemoryPluginInstaller rfactor2SharedMemoryPluginInstaller;
    private readonly ILmuSharedMemoryProvider sharedMemoryProvider;

    public LmuSharedMemoryConnectionFactory(ILmuSharedMemoryProvider sharedMemoryProvider,
        ILmuSharedMemoryPluginInstaller lmuSharedMemoryPluginInstaller,
        IRfactor2SharedMemoryPluginInstaller rfactor2SharedMemoryPluginInstaller)
    {
        this.sharedMemoryProvider = sharedMemoryProvider;
        this.lmuSharedMemoryPluginInstaller = lmuSharedMemoryPluginInstaller;
        this.rfactor2SharedMemoryPluginInstaller = rfactor2SharedMemoryPluginInstaller;
    }

    public static LmuSharedMemoryConnectionFactory Instance =>
        singletonInstance ??= new LmuSharedMemoryConnectionFactory(LmuSharedMemoryProvider.Instance,
            LmuSharedMemoryPluginInstaller.Instance,
            Rfactor2SharedMemoryPluginInstaller.Instance);

    public ILmuSharedMemoryConnection Create()
    {
        return new LmuSharedMemoryConnection(this.sharedMemoryProvider,
            this.lmuSharedMemoryPluginInstaller,
            this.rfactor2SharedMemoryPluginInstaller);
    }
}
