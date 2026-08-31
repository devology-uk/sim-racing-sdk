using SimRacingSdk.Core.Messages;
using SimRacingSdk.Lmu.SharedMemory.Models;

namespace SimRacingSdk.Lmu.SharedMemory.Abstractions;

public interface ILmuSharedMemoryConnection : IDisposable
{
    IObservable<LogMessage> LogMessages { get; }
    IObservable<LmuTelemetryFrame> Telemetry { get; }
    void Start(double updateIntervalMs = 20);
    void Stop();
}
