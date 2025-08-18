using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.SharedMemory.Abstractions;

public interface IAccSharedMemoryConnection: IDisposable
{
    IObservable<AccTelemetryFrame> Frames { get; }
    IObservable<AccTelemetryEvent> NewEvent { get; }
    IObservable<AccTelemetryLap> NewLap { get; }
    void Start(double updateIntervalMs = 100);
}