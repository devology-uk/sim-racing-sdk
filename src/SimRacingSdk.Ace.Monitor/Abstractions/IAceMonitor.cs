using SimRacingSdk.Ace.Monitor.Messages;
using SimRacingSdk.Ace.SharedMemory.Models;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ace.Monitor.Abstractions;

public interface IAceMonitor : IDisposable
{
    IObservable<bool> IsWhiteFlagActive { get; }
    IObservable<bool> IsYellowFlagActive { get; }
    IObservable<AceMonitorLap> LapCompleted { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<AceMonitorSession> SessionCompleted { get; }
    IObservable<AceMonitorSession> SessionStarted { get; }
    IObservable<AceTelemetryFrame> Telemetry { get; }
    void Start();
    void Stop();
}
