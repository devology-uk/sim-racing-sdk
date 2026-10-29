using SimRacingSdk.Core.Messages;
using SimRacingSdk.R3e.SharedMemory.Models;

namespace SimRacingSdk.R3e.SharedMemory.Abstractions;

public interface IR3eSharedMemoryConnection : IDisposable
{
    IObservable<R3eSharedMemoryLap> Laps { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<R3eSharedMemorySession> SessionEnded { get; }
    IObservable<R3eSharedMemorySession> SessionStarted { get; }
    IObservable<R3eTelemetryFrame> Telemetry { get; }
    void Start(double updateIntervalMs = 20);
    void Stop();
}
