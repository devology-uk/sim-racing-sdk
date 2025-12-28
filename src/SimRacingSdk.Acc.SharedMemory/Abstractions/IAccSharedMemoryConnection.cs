using SimRacingSdk.Acc.SharedMemory.Models;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Acc.SharedMemory.Abstractions;

public interface IAccSharedMemoryConnection : IDisposable
{
    IObservable<AccAppStatusChange> AppStatusChanges { get; }
    IObservable<AccFlagState> FlagState { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<AccSharedMemoryEvent> EventStarted { get; }
    IObservable<AccSharedMemoryLap> Laps { get; }
    IObservable<AccSharedMemorySession> SessionStarted { get; }
    IObservable<AccTelemetryFrame> Telemetry { get; }
    IObservable<AccSharedMemoryEvent> EventEnded { get; }
    IObservable<AccSharedMemorySession> SessionEnded { get; }
    void Start(double updateIntervalMs = 100);
    void Stop();
}