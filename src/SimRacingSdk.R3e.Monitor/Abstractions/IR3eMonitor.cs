using SimRacingSdk.Core.Messages;
using SimRacingSdk.R3e.Monitor.Messages;
using SimRacingSdk.R3e.SharedMemory.Models;

namespace SimRacingSdk.R3e.Monitor.Abstractions;

public interface IR3eMonitor : IDisposable
{
    IObservable<R3eMonitorLap> LapCompleted { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<R3eMonitorSession> SessionCompleted { get; }
    IObservable<R3eMonitorSession> SessionStarted { get; }
    IObservable<R3eTelemetryFrame> Telemetry { get; }
    void Start();
    void Stop();
}
