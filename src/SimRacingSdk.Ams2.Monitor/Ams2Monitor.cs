using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Ams2.Core.Abstractions;
using SimRacingSdk.Ams2.Core.Enums;
using SimRacingSdk.Ams2.Monitor.Abstractions;
using SimRacingSdk.Ams2.Monitor.Messages;
using SimRacingSdk.Ams2.SharedMemory.Abstractions;
using SimRacingSdk.Ams2.SharedMemory.Enums;
using SimRacingSdk.Ams2.SharedMemory.Models;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ams2.Monitor;

public class Ams2Monitor : IAms2Monitor
{
    private readonly IAms2NationalityInfoProvider ams2NationalityInfoProvider;
    private readonly IAms2SharedMemoryConnectionFactory ams2SharedMemoryConnectionFactory;
    private readonly IAms2CarInfoProvider amsCarInfoProvider;
    private readonly Subject<Ams2Lap> completedLapsSubject = new();
    private readonly Subject<LogMessage> logMessagesSubject = new();
    private readonly Subject<Ams2MonitorParticipant> participantUpdatesSubject = new();
    private readonly Subject<Ams2MonitorSession> sessionCompletedSubject = new();
    private readonly Subject<Ams2MonitorSession> sessionStartedSubject = new();
    private readonly Subject<Ams2TelemetryFrame> telemetrySubject = new();

    private IAms2SharedMemoryConnection? ams2SharedMemoryConnection;
    private Ams2GameStatus? currentGameStatus;
    private Ams2MonitorSession? currentSession;
    private int lapCount = 0;
    private int playerLapCount;
    private CompositeDisposable? subscriptionSink;

    public Ams2Monitor(IAms2SharedMemoryConnectionFactory ams2SharedMemoryConnectionFactory,
        IAms2CarInfoProvider ams2CarInfoProvider,
        IAms2NationalityInfoProvider ams2NationalityInfoProvider)
    {
        this.ams2SharedMemoryConnectionFactory = ams2SharedMemoryConnectionFactory;
        this.amsCarInfoProvider = ams2CarInfoProvider;
        this.ams2NationalityInfoProvider = ams2NationalityInfoProvider;
    }

    public IObservable<Ams2Lap> CompletedLaps => this.completedLapsSubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();
    public IObservable<Ams2MonitorParticipant> ParticipantUpdates =>
        this.participantUpdatesSubject.AsObservable();
    public IObservable<Ams2MonitorSession> SessionCompleted => this.sessionCompletedSubject.AsObservable();
    public IObservable<Ams2MonitorSession> SessionStarted => this.sessionStartedSubject.AsObservable();
    public IObservable<Ams2TelemetryFrame> Telemetry => this.telemetrySubject.AsObservable();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Start()
    {
        this.LogMessage(LoggingLevel.Information, "Starting AMS2 Monitor...");

        this.PrepareAndStartNewAccConnection();
    }

    public void Stop()
    {
        this.subscriptionSink?.Dispose();
        this.ams2SharedMemoryConnection?.Dispose();
        this.ams2SharedMemoryConnection = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!disposing)
        {
            return;
        }

