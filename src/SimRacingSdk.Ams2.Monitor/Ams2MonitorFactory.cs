using SimRacingSdk.Ams2.Core;
using SimRacingSdk.Ams2.Core.Abstractions;
using SimRacingSdk.Ams2.Monitor.Abstractions;
using SimRacingSdk.Ams2.SharedMemory;
using SimRacingSdk.Ams2.SharedMemory.Abstractions;

namespace SimRacingSdk.Ams2.Monitor;

public class Ams2MonitorFactory : IAms2MonitorFactory
{
    private static IAms2MonitorFactory? singletonInstance;
    private readonly IAms2CarInfoProvider accCarInfoProvider;
    private readonly IAms2CompatibilityChecker accCompatibilityChecker;
    private readonly IAms2SharedMemoryConnectionFactory accSharedMemoryConnectionFactory;

    public Ams2MonitorFactory(IAms2SharedMemoryConnectionFactory accSharedMemoryConnectionFactory,
        IAms2CompatibilityChecker accCompatibilityChecker,
        IAms2CarInfoProvider accCarInfoProvider)
    {
        this.accSharedMemoryConnectionFactory = accSharedMemoryConnectionFactory;
        this.accCompatibilityChecker = accCompatibilityChecker;
        this.accCarInfoProvider = accCarInfoProvider;
    }

    public static IAms2MonitorFactory Instance =>
        singletonInstance ??= new Ams2MonitorFactory(Ams2SharedMemoryConnectionFactory.Instance,
            Ams2CompatibilityChecker.Instance,
            Ams2CarInfoProvider.Instance);

    public IAms2Monitor Create()
    {
        return new Ams2Monitor(this.accSharedMemoryConnectionFactory,
            this.accCompatibilityChecker,
            this.accCarInfoProvider);
    }