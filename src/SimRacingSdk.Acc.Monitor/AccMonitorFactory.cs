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
    private readonly IAccTelemetryConnectionFactory accTelemetryConnectionFactory;
    private readonly IAccUdpConnectionFactory accUdpConnectionFactory;

    public AccMonitorFactory(IAccUdpConnectionFactory accUdpConnectionFactory,
        IAccTelemetryConnectionFactory accTelemetryConnectionFactory,
        IAccCompatibilityChecker accCompatibilityChecker,
        IAccLocalConfigProvider accLocalConfigProvider,
        IAccCarInfoProvider accCarInfoProvider,
        IAccNationalityInfoProvider accNationalityInfoProvider)
    {
        this.accUdpConnectionFactory = accUdpConnectionFactory;
        this.accTelemetryConnectionFactory = accTelemetryConnectionFactory;
        this.accCompatibilityChecker = accCompatibilityChecker;
        this.accLocalConfigProvider = accLocalConfigProvider;
        this.accCarInfoProvider = accCarInfoProvider;
        this.accNationalityInfoProvider = accNationalityInfoProvider;
    }

    public static IAccMonitorFactory Instance =>
        singletonInstance ??= new AccMonitorFactory(AccUdpConnectionFactory.Instance,
            AccTelemetryConnectionFactory.Instance,
            AccCompatibilityChecker.Instance,
            AccLocalConfigProvider.Instance,
            AccCarInfoProvider.Instance,
            AccNationalityInfoProvider.Instance);

    public IAccMonitor Create()
    {
        return new AccMonitor(this.accUdpConnectionFactory,
            this.accTelemetryConnectionFactory,
            this.accCompatibilityChecker,
            this.accLocalConfigProvider,
            this.accCarInfoProvider,
            this.accNationalityInfoProvider);
    }
}