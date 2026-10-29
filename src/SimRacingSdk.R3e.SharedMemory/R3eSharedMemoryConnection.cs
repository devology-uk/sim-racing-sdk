using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Core.Services;
using SimRacingSdk.R3e.SharedMemory.Abstractions;
using SimRacingSdk.R3e.SharedMemory.Messages;
using SimRacingSdk.R3e.SharedMemory.Models;
using SimRacingSdk.R3e.SharedMemory.Services;

namespace SimRacingSdk.R3e.SharedMemory;

public class R3eSharedMemoryConnection : IR3eSharedMemoryConnection
{
    private readonly R3eLapTracker lapTracker = new();
    private readonly Subject<R3eSharedMemoryLap> lapsSubject = new();
    private readonly LogMessageBroker logMessageBroker = new(nameof(R3eSharedMemoryConnection));
    private readonly Subject<R3eSharedMemorySession> sessionEndedSubject = new();
    private readonly Subject<R3eSharedMemorySession> sessionStartedSubject = new();
    private readonly IR3eSharedMemoryProvider sharedMemoryProvider;
    private readonly Subject<R3eTelemetryFrame> telemetrySubject = new();

    private R3eSharedMemorySession? currentSession;
    private R3eSessionKey? currentSessionKey;
    private string? lastLoggedWaitReason;
    private int? lastPlayerCompletedLaps;
    private CompositeDisposable? subscriptionSink;
    private IDisposable? updateSubscription;

    public R3eSharedMemoryConnection(IR3eSharedMemoryProvider sharedMemoryProvider)
    {
        this.sharedMemoryProvider = sharedMemoryProvider;
    }

    public IObservable<R3eSharedMemoryLap> Laps => this.lapsSubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessageBroker.Messages;
    public IObservable<R3eSharedMemorySession> SessionEnded => this.sessionEndedSubject.AsObservable();
    public IObservable<R3eSharedMemorySession> SessionStarted => this.sessionStartedSubject.AsObservable();
    public IObservable<R3eTelemetryFrame> Telemetry => this.telemetrySubject.AsObservable();

    public void Dispose()
    {
        this.Stop();
        GC.SuppressFinalize(this);
    }

    public void Start(double updateIntervalMs = 20)
    {
        this.Stop();
        this.LogMessage(LoggingLevel.Information, "Starting R3E Shared Memory connection...");
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
    }

    private static bool IsOnTrack(R3eShared data)
    {
        return data.GameInMenus == 0
               && data.GameInReplay == 0
               && data.GamePaused == 0
               && data.TrackId >= 0
               && data.SessionType >= 0;
    }

    private void BeginNewSession(R3eShared data)
    {
        this.EndCurrentSession();

        this.lapTracker.Reset();
        this.lastPlayerCompletedLaps = null;
        this.currentSessionKey = R3eSessionKey.From(data);
        this.currentSession = new R3eSharedMemorySession(data);
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
        this.currentSessionKey = null;
    }

    // The same session key with the player's lap count going backwards is a restart of that session.
    private bool HasSessionRestarted(R3eShared data)
    {
        return this.lastPlayerCompletedLaps is { } lastCompletedLaps
               && data.CompletedLaps >= 0
               && data.CompletedLaps < lastCompletedLaps;
    }

    private bool IsNewSession(R3eShared data)
    {
        return this.currentSession is null
               || this.currentSessionKey != R3eSessionKey.From(data)
               || this.HasSessionRestarted(data);
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
        var read = this.sharedMemoryProvider.Read();
        if(read is not { } data)
        {
            return;
        }

        if(!IsOnTrack(data))
        {
            this.LogWaitReasonOnce("Connected to R3E shared memory, waiting for the player to be on track...");
            return;
        }

        this.lastLoggedWaitReason = null;
        this.UpdateSession(data);
        this.PublishLaps(data);
        this.PublishTelemetry(data);
    }

    private void PublishLaps(R3eShared data)
    {
        foreach(var lap in this.lapTracker.Update(data, this.currentSession!.SessionId))
        {
            this.lapsSubject.OnNext(lap);
            var playerLapTime = lap.IsLocalPlayer ? $", LapTimePreviousSelf = {data.LapTimePreviousSelf}" : "";
            this.LogMessage(LoggingLevel.Information, $"Lap Completed: {lap}{playerLapTime}");
        }
    }

    private void PublishTelemetry(R3eShared data)
    {
        if(data.VehicleInfo.SlotId < 0)
        {
            return;
        }

        this.telemetrySubject.OnNext(new R3eTelemetryFrame(data, this.currentSession!.SessionId));
    }

    private void UpdateSession(R3eShared data)
    {
        if(this.IsNewSession(data))
        {
            this.BeginNewSession(data);
        }

        this.lastPlayerCompletedLaps = data.CompletedLaps;
    }
}
