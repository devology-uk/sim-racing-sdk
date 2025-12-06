using SimRacingSdk.Acc.SharedMemory.Models;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Acc.SharedMemory.Abstractions;

public interface IAccSharedMemoryConnection : IDisposable
{
    IObservable<AccAppStatusChange> AppStatusChanges { get; }
    IObservable<AccSharedMemoryConnectedState> ConnectedState { get; }
    IObservable<AccFlagState> FlagState { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<AccSharedMemoryEvent> NewEvent { get; }
    IObservable<AccSharedMemoryLap> NewLap { get; }
    IObservable<AccSharedMemorySession> NewSession { get; }
    IObservable<AccTelemetryFrame> Telemetry { get; }
    void Start(double updateIntervalMs = 100);
    void Stop();
}