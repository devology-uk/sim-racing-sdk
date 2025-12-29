using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.SharedMemory.Enums;
using SimRacingSdk.Acc.SharedMemory.Models;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Acc.SharedMemory;

public class AccSharedMemoryConnection : IAccSharedMemoryConnection
{
    private readonly Subject<AccAppStatusChange> appStatusChangesSubject = new();
    private readonly Subject<AccFlagState> flagStateSubject = new();
    private readonly Subject<LogMessage> logMessagesSubject = new();
    private readonly Subject<AccSharedMemoryLap> newLapSubject = new();
    private readonly Subject<AccSharedMemorySession> sessionEndedSubject = new();
    private readonly Subject<AccSharedMemorySession> sessionStartedSubject = new();
    private readonly IAccSharedMemoryProvider sharedMemoryProvider;
    private readonly Subject<AccTelemetryFrame> telemetrySubject = new();

    private int actualSectorIndex;
    private AccSharedMemorySession? currentSession;
    private AccFlagState? lastFlagState;
    private GraphicsData? lastGraphicsData;
    private StaticData? lastStaticData;
    private IDisposable? updateSubscription;

    public AccSharedMemoryConnection(IAccSharedMemoryProvider sharedMemoryProvider)
    {
        this.sharedMemoryProvider = sharedMemoryProvider;
    }

    public IObservable<AccAppStatusChange> AppStatusChanges => this.appStatusChangesSubject.AsObservable();
    public IObservable<AccFlagState> FlagState => this.flagStateSubject.AsObservable();
    public IObservable<AccSharedMemoryLap> Laps => this.newLapSubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();
    public IObservable<AccSharedMemorySession> SessionEnded => this.sessionEndedSubject.AsObservable();
    public IObservable<AccSharedMemorySession> SessionStarted => this.sessionStartedSubject.AsObservable();
    public IObservable<AccTelemetryFrame> Telemetry => this.telemetrySubject.AsObservable();

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

    private void BeginNewSession(GraphicsData graphicsData, StaticData staticData)
    {
        this.EndCurrentSession();

        this.currentSession = new AccSharedMemorySession(staticData, graphicsData);
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

    private bool HasStartedOutLap(GraphicsData graphicsData)
    {
        return this.lastGraphicsData is
        {
            IsInPits: true
        } && graphicsData.IsInPitLane;
    }

    private bool HasStartedPaceLap(GraphicsData graphicsData)
    {
        return this.lastGraphicsData != null && this.lastStaticData != null
                                             && this.lastGraphicsData.CurrentSectorIndex
                                             == this.lastStaticData.SectorCount - 1
                                             && graphicsData.CurrentSectorIndex == 0;
    }

    private void LogGraphicsData(GraphicsData graphicsData)
    {
        this.LogMessage(LoggingLevel.Debug, graphicsData.ToString());
    }

    private void LogMessage(LoggingLevel loggingLevel, string content)
    {
        this.logMessagesSubject.OnNext(new LogMessage(loggingLevel,
            content,
            nameof(AccSharedMemoryConnection)));
    }

    private void LogStaticData(StaticData staticData)
    {
        if(this.lastStaticData?.ComparesTo(staticData) is false)
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

        var physicsData = this.sharedMemoryProvider.ReadPhysicsData();
        if(physicsData.IsEmpty)
        {
            return;
        }

        this.LogMessage(LoggingLevel.Debug, physicsData.ToString());
        this.telemetrySubject.OnNext(new AccTelemetryFrame(staticData,
            graphicsData,
            physicsData,
            this.actualSectorIndex));
    }

    private void UpdateActiveLap(GraphicsData graphicsData, StaticData staticData)
    {
        var hasStartedOutLap = this.HasStartedOutLap(graphicsData);
        var hasStartedPaceLap = this.HasStartedPaceLap(graphicsData);
        this.actualSectorIndex = graphicsData.CurrentSectorIndex;

        if(!hasStartedOutLap && !hasStartedPaceLap)
        {
            return;
        }

        this.actualSectorIndex = 0;
        var accSharedMemoryLap = new AccSharedMemoryLap(staticData, graphicsData);
        this.newLapSubject.OnNext(accSharedMemoryLap);
        this.LogMessage(LoggingLevel.Debug, accSharedMemoryLap.ToString());

    }

    private void UpdateFlagState(GraphicsData graphicsData)
    {
        if(this.currentSession == null)
        {
            return;
        }

        var flagState = new AccFlagState(graphicsData.IsWhiteFlagActive,
            graphicsData.IsYellowFlagActive,
            graphicsData.IsYellowFlagActiveInSector1,
            graphicsData.IsYellowFlagActiveInSector2,
            graphicsData.IsYellowFlagActiveInSector3);

        if(this.lastFlagState != null && flagState == this.lastFlagState)
        {
            return;
        }

        this.lastFlagState = flagState;
        this.flagStateSubject.OnNext(flagState);
    }

    private void UpdateSession(GraphicsData graphicsData, StaticData staticData)
    {
        if(this.lastGraphicsData!.IsSameSession(graphicsData))
        {
            return;
        }

        switch(graphicsData.Status)
        {
            case AccAppStatus.Live:
                this.BeginNewSession(graphicsData, staticData);
                return;
            case AccAppStatus.Off:
                this.EndCurrentSession();
                break;
        }
    }
}