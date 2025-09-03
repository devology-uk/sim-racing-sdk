using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Acc.Monitor.Messages;

namespace SimRacingSdk.Acc.Monitor.Abstractions;

public interface IAccMonitor : IDisposable
{
    IObservable<IList<AccEventEntry>> EntryList { get; }
    IObservable<AccEvent> EventEnded { get; }
    IObservable<AccEventEntry> EventEntries { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<AccEvent> EventStarted { get; }
    IObservable<AccSession> SessionUpdates { get; }
    void Start(string? connectionIdentifier = null);
    void Stop();
}