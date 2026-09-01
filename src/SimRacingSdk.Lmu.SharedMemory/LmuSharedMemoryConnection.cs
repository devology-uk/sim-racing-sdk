using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Core.Services;
using SimRacingSdk.Lmu.SharedMemory.Abstractions;
using SimRacingSdk.Lmu.SharedMemory.Messages;
using SimRacingSdk.Lmu.SharedMemory.Models;

namespace SimRacingSdk.Lmu.SharedMemory;

// Reads LMU's native "LMU_Data" shared memory (Studio 397's own SharedMemoryInterface.hpp) directly - no plugin,
// no CustomPluginVariables.JSON, no DMA. The player's Telemetry entry comes straight from PlayerVehicleIdx; the
// matching Scoring entry is found via IsPlayer - both come from the same read, so there's no cross-source
// lap/session-key linking problem to guard against.
public class LmuSharedMemoryConnection : ILmuSharedMemoryConnection
{
    private readonly Subject<LmuSharedMemoryLap> lapsSubject = new();
    private readonly LogMessageBroker logMessageBroker = new(nameof(LmuSharedMemoryConnection));
    private readonly Subject<LmuSharedMemorySession> sessionEndedSubject = new();
    private readonly Subject<LmuSharedMemorySession> sessionStartedSubject = new();
    private readonly ILmuSharedMemoryProvider sharedMemoryProvider;
    private readonly Subject<LmuTelemetryFrame> telemetrySubject = new();

    private LmuSharedMemorySession? currentSession;
    private string? lastLoggedWaitReason;
    private LmuVehicleScoring? lastPlayerScoring;
    private LmuScoringInfo? lastScoringInfo;
    private CompositeDisposable? subscriptionSink;
    private IDisposable? updateSubscription;

    public LmuSharedMemoryConnection(ILmuSharedMemoryProvider sharedMemoryProvider)
    {
        this.sharedMemoryProvider = sharedMemoryProvider;
    }

    public IObservable<LmuSharedMemoryLap> Laps => this.lapsSubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessageBroker.Messages;
    public IObservable<LmuSharedMemorySession> SessionEnded => this.sessionEndedSubject.AsObservable();
    public IObservable<LmuSharedMemorySession> SessionStarted => this.sessionStartedSubject.AsObservable();
    public IObservable<LmuTelemetryFrame> Telemetry => this.telemetrySubject.AsObservable();

    public void Dispose()
    {
        this.Stop();
        GC.SuppressFinalize(this);
    }

    public void Start(double updateIntervalMs = 20)
    {
        this.Stop();
        this.LogMessage(LoggingLevel.Information, "Starting LMU Shared Memory connection...");
        this.subscriptionSink = new CompositeDisposable
        {
            this.sharedMemoryProvider.LogMessages.Subscribe(this.logMessageBroker.Relay)
        };
        this.updateSubscription = Observable.Interval(TimeSpan.FromMilliseconds(updateIntervalMs))
                                            .Subscribe(this.OnNextUpdate, this.OnError);
    }

    public void Stop()
    {
        this.EndCurrentSession();
        this.updateSubscription?.Dispose();
        this.updateSubscription = null;
        this.subscriptionSink?.Dispose();
        this.subscriptionSink = null;
        this.lastScoringInfo = null;
        this.lastPlayerScoring = null;
    }

    private void BeginNewSession(LmuScoringInfo scoringInfo)
    {
        this.EndCurrentSession();

        this.lastPlayerScoring = null;
        this.currentSession = new LmuSharedMemorySession(scoringInfo);
        this.sessionStartedSubject.OnNext(this.currentSession);
        this.LogMessage(LoggingLevel.Information, $"Session Started: {this.currentSession}");
    }

    private void EndCurrentSession()
    {
        if(this.currentSession is null)
        {
            return;
        }

        this.currentSession.IsRunning = false;
        this.sessionEndedSubject.OnNext(this.currentSession);
        this.LogMessage(LoggingLevel.Information, $"Session Ended: {this.currentSession}");
        this.currentSession = null;
    }

