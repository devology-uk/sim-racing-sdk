using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Acc.Monitor.Messages;
using SimRacingSdk.Acc.SharedMemory.Models;
using SimRacingSdk.Acc.Udp.Messages;

namespace SimRacingSdk.Acc.Monitor.Abstractions;

public interface IAccMonitor : IDisposable
{
    IObservable<AccMonitorAccident> Accidents { get; }
    IObservable<IList<AccMonitorEventEntry>> EntryList { get; }
    IObservable<AccMonitorEvent> EventCompleted { get; }
    IObservable<AccMonitorEventEntry> EventEntries { get; }
    IObservable<AccMonitorEvent> EventStarted { get; }
    IObservable<AccMonitorGreenFlag> GreenFlag { get; }
    IObservable<bool> IsWhiteFlagActive { get; }
    IObservable<bool> IsYellowFlagActive { get; }
    IObservable<AccMonitorLap> LapCompleted { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<AccMonitorPenalty> Penalties { get; }
    IObservable<AccMonitorLap> PersonalBestLap { get; }
    IObservable<AccMonitorSessionPhaseChange> PhaseChanged { get; }
    IObservable<RealtimeCarUpdate> RealtimeCarUpdates { get; }
    IObservable<AccMonitorLap> SessionBestLap { get; }
    IObservable<AccMonitorSessionChange> SessionChanged { get; }
    IObservable<AccMonitorSession> SessionCompleted { get; }
    IObservable<AccMonitorSession> SessionStarted { get; }
    IObservable<AccTelemetryFrame> Telemetry { get; }
    void Start(string? connectionIdentifier = null);
    void Stop();
}