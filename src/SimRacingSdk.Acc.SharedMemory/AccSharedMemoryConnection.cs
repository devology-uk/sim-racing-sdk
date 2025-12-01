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
    private readonly Subject<bool> connectedStateSubject = new();
    private readonly Subject<AccFlagState> flagStateSubject = new();
    private readonly Subject<LogMessage> logMessagesSubject = new();
    private readonly Subject<AccSharedMemoryEvent> newEventSubject = new();
    private readonly Subject<AccSharedMemoryLap> newLapSubject = new();
    private readonly Subject<AccSharedMemorySession> newSessionSubject = new();
    private readonly IAccSharedMemoryProvider sharedMemoryProvider;
    private readonly Subject<AccTelemetryFrame> telemetrySubject = new();

    private int actualSectorIndex;
    private StaticData? currentStaticData;
    private bool isConnected;
    private bool isOnActiveLap;
    private GraphicsData? lastGraphicsData;
    private IDisposable? updateSubscription;

    public AccSharedMemoryConnection(IAccSharedMemoryProvider sharedMemoryProvider)
    {
        this.sharedMemoryProvider = sharedMemoryProvider;
    }

    public IObservable<AccAppStatusChange> AppStatusChanges => this.appStatusChangesSubject.AsObservable();
    public IObservable<bool> ConnectedState => this.connectedStateSubject.AsObservable();
    public IObservable<AccFlagState> FlagState => this.flagStateSubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();
    public IObservable<AccSharedMemoryEvent> NewEvent => this.newEventSubject.AsObservable();
    public IObservable<AccSharedMemoryLap> NewLap => this.newLapSubject.AsObservable();
    public IObservable<AccSharedMemorySession> NewSession => this.newSessionSubject.AsObservable();
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
        this.updateSubscription?.Dispose();
        this.updateSubscription = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!disposing)
        {
            return;
        }

        this.updateSubscription?.Dispose();
    }

    private bool HasAppStatusChanged(GraphicsData graphicsData)
    {
        return this.lastGraphicsData != null && this.lastGraphicsData.Status != graphicsData.Status;
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

    private bool IsNewEvent(StaticData staticData)
    {
        if(this.currentStaticData == null)
        {
            return true;
        }

        return !string.IsNullOrWhiteSpace(staticData.Track) && !string.IsNullOrWhiteSpace(staticData.CarModel)
                                                            && (this.currentStaticData.Track
                                                                != staticData.Track
                                                                || this.currentStaticData.CarModel
                                                                != staticData.CarModel
                                                                || this.currentStaticData.NumberOfSessions
                                                                != staticData.NumberOfSessions
                                                                || this.currentStaticData.NumberOfCars
                                                                != staticData.NumberOfCars
                                                                || this.currentStaticData.PlayerFirstName
                                                                != staticData.PlayerFirstName
                                                                || this.currentStaticData.IsOnline
                                                                != staticData.IsOnline);
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

        if(!staticData.IsActualEvent())
        {
            if(this.isConnected)
            {
                this.LogMessage(LoggingLevel.Information,
                    "Shared Memory data is no longer available, the user has probably quit the event.");
                this.connectedStateSubject.OnNext(false);
            }

            this.isConnected = false;
            return;
        }

        this.LogMessage(LoggingLevel.Debug, staticData.ToString());

        this.isConnected = true;
        this.connectedStateSubject.OnNext(true);

        if(this.IsNewEvent(staticData!))
        {
            this.newEventSubject.OnNext(new AccSharedMemoryEvent(staticData));
            this.currentStaticData = staticData!;
        }

        var graphicsData = this.sharedMemoryProvider.ReadGraphicsData();
        this.LogMessage(LoggingLevel.Debug, graphicsData.ToString());

        var flagState = new AccFlagState(graphicsData.IsWhiteFlagActive,
            graphicsData.IsYellowFlagActive,
            graphicsData.IsYellowFlagActiveInSector1,
            graphicsData.IsYellowFlagActiveInSector2,
            graphicsData.IsYellowFlagActiveInSector3);

        this.flagStateSubject.OnNext(flagState);

        if(this.lastGraphicsData == null)
        {
            this.appStatusChangesSubject.OnNext(new AccAppStatusChange(AccAppStatus.Off, AccAppStatus.Live));
        }
        else if(this.HasAppStatusChanged(graphicsData))
        {
            this.appStatusChangesSubject.OnNext(new AccAppStatusChange(this.lastGraphicsData.Status,
                graphicsData.Status));
        }

        if (this.lastGraphicsData == null || this.lastGraphicsData.SessionType != graphicsData.SessionType)
        {
            this.newSessionSubject.OnNext(new AccSharedMemorySession(staticData, graphicsData));
        }

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

        if(!this.isOnActiveLap)
        {
            return;
        }

        var physicsData = this.sharedMemoryProvider.ReadPhysicsData();
        this.telemetrySubject.OnNext(new AccTelemetryFrame(staticData,
            graphicsData,
            physicsData,
            this.actualSectorIndex));
        this.LogMessage(LoggingLevel.Debug, physicsData.ToString());
    }
}