using SimRacingSdk.Ace.SharedMemory.Models;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ace.SharedMemory.Abstractions;

public interface IAceSharedMemoryConnection : IDisposable
{
    IObservable<AceAppStatusChange> AppStatusChanges { get; }
    IObservable<AceFlagState> FlagState { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<AceSharedMemoryLap> Laps { get; }
    IObservable<AceSharedMemorySession> SessionStarted { get; }
    IObservable<AceTelemetryFrame> Telemetry { get; }
    IObservable<AceSharedMemorySession> SessionEnded { get; }
    void Start(double updateIntervalMs = 100);
    void Stop();
}
