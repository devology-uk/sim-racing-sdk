using SimRacingSdk.R3e.Core;
using SimRacingSdk.R3e.Core.Abstractions;
using SimRacingSdk.R3e.Monitor.Abstractions;
using SimRacingSdk.R3e.SharedMemory;
using SimRacingSdk.R3e.SharedMemory.Abstractions;

namespace SimRacingSdk.R3e.Monitor;

public class R3eMonitorFactory : IR3eMonitorFactory
{
    private static IR3eMonitorFactory? singletonInstance;
    private readonly IR3eCarInfoProvider r3eCarInfoProvider;
    private readonly IR3eSharedMemoryConnectionFactory r3eSharedMemoryConnectionFactory;
    private readonly IR3eTrackInfoProvider r3eTrackInfoProvider;

    public R3eMonitorFactory(IR3eSharedMemoryConnectionFactory r3eSharedMemoryConnectionFactory,
        IR3eCarInfoProvider r3eCarInfoProvider,
        IR3eTrackInfoProvider r3eTrackInfoProvider)
    {
        this.r3eSharedMemoryConnectionFactory = r3eSharedMemoryConnectionFactory;
        this.r3eCarInfoProvider = r3eCarInfoProvider;
        this.r3eTrackInfoProvider = r3eTrackInfoProvider;
    }

    public static IR3eMonitorFactory Instance =>
        singletonInstance ??= new R3eMonitorFactory(R3eSharedMemoryConnectionFactory.Instance,
            R3eCarInfoProvider.Instance,
            R3eTrackInfoProvider.Instance);

    public IR3eMonitor Create()
    {
        return new R3eMonitor(this.r3eSharedMemoryConnectionFactory, this.r3eCarInfoProvider,
            this.r3eTrackInfoProvider);
    }
}
