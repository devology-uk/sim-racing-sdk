using SimRacingSdk.Core.Messages;
using SimRacingSdk.Lmu.SharedMemory.Models;

namespace SimRacingSdk.Lmu.SharedMemory.Abstractions;

public interface ILmuSharedMemoryConnection : IDisposable
{
    IObservable<LmuSharedMemoryLap> Laps { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<LmuSharedMemorySession> SessionEnded { get; }
    IObservable<LmuSharedMemorySession> SessionStarted { get; }
    IObservable<LmuTelemetryFrame> Telemetry { get; }
    void Start(double updateIntervalMs = 20);
    void Stop();
}