    private void LogMessage(LoggingLevel level, string content)
    {
        this.logMessageBroker.Log(level, content);
    }

    private void LogWaitReasonOnce(string reason)
    {
        if(this.lastLoggedWaitReason == reason)
        {
            return;
        }

        this.lastLoggedWaitReason = reason;
        this.LogMessage(LoggingLevel.Information, reason);
    }

    private void OnError(Exception exception)
    {
        this.LogMessage(LoggingLevel.Error, exception.Message);
    }

    private void OnNextUpdate(long tick)
    {
        var data = this.sharedMemoryProvider.Read();
        if(data is null)
        {
            return;
        }

        var scoringInfo = data.Value.Scoring.ScoringInfo;
        this.UpdateSession(scoringInfo);
        this.lastScoringInfo = scoringInfo;

        var telemetry = data.Value.Telemetry;
        if(!telemetry.PlayerHasVehicle || telemetry.PlayerVehicleIdx >= telemetry.TelemInfo.Length)
        {
            this.LogWaitReasonOnce(
                "Connected to LMU shared memory, waiting for a player vehicle (join a session/get in a car)...");
            return;
        }

        var playerTelemetry = telemetry.TelemInfo[telemetry.PlayerVehicleIdx];

        var scoring = data.Value.Scoring;
        var numScoredVehicles =
            Math.Clamp(scoring.ScoringInfo.NumVehicles, 0, LmuSharedMemoryScoringData.MaxVehicles);
        var playerScoringIndex =
            Array.FindIndex(scoring.VehScoringInfo, 0, numScoredVehicles, vehicle => vehicle.IsPlayer);
        if(playerScoringIndex < 0)
        {
            this.LogWaitReasonOnce(
                "Connected to LMU shared memory, have player telemetry but no matching scoring entry yet...");
            return;
        }

        this.lastLoggedWaitReason = null;
        var playerScoring = scoring.VehScoringInfo[playerScoringIndex];
        this.UpdateLap(playerScoring, playerTelemetry);
        this.lastPlayerScoring = playerScoring;

        var frame = new LmuTelemetryFrame(playerTelemetry, playerScoring);
        this.telemetrySubject.OnNext(frame);
    }

    private void UpdateLap(LmuVehicleScoring playerScoring, LmuVehicleTelemetry playerTelemetry)
    {
        if(this.currentSession is null || this.lastPlayerScoring is null)
        {
            return;
        }

        if(playerScoring.TotalLaps <= this.lastPlayerScoring.Value.TotalLaps)
        {
            return;
        }

        var lap = new LmuSharedMemoryLap(playerScoring, playerTelemetry, this.currentSession.SessionId,
            this.currentSession.TrackName);
        this.lapsSubject.OnNext(lap);
        this.LogMessage(LoggingLevel.Information, $"Lap Completed: {lap}");
    }

    // InRealtime alone isn't enough to detect session start/end: it also goes false for a mid-session garage visit
    // (e.g. pitting to repair damage) and, as confirmed by a real rig log, flickers unpredictably for tens of
    // seconds on the post-session results screen too - neither should split or re-split a session. LMU only ever
    // allows one instance of each session type per event (confirmed by Mike against the game's own behaviour), so
    // a change in Session - which distinguishes Practice/Qualify/Race - is a sufficient and sole signal for a
    // genuine new session; once currentSession exists, nothing else here can end or restart it mid-session.
    private void UpdateSession(LmuScoringInfo scoringInfo)
    {
        if(this.lastScoringInfo is null)
        {
            return;
        }

        var previous = this.lastScoringInfo.Value;

        if(this.currentSession is not null)
        {
            if(previous.Session != scoringInfo.Session)
            {
                this.BeginNewSession(scoringInfo);
            }

            return;
        }

        if(scoringInfo.InRealtime)
        {
            this.BeginNewSession(scoringInfo);
        }
    }
}
