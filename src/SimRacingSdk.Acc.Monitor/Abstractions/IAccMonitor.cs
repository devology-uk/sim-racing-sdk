using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Acc.Monitor.Messages;
using SimRacingSdk.Acc.SharedMemory.Models;
using SimRacingSdk.Acc.Udp.Messages;

namespace SimRacingSdk.Acc.Monitor.Abstractions;

public interface IAccMonitor : IDisposable
{
    IObservable<AccMonitorAccident> Accidents { get; }
    IObservable<AccMonitorLap> LapCompleted { get; }
    IObservable<IList<AccMonitorEventEntry>> EntryList { get; }
    IObservable<AccMonitorEvent> EventEnded { get; }
    IObservable<AccMonitorEventEntry> EventEntries { get; }
    IObservable<AccMonitorEvent> EventStarted { get; }
    IObservable<AccMonitorGreenFlag> GreenFlag { get; }
    IObservable<bool> IsWhiteFlagActive { get; }
    IObservable<bool> IsYellowFlagActive { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<AccMonitorPenalty> Penalties { get; }
    IObservable<AccMonitorLap> PersonalBestLap { get; }
    IObservable<SessionPhase> CurrentPhase { get; }
    IObservable<RealtimeCarUpdate> RealtimeCarUpdates { get; }
    IObservable<AccMonitorLap> SessionBestLap { get; }
    IObservable<AccMonitorSession> SessionEnded { get; }
    IObservable<AccMonitorSession> SessionOver { get; }
    IObservable<AccMonitorSession> SessionStarted { get; }
    IObservable<AccTelemetryFrame> Telemetry { get; }
    void Start(string? connectionIdentifier = null);
    void Stop();
}