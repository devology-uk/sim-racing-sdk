using SimRacingSdk.Acc.Core.Messages;

namespace SimRacingSdk.Acc.Monitor.Abstractions;

public interface IAccMonitor: IDisposable
{
    void Start(string? connectionIdentifier = null);
    void Stop();
    IObservable<LogMessage> LogMessages { get; }
}