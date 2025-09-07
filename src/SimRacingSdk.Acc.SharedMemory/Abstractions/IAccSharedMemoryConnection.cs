using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.SharedMemory.Abstractions;

public interface IAccSharedMemoryConnection: IDisposable
{
    IObservable<AccTelemetryFrame> Telemetry { get; }
    IObservable<AccSharedMemoryEvent> NewEvent { get; }
    IObservable<AccSharedMemoryLap> NewLap { get; }
    IObservable<LogMessage> LogMessages { get; }
    void Start(double updateIntervalMs = 100);
}