        this.Stop();
    }

    private void EndCurrentSession(Ams2GameStatus ams2GameStatus)
    {
        if(this.currentSession == null)
        {
            return;
        }

        this.currentSession.EndTime = DateTime.UtcNow;
        this.currentSession.TotalLapCount = this.lapCount;
        this.currentSession.PlayerLapCount = this.playerLapCount;

        this.sessionCompletedSubject.OnNext(this.currentSession);
        this.LogMessage(LoggingLevel.Information, $"Session Completed: {this.currentSession}");
    }

    private bool HasChangedToActiveSessionType(Ams2GameStatus ams2GameStatus)
    {
        if(this.currentGameStatus == null)
        {
            return false;
        }

        if(this.currentGameStatus.SessionState != Ams2SessionState.Practice
           && ams2GameStatus.SessionState == Ams2SessionState.Practice)
        {
            return true;
        }

        if(this.currentGameStatus.SessionState != Ams2SessionState.Test
           && ams2GameStatus.SessionState == Ams2SessionState.Test)
        {
            return true;
        }

        if(this.currentGameStatus.SessionState != Ams2SessionState.Qualify
           && ams2GameStatus.SessionState == Ams2SessionState.Qualify)
        {
            return true;
        }

        if(this.currentGameStatus.SessionState != Ams2SessionState.Race
           && ams2GameStatus.SessionState == Ams2SessionState.Race)
        {
            return true;
        }

        return this.currentGameStatus.SessionState != Ams2SessionState.TimeAttack
               && ams2GameStatus.SessionState == Ams2SessionState.TimeAttack;
    }

    private void LogMessage(LoggingLevel level, string content)
    {
        this.logMessagesSubject.OnNext(new LogMessage(level, content, nameof(Ams2Monitor)));
    }

    private void OnNextCompletedLap(Ams2Lap ams2Lap)
    {
        this.lapCount++;
        if(ams2Lap.IsPlayerLap)
        {
            this.playerLapCount++;
        }

        this.LogMessage(LoggingLevel.Information, $"Lap Completed: {ams2Lap}");
        this.completedLapsSubject.OnNext(ams2Lap);
    }

    private void OnNextGameStatusUpdate(Ams2GameStatus ams2GameStatus)
    {
        if(this.currentGameStatus == null)
        {
            this.currentGameStatus = ams2GameStatus;
            return;
        }

        if(this.HasChangedToActiveSessionType(ams2GameStatus))
        {
            this.EndCurrentSession(ams2GameStatus);
            this.StartNewSession(ams2GameStatus);
        }

        if(this.currentGameStatus.SessionState != Ams2SessionState.Invalid
           && ams2GameStatus.SessionState == Ams2SessionState.Invalid)
        {
            this.EndCurrentSession(ams2GameStatus);
        }

        this.currentGameStatus = ams2GameStatus;
    }

    private void OnNextParticipantUpdate(Ams2Participant ams2Participant)
    {
        if(this.currentSession == null)
        {
            return;
        }

        var carInfo = this.amsCarInfoProvider.FindByModel(ams2Participant.CarName);
        var countryCode =
            this.ams2NationalityInfoProvider.GetCountryCode((Ams2Nationality)ams2Participant.Nationality);
        var ams2MonitorParticipant = new Ams2MonitorParticipant(ams2Participant)
        {
            CarManufacturer = carInfo?.Manufacturer,
            CountryCode = countryCode
        };

        this.participantUpdatesSubject.OnNext(ams2MonitorParticipant);
        this.LogMessage(LoggingLevel.Information, $"Participant Update: {ams2MonitorParticipant}");
    }

    private void OnNextTelemetryFrame(Ams2TelemetryFrame ams2TelemetryFrame)
    {
        this.telemetrySubject.OnNext(ams2TelemetryFrame);
    }

    private void PrepareAndStartNewAccConnection()
    {
        this.ams2SharedMemoryConnection = this.ams2SharedMemoryConnectionFactory.Create();
        this.subscriptionSink = new CompositeDisposable
        {
            this.ams2SharedMemoryConnection.CompletedLaps.Subscribe(this.OnNextCompletedLap),
            this.ams2SharedMemoryConnection.GameStatusUpdates.Subscribe(this.OnNextGameStatusUpdate),
            this.ams2SharedMemoryConnection.LogMessages.Subscribe(m => this.logMessagesSubject.OnNext(m)),
            this.ams2SharedMemoryConnection.ParticipantUpdates.Subscribe(this.OnNextParticipantUpdate),
            this.ams2SharedMemoryConnection.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };

        this.ams2SharedMemoryConnection.Start();
    }

    private void StartNewSession(Ams2GameStatus ams2GameStatus)
    {
        this.currentSession = new Ams2MonitorSession
        {
            ParticipantCount = ams2GameStatus.ParticipantCount,
            ScheduledDurationMs = ams2GameStatus.SessionDuration,
            ScheduledLaps = ams2GameStatus.LapsInEvent,
            SessionType = ams2GameStatus.SessionState.ToSessionType(),
            StartTime = DateTime.UtcNow,
            TrackLayout = ams2GameStatus.TrackLayout,
            TrackLocation = ams2GameStatus.TrackLocation
        };
        this.lapCount = 0;
        this.sessionStartedSubject.OnNext(this.currentSession);
        this.LogMessage(LoggingLevel.Information, $"Session Started: {this.currentSession}");
    }
}