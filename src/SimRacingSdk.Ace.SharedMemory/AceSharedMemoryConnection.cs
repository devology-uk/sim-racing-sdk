using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Ace.SharedMemory.Abstractions;
using SimRacingSdk.Ace.SharedMemory.Enums;
using SimRacingSdk.Ace.SharedMemory.Models;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ace.SharedMemory;

// Session identity here is derived from AceStaticData (EventId/SessionId/Session/IsOnline),
// which the PDF documents as written once per session - unlike Acc, where the equivalent
// identity fields (SessionIndex/SessionType) live on the per-frame graphics page. Lap
// completion is detected from GraphicsData.TotalLapCount incrementing rather than Acc's
// sector-index heuristic, because Evo's shared memory doesn't expose sector data.
public class AceSharedMemoryConnection : IAceSharedMemoryConnection
{
    private readonly Subject<AceAppStatusChange> appStatusChangesSubject = new();
    private readonly Subject<AceFlagState> flagStateSubject = new();
    private readonly Subject<LogMessage> logMessagesSubject = new();
    private readonly Subject<AceSharedMemoryLap> newLapSubject = new();
    private readonly Subject<AceSharedMemorySession> sessionEndedSubject = new();
    private readonly Subject<AceSharedMemorySession> sessionStartedSubject = new();
    private readonly IAceSharedMemoryProvider sharedMemoryProvider;
    private readonly Subject<AceTelemetryFrame> telemetrySubject = new();

    private AceSharedMemorySession? currentSession;
    private AceFlagState? lastFlagState;
    private AceGraphicsData? lastGraphicsData;
    private AceStaticData? lastStaticData;
    private IDisposable? updateSubscription;

    public AceSharedMemoryConnection(IAceSharedMemoryProvider sharedMemoryProvider)
    {
        this.sharedMemoryProvider = sharedMemoryProvider;
    }

    public IObservable<AceAppStatusChange> AppStatusChanges => this.appStatusChangesSubject.AsObservable();
    public IObservable<AceFlagState> FlagState => this.flagStateSubject.AsObservable();
    public IObservable<AceSharedMemoryLap> Laps => this.newLapSubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();
    public IObservable<AceSharedMemorySession> SessionEnded => this.sessionEndedSubject.AsObservable();
    public IObservable<AceSharedMemorySession> SessionStarted => this.sessionStartedSubject.AsObservable();
    public IObservable<AceTelemetryFrame> Telemetry => this.telemetrySubject.AsObservable();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Start(double updateIntervalMs = 100)
    {
        this.updateSubscription = Observable.Interval(TimeSpan.FromMilliseconds(updateIntervalMs))
                                            .Subscribe(this.OnNextUpdate);
    }

    public void Stop()
    {
        this.EndCurrentSession();
        this.updateSubscription?.Dispose();
        this.updateSubscription = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!disposing)
        {
            return;
        }

        this.Stop();
    }

    private void BeginNewSession(AceGraphicsData graphicsData, AceStaticData staticData)
    {
        this.EndCurrentSession();

        this.currentSession = new AceSharedMemorySession(staticData, graphicsData);
        this.sessionStartedSubject.OnNext(this.currentSession);
    }

    private void EndCurrentSession()
    {
        if(this.currentSession == null)
        {
            return;
        }

        this.currentSession.IsRunning = false;
        this.sessionEndedSubject.OnNext(this.currentSession);
        this.currentSession = null;
    }

    private void LogGraphicsData(AceGraphicsData graphicsData)
    {
        this.LogMessage(LoggingLevel.Debug, graphicsData.ToString());
    }

    private void LogMessage(LoggingLevel loggingLevel, string content)
    {
        this.logMessagesSubject.OnNext(new LogMessage(loggingLevel,
            content,
            nameof(AceSharedMemoryConnection)));
    }

    private void LogStaticData(AceStaticData staticData)
    {
        if(this.lastStaticData == null || !this.lastStaticData.IsSameSession(staticData))
        {
            this.LogMessage(LoggingLevel.Debug, staticData.ToString());
        }
    }

    private void OnNextUpdate(long index)
    {
        var staticData = this.sharedMemoryProvider.ReadStaticData();
        this.LogStaticData(staticData);

        var graphicsData = this.sharedMemoryProvider.ReadGraphicsData();
        this.LogGraphicsData(graphicsData);

        if(this.lastStaticData == null)
        {
            this.lastStaticData = staticData;
            this.lastGraphicsData = graphicsData;
            return;
        }

        this.UpdateSession(graphicsData, staticData);
        this.UpdateFlagState(graphicsData);
        this.UpdateActiveLap(graphicsData, staticData);

        this.lastStaticData = staticData;
        this.lastGraphicsData = graphicsData;

        if(this.currentSession == null)
        {
            return;
        }

        var physicsData = this.sharedMemoryProvider.ReadPhysicsData();
        if(physicsData.IsEmpty)
        {
            return;
        }

        this.LogMessage(LoggingLevel.Debug, physicsData.ToString());
        this.telemetrySubject.OnNext(new AceTelemetryFrame(staticData, graphicsData, physicsData));
    }

    private void UpdateActiveLap(AceGraphicsData graphicsData, AceStaticData staticData)
    {
        if(this.lastGraphicsData == null || graphicsData.TotalLapCount <= this.lastGraphicsData.TotalLapCount)
        {
            return;
        }

        var aceSharedMemoryLap = new AceSharedMemoryLap(staticData, graphicsData);
        this.newLapSubject.OnNext(aceSharedMemoryLap);
        this.LogMessage(LoggingLevel.Debug, aceSharedMemoryLap.ToString());
    }

    private void UpdateFlagState(AceGraphicsData graphicsData)
    {
        if(this.currentSession == null)
        {
            return;
        }

        var flagState = new AceFlagState(graphicsData.Flag, graphicsData.GlobalFlag);

        if(this.lastFlagState != null && flagState == this.lastFlagState)
        {
            return;
        }

        this.lastFlagState = flagState;
        this.flagStateSubject.OnNext(flagState);
    }

    private void UpdateSession(AceGraphicsData graphicsData, AceStaticData staticData)
    {
        if(this.lastStaticData!.IsSameSession(staticData))
        {
            if(this.currentSession != null
               && graphicsData.Status == AceStatus.Live
               && this.lastGraphicsData != null
               && graphicsData.SessionState.TimeLeftMs > this.lastGraphicsData.SessionState.TimeLeftMs + 60000)
            {
                this.BeginNewSession(graphicsData, staticData);
            }

            return;
        }

        switch(graphicsData.Status)
        {
            case AceStatus.Live:
                this.BeginNewSession(graphicsData, staticData);
                break;
            case AceStatus.Off:
                this.EndCurrentSession();
                break;
        }

        if(this.lastGraphicsData != null && this.lastGraphicsData.Status != graphicsData.Status)
        {
            this.appStatusChangesSubject.OnNext(new AceAppStatusChange(this.lastGraphicsData.Status,
                graphicsData.Status));
        }
    }
}
