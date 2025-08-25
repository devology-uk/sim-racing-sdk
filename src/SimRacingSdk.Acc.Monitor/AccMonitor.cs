using SimRacingSdk.Acc.Monitor.Abstractions;
using SimRacingSdk.Acc.SharedMemory;
using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.Udp;
using SimRacingSdk.Acc.Udp.Abstractions;

namespace SimRacingSdk.Acc.Monitor;

public class AccMonitor : IAccMonitor
{
    private static IAccMonitor? singletonInstance;
    private readonly IAccTelemetryConnectionFactory accTelemetryConnectionFactory;
    private readonly IAccUdpConnectionFactory accUdpConnectionFactory;

    public AccMonitor(IAccUdpConnectionFactory accUdpConnectionFactory,
        IAccTelemetryConnectionFactory accTelemetryConnectionFactory)
    {
        this.accUdpConnectionFactory = accUdpConnectionFactory;
        this.accTelemetryConnectionFactory = accTelemetryConnectionFactory;
    }

    public static IAccMonitor Instance()
    {
        return singletonInstance ??= new AccMonitor(AccUdpConnectionFactory.Instance,
                   AccTelemetryConnectionFactory.Instance);
    }

    public void Start(){}
    public void Stop(){ }

    public void Dispose()
    {
         this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!disposing)
        {
            return;
        }

        this.Stop();
    }
}