using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ams2.SharedMemory.Abstractions;

public interface IAms2SharedMemoryConnection : IDisposable
{
    IObservable<LogMessage> LogMessages { get; }
    void Start(double updateIntervalMs = 300);
    void Stop();
}