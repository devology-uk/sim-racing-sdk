using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Core.Services;
using SimRacingSdk.R3e.Core.Abstractions;
using SimRacingSdk.R3e.Monitor.Abstractions;
using SimRacingSdk.R3e.Monitor.Messages;
using SimRacingSdk.R3e.SharedMemory.Abstractions;
using SimRacingSdk.R3e.SharedMemory.Enums;
using SimRacingSdk.R3e.SharedMemory.Models;

namespace SimRacingSdk.R3e.Monitor;

public class R3eMonitor : IR3eMonitor
{
    private readonly Subject<R3eMonitorLap> lapCompletedSubject = new();
    private readonly LogMessageBroker logMessageBroker = new(nameof(R3eMonitor));
    private readonly IR3eCarInfoProvider r3eCarInfoProvider;
    private readonly IR3eSharedMemoryConnectionFactory r3eSharedMemoryConnectionFactory;
    private readonly IR3eTrackInfoProvider r3eTrackInfoProvider;
    private readonly Subject<R3eMonitorSession> sessionCompletedSubject = new();
    private readonly Subject<R3eMonitorSession> sessionStartedSubject = new();
    private readonly Subject<R3eTelemetryFrame> telemetrySubject = new();

    private R3eMonitorSession? currentSession;
    private IR3eSharedMemoryConnection? r3eSharedMemoryConnection;
    private CompositeDisposable? subscriptionSink;

    public R3eMonitor(IR3eSharedMemoryConnectionFactory r3eSharedMemoryConnectionFactory,
        IR3eCarInfoProvider r3eCarInfoProvider,
        IR3eTrackInfoProvider r3eTrackInfoProvider)
    {
        this.r3eSharedMemoryConnectionFactory = r3eSharedMemoryConnectionFactory;
        this.r3eCarInfoProvider = r3eCarInfoProvider;
        this.r3eTrackInfoProvider = r3eTrackInfoProvider;
    }

    public IObservable<R3eMonitorLap> LapCompleted => this.lapCompletedSubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessageBroker.Messages;
    public IObservable<R3eMonitorSession> SessionCompleted => this.sessionCompletedSubject.AsObservable();
    public IObservable<R3eMonitorSession> SessionStarted => this.sessionStartedSubject.AsObservable();
    public IObservable<R3eTelemetryFrame> Telemetry => this.telemetrySubject.AsObservable();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Start()
    {
        this.Stop();
        this.LogMessage(LoggingLevel.Information, "Starting R3E Monitor...");

        this.r3eSharedMemoryConnection = this.r3eSharedMemoryConnectionFactory.Create();
        this.subscriptionSink = new CompositeDisposable
        {
            this.r3eSharedMemoryConnection.Laps.Subscribe(this.OnNextLap),
            this.r3eSharedMemoryConnection.LogMessages.Subscribe(this.logMessageBroker.Relay),
            this.r3eSharedMemoryConnection.SessionEnded.Subscribe(this.OnNextSessionEnded),
            this.r3eSharedMemoryConnection.SessionStarted.Subscribe(this.OnNextSessionStarted),
            this.r3eSharedMemoryConnection.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };

        this.r3eSharedMemoryConnection.Start();
    }

    // The connection goes first because stopping it ends the running session, which is the only thing that ends one
    // when the game exits, and the subscriptions have to still be there to hear it.
    public void Stop()
    {
        this.r3eSharedMemoryConnection?.Dispose();
        this.r3eSharedMemoryConnection = null;
        this.subscriptionSink?.Dispose();
        this.subscriptionSink = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!disposing)
        {
            return;
        }

