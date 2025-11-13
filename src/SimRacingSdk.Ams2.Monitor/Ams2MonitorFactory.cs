using SimRacingSdk.Ams2.Core;
using SimRacingSdk.Ams2.Core.Abstractions;
using SimRacingSdk.Ams2.Monitor.Abstractions;
using SimRacingSdk.Ams2.SharedMemory;
using SimRacingSdk.Ams2.SharedMemory.Abstractions;

namespace SimRacingSdk.Ams2.Monitor;

public class Ams2MonitorFactory : IAms2MonitorFactory
{
    private static IAms2MonitorFactory? singletonInstance;
    private readonly IAms2CarInfoProvider ams2CarInfoProvider;
    private readonly IAms2NationalityInfoProvider ams2NationalityInfoProvider;
    private readonly IAms2SharedMemoryConnectionFactory ams2SharedMemoryConnectionFactory;

    public Ams2MonitorFactory(IAms2SharedMemoryConnectionFactory ams2SharedMemoryConnectionFactory,
        IAms2CarInfoProvider ams2CarInfoProvider,
        IAms2NationalityInfoProvider ams2NationalityInfoProvider)
    {
        this.ams2SharedMemoryConnectionFactory = ams2SharedMemoryConnectionFactory;
        this.ams2CarInfoProvider = ams2CarInfoProvider;
        this.ams2NationalityInfoProvider = ams2NationalityInfoProvider;
    }

    public static IAms2MonitorFactory Instance =>
        singletonInstance ??= new Ams2MonitorFactory(Ams2SharedMemoryConnectionFactory.Instance,
            Ams2CarInfoProvider.Instance,
            Ams2NationalityInfoProvider.Instance);

    public IAms2Monitor Create()
    {
        return new Ams2Monitor(this.ams2SharedMemoryConnectionFactory,
            this.ams2CarInfoProvider,
            this.ams2NationalityInfoProvider);
    }
}