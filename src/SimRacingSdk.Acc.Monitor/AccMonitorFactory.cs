using SimRacingSdk.Acc.Monitor.Abstractions;
using SimRacingSdk.Acc.SharedMemory;
using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.Udp;
using SimRacingSdk.Acc.Udp.Abstractions;

namespace SimRacingSdk.Acc.Monitor;

public class AccMonitorFactory : IAccMonitorFactory
{
    private static IAccMonitorFactory? singletonInstance;
    private readonly IAccTelemetryConnectionFactory accTelemetryConnectionFactory;
    private readonly IAccUdpConnectionFactory accUdpConnectionFactory;

    public AccMonitorFactory(IAccUdpConnectionFactory accUdpConnectionFactory,
        IAccTelemetryConnectionFactory accTelemetryConnectionFactory)
    {
        this.accUdpConnectionFactory = accUdpConnectionFactory;
        this.accTelemetryConnectionFactory = accTelemetryConnectionFactory;
    }

    public static IAccMonitorFactory Instance =>
        singletonInstance ??= new AccMonitorFactory(AccUdpConnectionFactory.Instance,
            AccTelemetryConnectionFactory.Instance);

    public IAccMonitor Create(string? connectionIdentifier = null)
    {
        return new AccMonitor(this.accUdpConnectionFactory, this.accTelemetryConnectionFactory);
    }
}