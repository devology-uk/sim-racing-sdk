using SimRacingSdk.Ace.Core;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Monitor.Abstractions;
using SimRacingSdk.Ace.SharedMemory;
using SimRacingSdk.Ace.SharedMemory.Abstractions;

namespace SimRacingSdk.Ace.Monitor;

public class AceMonitorFactory : IAceMonitorFactory
{
    private static IAceMonitorFactory? singletonInstance;
    private readonly IAceCarInfoProvider aceCarInfoProvider;
    private readonly IAceSharedMemoryConnectionFactory aceSharedMemoryConnectionFactory;

    public AceMonitorFactory(IAceSharedMemoryConnectionFactory aceSharedMemoryConnectionFactory,
        IAceCarInfoProvider aceCarInfoProvider)
    {
        this.aceSharedMemoryConnectionFactory = aceSharedMemoryConnectionFactory;
        this.aceCarInfoProvider = aceCarInfoProvider;
    }

    public static IAceMonitorFactory Instance =>
        singletonInstance ??= new AceMonitorFactory(AceSharedMemoryConnectionFactory.Instance,
            AceCarInfoProvider.Instance);

    public IAceMonitor Create()
    {
        return new AceMonitor(this.aceSharedMemoryConnectionFactory, this.aceCarInfoProvider);
    }
}
