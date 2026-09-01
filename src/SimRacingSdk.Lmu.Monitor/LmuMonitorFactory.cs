using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.Core.Services;
using SimRacingSdk.Lmu.Monitor.Abstractions;
using SimRacingSdk.Lmu.SharedMemory;
using SimRacingSdk.Lmu.SharedMemory.Abstractions;

namespace SimRacingSdk.Lmu.Monitor;

public class LmuMonitorFactory : ILmuMonitorFactory
{
    private static ILmuMonitorFactory? singletonInstance;
    private readonly ILmuCarInfoProvider lmuCarInfoProvider;
    private readonly ILmuSharedMemoryConnectionFactory lmuSharedMemoryConnectionFactory;

    public LmuMonitorFactory(ILmuSharedMemoryConnectionFactory lmuSharedMemoryConnectionFactory,
        ILmuCarInfoProvider lmuCarInfoProvider)
    {
        this.lmuSharedMemoryConnectionFactory = lmuSharedMemoryConnectionFactory;
        this.lmuCarInfoProvider = lmuCarInfoProvider;
    }

    public static ILmuMonitorFactory Instance =>
        singletonInstance ??= new LmuMonitorFactory(LmuSharedMemoryConnectionFactory.Instance,
            LmuCarInfoProvider.Instance);

    public ILmuMonitor Create()
    {
        return new LmuMonitor(this.lmuSharedMemoryConnectionFactory, this.lmuCarInfoProvider);
    }
}
