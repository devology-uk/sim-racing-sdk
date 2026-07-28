using SimRacingSdk.Ace.Monitor.Messages;
using SimRacingSdk.Ace.SharedMemory.Models;
using SimRacingSdk.Ace.Udp.Messages;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ace.Monitor.Abstractions;

public interface IAceMonitor : IDisposable
{
    IObservable<AceMonitorAccident> Accidents { get; }
    IObservable<IList<AceMonitorEntry>> EntryList { get; }
    IObservable<AceMonitorEntry> Entries { get; }
    IObservable<AceMonitorEvent> EventEnded { get; }
    IObservable<AceMonitorEvent> EventStarted { get; }
    IObservable<AceMonitorGreenFlag> GreenFlag { get; }
    IObservable<bool> IsWhiteFlagActive { get; }
    IObservable<bool> IsYellowFlagActive { get; }
    IObservable<AceMonitorLap> LapCompleted { get; }
    IObservable<LogMessage> LogMessages { get; }
    IObservable<AceMonitorPenalty> Penalties { get; }
    IObservable<AceMonitorLap> PersonalBestLap { get; }
    IObservable<AceMonitorSessionPhaseChange> PhaseChanged { get; }
    IObservable<RealtimeCarUpdate> RealtimeCarUpdates { get; }
    IObservable<AceMonitorLap> SessionBestLap { get; }
    IObservable<AceMonitorSessionTypeChange> SessionTypeChanged { get; }
    IObservable<AceMonitorSession> SessionCompleted { get; }
    IObservable<AceMonitorSession> SessionStarted { get; }
    IObservable<AceTelemetryFrame> Telemetry { get; }
    void Start(string? connectionIdentifier = null);
    void Stop();
    void RequestEntryList();
}
