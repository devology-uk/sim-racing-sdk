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
    private readonly Subject<AccSharedMemoryConnectedState> connectedStateSubject = new();
    private readonly Subject<AccFlagState> flagStateSubject = new();
    private readonly Subject<LogMessage> logMessagesSubject = new();
    private readonly Subject<AccSharedMemoryLap> newLapSubject = new();
    private readonly Subject<AccSharedMemorySession> sessionEndedSubject = new();
    private readonly Subject<AccSharedMemorySession> sessionStartedSubject = new();
    private readonly IAccSharedMemoryProvider sharedMemoryProvider;
    private readonly Subject<AccTelemetryFrame> telemetrySubject = new();

    private int actualSectorIndex;
    private AccSharedMemorySession? currentSession;
    private StaticData? currentStaticData;
    private bool isConnected;
    private bool isOnActiveLap;
    private AccFlagState? lastFlagState;
    private GraphicsData? lastGraphicsData;
    private IDisposable? updateSubscription;
    private AccAppStatus currentAppStatus;

    public AccSharedMemoryConnection(IAccSharedMemoryProvider sharedMemoryProvider)
    {
        this.sharedMemoryProvider = sharedMemoryProvider;
    }

    public IObservable<AccAppStatusChange> AppStatusChanges => this.appStatusChangesSubject.AsObservable();
    public IObservable<AccSharedMemoryConnectedState> ConnectedState => this.connectedStateSubject.AsObservable();
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
                                            .Subscribe(this.OnNextUpdate,
                                                e => this.telemetrySubject.OnError(e),
                                                () => this.telemetrySubject.OnCompleted());
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
        return this.lastGraphicsData != null && this.currentStaticData != null
                                             && this.lastGraphicsData.CurrentSectorIndex
                                             == this.currentStaticData.SectorCount - 1
                                             && graphicsData.CurrentSectorIndex == 0;
    }

    private void LogMessage(LoggingLevel loggingLevel, string content)
    {
        this.logMessagesSubject.OnNext(new LogMessage(loggingLevel,
            content,
            nameof(AccSharedMemoryConnection)));
    }

    private void OnNextUpdate(long index)
    {
        var staticData = this.sharedMemoryProvider.ReadStaticData();
        this.UpdateConnectionState(staticData.IsConnected);

        if(!this.isConnected)
        {
            return;
        }

        this.LogMessage(LoggingLevel.Debug, staticData.ToString());

        this.currentStaticData = staticData!;

        var graphicsData = this.sharedMemoryProvider.ReadGraphicsData();
        if(graphicsData.IsEmpty)
        {
            return;
        }

        this.LogMessage(LoggingLevel.Debug, graphicsData.ToString());

        this.UpdateSession(graphicsData, staticData);
        this.UpdateAppStatus(graphicsData);
        this.UpdateFlagState(graphicsData);
        if(!this.UpdateActiveLap(graphicsData, staticData))
        {
            return;
        }

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

    private bool UpdateActiveLap(GraphicsData graphicsData, StaticData staticData)
    {
        var hasStartedOutLap = this.HasStartedOutLap(graphicsData);
        var hasStartedPaceLap = this.HasStartedPaceLap(graphicsData);
        this.actualSectorIndex = graphicsData.CurrentSectorIndex;

        this.lastGraphicsData = graphicsData;

        if(hasStartedOutLap || hasStartedPaceLap)
        {
            this.actualSectorIndex = 0;
            var accSharedMemoryLap = new AccSharedMemoryLap(staticData, graphicsData);
            this.newLapSubject.OnNext(accSharedMemoryLap);
            this.LogMessage(LoggingLevel.Debug, accSharedMemoryLap.ToString());
        }

        if(!this.isOnActiveLap)
        {
            this.isOnActiveLap = hasStartedOutLap || hasStartedPaceLap;
        }

        if(this.isOnActiveLap && (graphicsData.IsInPits || string.IsNullOrWhiteSpace(staticData.Track)))
        {
            this.isOnActiveLap = false;
        }

        return this.isOnActiveLap;
    }

    private void UpdateAppStatus(GraphicsData graphicsData)
    {
        var currentStatus = this.lastGraphicsData?.Status ?? AccAppStatus.Off;
        if(graphicsData.Status == currentStatus)
        {
            return;
        }

        this.currentAppStatus = currentStatus;
        this.appStatusChangesSubject.OnNext(new AccAppStatusChange(currentStatus, graphicsData.Status));
    }

    private void UpdateConnectionState(bool isConnected)
    {
        if(this.isConnected == isConnected)
        {
            return;
        }

        var accSharedMemoryConnectedState = new AccSharedMemoryConnectedState(this.isConnected, isConnected);
        this.connectedStateSubject.OnNext(accSharedMemoryConnectedState);
        this.isConnected = isConnected;

        if(this.isConnected)
        {
            return;
        }

        this.EndCurrentSession();
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
        if(this.lastGraphicsData == null)
        {
            return;
        }

        if(this.lastGraphicsData.Status == AccAppStatus.Live && graphicsData.Status == AccAppStatus.Off)
        {
            this.EndCurrentSession();
        }

        if(this.lastGraphicsData.Status == AccAppStatus.Off && graphicsData.Status == AccAppStatus.Live)
        {
            this.currentSession =
                new AccSharedMemorySession(staticData, graphicsData);
            this.sessionStartedSubject.OnNext(this.currentSession);
        }
    }
}