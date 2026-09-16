using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Core.Services;
using SimRacingSdk.Pmr.Monitor.Abstractions;
using SimRacingSdk.Pmr.Monitor.Messages;
using SimRacingSdk.Pmr.Udp;
using SimRacingSdk.Pmr.Udp.Abstractions;
using SimRacingSdk.Pmr.Udp.Enums;
using SimRacingSdk.Pmr.Udp.Messages;

namespace SimRacingSdk.Pmr.Monitor;

public class PmrMonitor : IPmrMonitor
{
    private readonly Dictionary<(string DriverName, string LiveryId), PmrParticipantRaceState> entries = [];
    private readonly Subject<PmrMonitorLap> lapCompletedSubject = new();
    private readonly LogMessageBroker logMessageBroker = new(nameof(PmrMonitor));
    private readonly Subject<PmrParticipantRaceState> participantUpdatesSubject = new();
    private readonly IPmrUdpConnectionFactory pmrUdpConnectionFactory;
    private readonly Subject<PmrMonitorSession> sessionCompletedSubject = new();
    private readonly Subject<PmrMonitorSession> sessionStartedSubject = new();
    private readonly Subject<PmrVehicleTelemetry> telemetrySubject = new();

    private PmrMonitorSession? currentSession;
    private IPmrUdpConnection? pmrUdpConnection;
    private CompositeDisposable? subscriptionSink;

    public PmrMonitor(IPmrUdpConnectionFactory pmrUdpConnectionFactory)
    {
        this.pmrUdpConnectionFactory = pmrUdpConnectionFactory;
    }

    public IObservable<PmrMonitorLap> LapCompleted => this.lapCompletedSubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessageBroker.Messages;
    public IObservable<PmrParticipantRaceState> ParticipantUpdates => this.participantUpdatesSubject.AsObservable();
    public IObservable<PmrMonitorSession> SessionCompleted => this.sessionCompletedSubject.AsObservable();
    public IObservable<PmrMonitorSession> SessionStarted => this.sessionStartedSubject.AsObservable();
    public IObservable<PmrVehicleTelemetry> Telemetry => this.telemetrySubject.AsObservable();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Start(int port = PmrUdpConnection.DefaultPort,
        bool useMulticast = false,
        string multicastGroup = PmrUdpConnection.DefaultMulticastGroup)
    {
        this.LogMessage(LoggingLevel.Information, "Starting Pmr Monitor...");

        this.pmrUdpConnection = this.pmrUdpConnectionFactory.Create(port, useMulticast, multicastGroup);
        this.subscriptionSink = new CompositeDisposable
        {
            this.pmrUdpConnection.LogMessages.Subscribe(this.logMessageBroker.Relay),
            this.pmrUdpConnection.ParticipantRaceStates.Subscribe(this.OnNextParticipantRaceState),
            this.pmrUdpConnection.RaceInfoUpdates.Subscribe(this.OnNextRaceInfo),
            this.pmrUdpConnection.VehicleTelemetryUpdates.Subscribe(this.telemetrySubject.OnNext)
        };

        this.pmrUdpConnection.Connect();
    }

    public void Stop()
    {
        this.subscriptionSink?.Dispose();
        this.pmrUdpConnection?.Dispose();
        this.pmrUdpConnection = null;
        this.EndCurrentSession();
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!disposing)
        {
            return;
        }

        this.Stop();
    }

    private void EmitCompletedLap(PmrParticipantRaceState finishedLapState)
    {
        if(this.currentSession == null)
        {
            return;
        }

        var pmrMonitorLap = new PmrMonitorLap
        {
            CompletedLap = finishedLapState.CurrentLap,
            DriverName = finishedLapState.DriverName,
            IsDisqualified = finishedLapState.IsDisqualified,
            IsPlayer = finishedLapState.IsPlayer,
            LapTimeSeconds = finishedLapState.CurrentLapTimeSeconds,
            Position = finishedLapState.RacePosition,
            SectorTimesSeconds = finishedLapState.CurrentSectorTimesSeconds,
            SessionId = this.currentSession.SessionId,
            VehicleClass = finishedLapState.VehicleClass,
            VehicleId = finishedLapState.VehicleId,
            VehicleName = finishedLapState.VehicleName
        };

        this.LogMessage(LoggingLevel.Information, $"Lap Completed: {pmrMonitorLap}");
        this.lapCompletedSubject.OnNext(pmrMonitorLap);
    }

    private void EndCurrentSession()
    {
        if(this.currentSession == null)
        {
            return;
        }

        this.currentSession.IsRunning = false;
        this.LogMessage(LoggingLevel.Information, $"Session Completed: {this.currentSession}");
        this.sessionCompletedSubject.OnNext(this.currentSession);
        this.currentSession = null;
        this.entries.Clear();
    }

    private void LogMessage(LoggingLevel level, string content)
    {
        this.logMessageBroker.Log(level, content);
    }

    private void OnNextParticipantRaceState(PmrParticipantRaceState participantRaceState)
    {
        // Keyed by (DriverName, LiveryId), not VehicleId - a live rig test (2026-09-16) confirmed
        // PMR sends VehicleId=0 for every car in every ParticipantRaceState packet, so it can't
        // distinguish cars at all. DriverName is the primary differentiator; LiveryId is added
        // because online races let drivers pick their own name and livery independently, so two
        // drivers sharing a name (more likely than sharing both name and livery) would otherwise
        // collide onto one entry.
        var key = (participantRaceState.DriverName, participantRaceState.LiveryId);
        if(this.entries.TryGetValue(key, out var previousState)
           && participantRaceState.CurrentLap > previousState.CurrentLap)
        {
            this.EmitCompletedLap(previousState);
        }

        this.entries[key] = participantRaceState;
        this.participantUpdatesSubject.OnNext(participantRaceState);
    }

    private void OnNextRaceInfo(PmrRaceInfo raceInfo)
    {
        if(raceInfo.State != PmrRaceSessionState.Active)
        {
            this.EndCurrentSession();
            return;
        }

        // A session-name change while still Active catches a same-event rollover (e.g. Practice
        // -> Race) with no Inactive gap between them - the same heuristic LmuSharedMemoryConnection
        // uses for its own Session-int change.
        if(this.currentSession == null || this.currentSession.SessionType != raceInfo.Session)
        {
            this.EndCurrentSession();
            this.StartNewSession(raceInfo);
        }
    }

    private void StartNewSession(PmrRaceInfo raceInfo)
    {
        this.currentSession = new PmrMonitorSession
        {
            AmbientTemperatureCelsius = raceInfo.AmbientTemperatureCelsius,
            DurationSeconds = raceInfo.DurationSeconds,
            GameMode = raceInfo.GameMode,
            IsLaps = raceInfo.IsLaps,
            IsRunning = true,
            Layout = raceInfo.Layout,
            NumberOfParticipants = raceInfo.NumberOfParticipants,
            OvertimeSeconds = raceInfo.OvertimeSeconds,
            Season = raceInfo.Season,
            SessionId = Guid.NewGuid(),
            SessionType = raceInfo.Session,
            TrackName = raceInfo.Track,
            TrackTemperatureCelsius = raceInfo.TrackTemperatureCelsius,
            Weather = raceInfo.Weather
        };

        this.LogMessage(LoggingLevel.Information, $"Session Started: {this.currentSession}");
        this.sessionStartedSubject.OnNext(this.currentSession);
    }
}
