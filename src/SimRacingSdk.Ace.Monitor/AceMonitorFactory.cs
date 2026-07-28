using SimRacingSdk.Ace.Core;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Monitor.Abstractions;
using SimRacingSdk.Ace.SharedMemory;
using SimRacingSdk.Ace.SharedMemory.Abstractions;
using SimRacingSdk.Ace.Udp;
using SimRacingSdk.Ace.Udp.Abstractions;

namespace SimRacingSdk.Ace.Monitor;

public class AceMonitorFactory : IAceMonitorFactory
{
    private static IAceMonitorFactory? singletonInstance;
    private readonly IAceCarInfoProvider aceCarInfoProvider;
    private readonly IAceNationalityInfoProvider aceNationalityInfoProvider;
    private readonly IAceCompatibilityChecker aceCompatibilityChecker;
    private readonly IAceLocalConfigProvider aceLocalConfigProvider;
    private readonly IAceSharedMemoryConnectionFactory aceSharedMemoryConnectionFactory;
    private readonly IAceUdpConnectionFactory aceUdpConnectionFactory;

    public AceMonitorFactory(IAceUdpConnectionFactory aceUdpConnectionFactory,
        IAceSharedMemoryConnectionFactory aceSharedMemoryConnectionFactory,
        IAceCompatibilityChecker aceCompatibilityChecker,
        IAceLocalConfigProvider aceLocalConfigProvider,
        IAceCarInfoProvider aceCarInfoProvider,
        IAceNationalityInfoProvider aceNationalityInfoProvider)
    {
        this.aceUdpConnectionFactory = aceUdpConnectionFactory;
        this.aceSharedMemoryConnectionFactory = aceSharedMemoryConnectionFactory;
        this.aceCompatibilityChecker = aceCompatibilityChecker;
        this.aceLocalConfigProvider = aceLocalConfigProvider;
        this.aceCarInfoProvider = aceCarInfoProvider;
        this.aceNationalityInfoProvider = aceNationalityInfoProvider;
    }

    public static IAceMonitorFactory Instance =>
        singletonInstance ??= new AceMonitorFactory(AceUdpConnectionFactory.Instance,
            AceSharedMemoryConnectionFactory.Instance,
            AceCompatibilityChecker.Instance,
            AceLocalConfigProvider.Instance,
            AceCarInfoProvider.Instance,
            AceNationalityInfoProvider.Instance);

    public IAceMonitor Create()
    {
        return new AceMonitor(this.aceUdpConnectionFactory,
            this.aceSharedMemoryConnectionFactory,
            this.aceCompatibilityChecker,
            this.aceLocalConfigProvider,
            this.aceCarInfoProvider,
            this.aceNationalityInfoProvider);
    }
}
