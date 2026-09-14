using SimRacingSdk.Pmr.Monitor.Abstractions;
using SimRacingSdk.Pmr.Udp;
using SimRacingSdk.Pmr.Udp.Abstractions;

namespace SimRacingSdk.Pmr.Monitor;

public class PmrMonitorFactory : IPmrMonitorFactory
{
    private static IPmrMonitorFactory? singletonInstance;
    private readonly IPmrUdpConnectionFactory pmrUdpConnectionFactory;

    public PmrMonitorFactory(IPmrUdpConnectionFactory pmrUdpConnectionFactory)
    {
        this.pmrUdpConnectionFactory = pmrUdpConnectionFactory;
    }

    public static IPmrMonitorFactory Instance =>
        singletonInstance ??= new PmrMonitorFactory(PmrUdpConnectionFactory.Instance);

    public IPmrMonitor Create()
    {
        return new PmrMonitor(this.pmrUdpConnectionFactory);
    }
}
