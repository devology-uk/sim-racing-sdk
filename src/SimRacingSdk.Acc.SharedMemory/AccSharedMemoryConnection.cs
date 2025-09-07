using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.SharedMemory;

public class AccSharedMemoryConnection : IAccSharedMemoryConnection
{
    private readonly Subject<AccFlagState> flagStateSubject = new();
    private readonly Subject<LogMessage> logMessagesSubject = new();
    private readonly ReplaySubject<AccSharedMemoryEvent> newEventSubject = new();
    private readonly ReplaySubject<AccSharedMemoryLap> newLapSubject = new();
    private readonly Subject<string> sessionEndedSubject = new();
    private readonly Subject<string> sessionStartedSubject = new();
    private readonly IAccSharedMemoryProvider sharedMemoryProvider;
    private readonly ReplaySubject<AccTelemetryFrame> telemetrySubject = new();
    private int actualSectorIndex;

    private StaticData? currentStaticData;
    private bool isOnActiveLap;
    private GraphicsData? lastGraphicsData;
    private IDisposable? updateSubscription;

    public AccSharedMemoryConnection(IAccSharedMemoryProvider sharedMemoryProvider)
    {
        this.sharedMemoryProvider = sharedMemoryProvider;
    }

    public IObservable<AccFlagState> FlagState => this.flagStateSubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();
    public IObservable<AccSharedMemoryEvent> NewEvent => this.newEventSubject.AsObservable();
    public IObservable<AccSharedMemoryLap> NewLap => this.newLapSubject.AsObservable();
    public IObservable<string> SessionEnded => this.sessionEndedSubject.AsObservable();
    public IObservable<string> SessionStarted => this.sessionStartedSubject.AsObservable();
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

    protected virtual void Dispose(bool disposing)
    {
        if(!disposing)
        {
            return;
        }

        this.telemetrySubject.Dispose();
        this.updateSubscription?.Dispose();
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
                                                                || this.currentStaticData.PlayerName
                                                                != staticData.PlayerName
                                                                || this.currentStaticData.IsOnline
                                                                != staticData.IsOnline);
    }

    private void OnNextUpdate(long index)
    {
        var staticData = this.sharedMemoryProvider.ReadStaticData();
        if(!staticData.IsActualEvent())
        {
            return;
        }

        this.logMessagesSubject.OnNext(new LogMessage(LoggingLevel.Debug, staticData.ToString()));

        if(this.IsNewEvent(staticData!))
        {
            this.newEventSubject.OnNext(new AccSharedMemoryEvent(staticData));
            this.currentStaticData = staticData!;
        }

        var graphicsData = this.sharedMemoryProvider.ReadGraphicsData();
        this.logMessagesSubject.OnNext(new LogMessage(LoggingLevel.Debug, graphicsData.ToString()));

        var flagState = new AccFlagState(graphicsData.IsWhiteFlagActive,
            graphicsData.IsYellowFlagActive,
            graphicsData.IsYellowFlagActiveInSector1,
            graphicsData.IsYellowFlagActiveInSector2,
            graphicsData.IsYellowFlagActiveInSector3);

        this.flagStateSubject.OnNext(flagState);
        this.logMessagesSubject.OnNext(new LogMessage(LoggingLevel.Debug, flagState.ToString()));

        if(this.lastGraphicsData == null)
        {
            this.sessionStartedSubject.OnNext(graphicsData.SessionType.ToFriendlyName());
        }
        else if(this.lastGraphicsData.SessionType != graphicsData.SessionType)
        {
            this.sessionEndedSubject.OnNext(this.lastGraphicsData.SessionType.ToFriendlyName());
            this.sessionStartedSubject.OnNext(graphicsData.SessionType.ToFriendlyName());
        }

        this.lastGraphicsData = graphicsData;

        var hasStartedOutLap = this.HasStartedOutLap(graphicsData);
        var hasStartedPaceLap = this.HasStartedPaceLap(graphicsData);
        this.actualSectorIndex = graphicsData.CurrentSectorIndex;

        if(hasStartedOutLap || hasStartedPaceLap)
        {
            this.actualSectorIndex = 0;
            var accSharedMemoryLap = new AccSharedMemoryLap(staticData, graphicsData);
            this.newLapSubject.OnNext(accSharedMemoryLap);
            this.logMessagesSubject.OnNext(new LogMessage(LoggingLevel.Debug, accSharedMemoryLap.ToString()));
        }

        if(!this.isOnActiveLap)
        {
            this.isOnActiveLap = hasStartedOutLap || hasStartedPaceLap;
        }

        if(this.isOnActiveLap && (graphicsData.IsInPits || string.IsNullOrWhiteSpace(staticData.Track)))
        {
            this.isOnActiveLap = false;
        }

        if(this.isOnActiveLap)
        {
            var physicsData = this.sharedMemoryProvider.ReadPhysicsData();
            this.telemetrySubject.OnNext(new AccTelemetryFrame(staticData,
                graphicsData,
                physicsData,
                this.actualSectorIndex));
            this.logMessagesSubject.OnNext(new LogMessage(LoggingLevel.Debug, physicsData.ToString()));
        }
    }
}