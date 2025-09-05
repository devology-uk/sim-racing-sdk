using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Acc.Monitor.Messages;
using SimRacingSdk.Acc.Udp.Messages;

namespace SimRacingSdk.Acc.Monitor.Abstractions;

public interface IAccMonitor : IDisposable
{
    IObservable<AccAccident> Accidents { get; }
    IObservable<AccLap> CompletedLaps { get; }
    IObservable<IList<AccEventEntry>> EntryList { get; }
    IObservable<AccEvent> EventEnded { get; }
    IObservable<AccEventEntry> EventEntries { get; }
    IObservable<AccEvent> EventStarted { get; }
    IObservable<AccGreenFlag> GreenFlag { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<AccPenalty> Penalties { get; }
    IObservable<AccLap> PersonalBestLap { get; }
    IObservable<AccSessionPhase> PhaseEnded { get; }
    IObservable<AccSessionPhase> PhaseStarted { get; }
    IObservable<RealtimeCarUpdate> RealtimeCarUpdates { get; }
    IObservable<AccLap> SessionBestLap { get; }
    IObservable<AccSession> SessionEnded { get; }
    IObservable<AccSession> SessionOver { get; }
    IObservable<AccSession> SessionStarted { get; }
    void Start(string? connectionIdentifier = null);
    void Stop();
}