        this.Stop();
    }

    private static bool IsOnline(R3eGameMode gameMode)
    {
        return gameMode is R3eGameMode.Multiplayer or R3eGameMode.MultiplayerRanked;
    }

    private static bool IsSimulatedFinish(R3eSharedMemoryLap lap)
    {
        return !lap.IsLocalPlayer && lap.FinishStatus == R3eFinishStatus.Finished && lap.LapTimeMs is null;
    }

    private static bool IsValidTimedLap(R3eSharedMemoryLap lap)
    {
        return lap.IsValid == true && lap.LapTimeMs is not null;
    }

    private void LogMessage(LoggingLevel level, string content)
    {
        this.logMessageBroker.Log(level, content);
    }

    private void OnNextLap(R3eSharedMemoryLap r3eSharedMemoryLap)
    {
        if(this.currentSession == null)
        {
            return;
        }

        var car = this.r3eCarInfoProvider.FindByGameId(r3eSharedMemoryLap.ModelId);
        var r3eMonitorLap = new R3eMonitorLap
        {
            CarClass = car?.Class ?? string.Empty,
            CarManufacturer = car?.Manufacturer ?? string.Empty,
            CarModelName = car?.Name ?? $"R3E car {r3eSharedMemoryLap.ModelId}",
            CarNumber = r3eSharedMemoryLap.CarNumber,
            CompletedLaps = r3eSharedMemoryLap.CompletedLaps,
            DriverName = r3eSharedMemoryLap.DriverName,
            FinishStatus = r3eSharedMemoryLap.FinishStatus,
            IsLocalPlayer = r3eSharedMemoryLap.IsLocalPlayer,
            IsSimulatedFinish = IsSimulatedFinish(r3eSharedMemoryLap),
            IsValid = IsValidTimedLap(r3eSharedMemoryLap),
            LapTimeMs = r3eSharedMemoryLap.LapTimeMs,
            LayoutName = this.currentSession.LayoutName,
            LiveryId = r3eSharedMemoryLap.LiveryId,
            ModelId = r3eSharedMemoryLap.ModelId,
            Place = r3eSharedMemoryLap.Place,
            PlaceClass = r3eSharedMemoryLap.PlaceClass,
            Sector1Ms = r3eSharedMemoryLap.Sector1Ms,
            Sector2Ms = r3eSharedMemoryLap.Sector2Ms,
            Sector3Ms = r3eSharedMemoryLap.Sector3Ms,
            SessionId = this.currentSession.SessionId,
            SlotId = r3eSharedMemoryLap.SlotId,
            TeamId = r3eSharedMemoryLap.TeamId,
            TrackName = this.currentSession.TrackName,
            UserId = r3eSharedMemoryLap.UserId
        };

        this.LogMessage(LoggingLevel.Information, $"Lap Completed: {r3eMonitorLap}");
        this.lapCompletedSubject.OnNext(r3eMonitorLap);
    }

    private void OnNextSessionEnded(R3eSharedMemorySession r3eSharedMemorySession)
    {
        if(this.currentSession == null)
        {
            return;
        }

        this.currentSession.IsRunning = false;
        this.LogMessage(LoggingLevel.Information, $"Session Completed: {this.currentSession}");
        this.sessionCompletedSubject.OnNext(this.currentSession);
        this.currentSession = null;
    }

    private void OnNextSessionStarted(R3eSharedMemorySession r3eSharedMemorySession)
    {
        var trackAndLayout = this.r3eTrackInfoProvider.FindByLayoutGameId(r3eSharedMemorySession.LayoutId);
        this.currentSession = new R3eMonitorSession
        {
            Country = trackAndLayout?.Track.Country,
            CountryCode = trackAndLayout?.Track.CountryCode,
            EventIndex = r3eSharedMemorySession.EventIndex,
            GameMode = r3eSharedMemorySession.GameMode,
            IsOnline = IsOnline(r3eSharedMemorySession.GameMode),
            IsRunning = true,
            LayoutId = r3eSharedMemorySession.LayoutId,
            LayoutLengthMeters = r3eSharedMemorySession.LayoutLengthMeters,
            LayoutName = trackAndLayout?.Layout.Name ?? r3eSharedMemorySession.LayoutName,
            NumberOfCars = r3eSharedMemorySession.NumberOfCars,
            NumberOfLaps = r3eSharedMemorySession.NumberOfLaps,
            PlayerName = r3eSharedMemorySession.PlayerName,
            PlayerUserId = r3eSharedMemorySession.PlayerUserId,
            SessionId = r3eSharedMemorySession.SessionId,
            SessionIteration = r3eSharedMemorySession.SessionIteration,
            SessionLengthFormat = r3eSharedMemorySession.SessionLengthFormat,
            SessionTimeDurationSeconds = r3eSharedMemorySession.SessionTimeDurationSeconds,
            SessionType = r3eSharedMemorySession.SessionType,
            TrackId = r3eSharedMemorySession.TrackId,
            TrackName = trackAndLayout?.Track.Name ?? r3eSharedMemorySession.TrackName
        };

        this.LogMessage(LoggingLevel.Information, $"Session Started: {this.currentSession}");
        this.sessionStartedSubject.OnNext(this.currentSession);
    }

    private void OnNextTelemetryFrame(R3eTelemetryFrame r3eTelemetryFrame)
    {
        this.telemetrySubject.OnNext(r3eTelemetryFrame);
    }
}
