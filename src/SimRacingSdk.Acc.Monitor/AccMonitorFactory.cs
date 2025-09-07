using SimRacingSdk.Acc.Core;
using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Acc.Monitor.Abstractions;
using SimRacingSdk.Acc.SharedMemory;
using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.Udp;
using SimRacingSdk.Acc.Udp.Abstractions;

namespace SimRacingSdk.Acc.Monitor;

public class AccMonitorFactory : IAccMonitorFactory
{
    private static IAccMonitorFactory? singletonInstance;
    private readonly IAccCarInfoProvider accCarInfoProvider;
    private readonly IAccNationalityInfoProvider accNationalityInfoProvider;
    private readonly IAccCompatibilityChecker accCompatibilityChecker;
    private readonly IAccLocalConfigProvider accLocalConfigProvider;
    private readonly IAccSharedMemoryConnectionFactory accSharedMemoryConnectionFactory;
    private readonly IAccUdpConnectionFactory accUdpConnectionFactory;

    public AccMonitorFactory(IAccUdpConnectionFactory accUdpConnectionFactory,
        IAccSharedMemoryConnectionFactory accSharedMemoryConnectionFactory,
        IAccCompatibilityChecker accCompatibilityChecker,
        IAccLocalConfigProvider accLocalConfigProvider,
        IAccCarInfoProvider accCarInfoProvider,
        IAccNationalityInfoProvider accNationalityInfoProvider)
    {
        this.accUdpConnectionFactory = accUdpConnectionFactory;
        this.accSharedMemoryConnectionFactory = accSharedMemoryConnectionFactory;
        this.accCompatibilityChecker = accCompatibilityChecker;
        this.accLocalConfigProvider = accLocalConfigProvider;
        this.accCarInfoProvider = accCarInfoProvider;
        this.accNationalityInfoProvider = accNationalityInfoProvider;
    }

    public static IAccMonitorFactory Instance =>
        singletonInstance ??= new AccMonitorFactory(AccUdpConnectionFactory.Instance,
            AccSharedMemoryConnectionFactory.Instance,
            AccCompatibilityChecker.Instance,
            AccLocalConfigProvider.Instance,
            AccCarInfoProvider.Instance,
            AccNationalityInfoProvider.Instance);

    public IAccMonitor Create()
    {
        return new AccMonitor(this.accUdpConnectionFactory,
            this.accSharedMemoryConnectionFactory,
            this.accCompatibilityChecker,
            this.accLocalConfigProvider,
            this.accCarInfoProvider,
            this.accNationalityInfoProvider);
    }
}