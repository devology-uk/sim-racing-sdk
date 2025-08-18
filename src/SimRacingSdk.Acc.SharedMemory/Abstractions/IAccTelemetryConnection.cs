using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.SharedMemory.Abstractions;

public interface IAccTelemetryConnection : IDisposable
{
    IObservable<AccTelemetryFrame> Frames { get; }
    void Start(double updateIntervalMs = 100);
}