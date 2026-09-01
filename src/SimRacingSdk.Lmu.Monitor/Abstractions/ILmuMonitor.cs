using SimRacingSdk.Core.Messages;
using SimRacingSdk.Lmu.Monitor.Messages;
using SimRacingSdk.Lmu.SharedMemory.Models;

namespace SimRacingSdk.Lmu.Monitor.Abstractions;

public interface ILmuMonitor : IDisposable
{
    IObservable<LmuMonitorLap> LapCompleted { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<LmuMonitorSession> SessionCompleted { get; }
    IObservable<LmuMonitorSession> SessionStarted { get; }
    IObservable<LmuTelemetryFrame> Telemetry { get; }
    void Start();
    void Stop();
}
