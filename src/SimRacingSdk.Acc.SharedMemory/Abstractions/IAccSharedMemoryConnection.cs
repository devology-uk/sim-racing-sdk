using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.SharedMemory.Abstractions;

public interface IAccSharedMemoryConnection : IDisposable
{
    IObservable<AccFlagState> FlagState { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<AccSharedMemoryEvent> NewEvent { get; }
    IObservable<AccSharedMemoryLap> NewLap { get; }
    IObservable<string> NewSession { get; }
    IObservable<AccTelemetryFrame> Telemetry { get; }
    IObservable<bool> ConnectedState { get; }
    void Start(double updateIntervalMs = 100);
    void Stop();
}