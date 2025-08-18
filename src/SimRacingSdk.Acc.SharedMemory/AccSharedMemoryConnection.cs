using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.SharedMemory;

public class AccSharedMemoryConnection : IAccSharedMemoryConnection
{
    private readonly ReplaySubject<AccTelemetryFrame> framesSubject = new();
    private readonly ReplaySubject<AccTelemetryEvent> newEventSubject = new();
    private readonly ReplaySubject<AccTelemetryLap> newLapSubject = new();
    private readonly IAccSharedMemoryProvider sharedMemoryProvider;
    private int actualSectorIndex;

    private StaticData? currentEventData;
    private bool isOnActiveLap;
    private GraphicsData? lastGraphicsData;
    private IDisposable? updateSubscription;

    public AccSharedMemoryConnection(IAccSharedMemoryProvider sharedMemoryProvider)
    {
        this.sharedMemoryProvider = sharedMemoryProvider;
    }

    public IObservable<AccTelemetryFrame> Frames => this.framesSubject.AsObservable();

    public IObservable<AccTelemetryEvent> NewEvent => this.newEventSubject.AsObservable();

    public IObservable<AccTelemetryLap> NewLap => this.newLapSubject.AsObservable();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Start(double updateIntervalMs = 100)
    {
        this.updateSubscription = Observable.Interval(TimeSpan.FromMilliseconds(updateIntervalMs))
                                            .Subscribe(this.OnNextUpdate,
                                                e => this.framesSubject.OnError(e),
                                                () => this.framesSubject.OnCompleted());
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!disposing)
        {
            return;
        }

        this.framesSubject.Dispose();
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
        return this.lastGraphicsData != null && this.currentEventData != null
               && this.lastGraphicsData.CurrentSectorIndex == this.currentEventData.SectorCount - 1
               && graphicsData.CurrentSectorIndex == 0;
    }

    private bool IsNewEvent(StaticData staticData)
    {
        if(this.currentEventData == null)
        {
            return true;
        }

        return !string.IsNullOrWhiteSpace(staticData.Track) && !string.IsNullOrWhiteSpace(staticData.CarModel)
                                                            && (this.currentEventData.Track
                                                                != staticData.Track
                                                                || this.currentEventData.CarModel
                                                                != staticData.CarModel
                                                                || this.currentEventData.NumberOfSessions
                                                                != staticData.NumberOfSessions
                                                                || this.currentEventData.NumberOfCars
                                                                != staticData.NumberOfCars
                                                                || this.currentEventData.PlayerName
                                                                != staticData.PlayerName
                                                                || this.currentEventData.IsOnline
                                                                != staticData.IsOnline);
    }

    private void OnNextUpdate(long index)
    {
        var staticData = this.sharedMemoryProvider.ReadStaticData();
        if(staticData == null || !staticData.IsActualEvent())
        {
            return;
        }

        if(this.IsNewEvent(staticData!))
        {
            this.newEventSubject.OnNext(new AccTelemetryEvent(staticData));
            this.currentEventData = staticData!;
        }

        var graphicsData = this.sharedMemoryProvider.ReadGraphicsData();
        if(graphicsData == null)
        {
            return;
        }

        var hasStartedOutLap = this.HasStartedOutLap(graphicsData);
        var hasStartedPaceLap = this.HasStartedPaceLap(graphicsData);
        this.actualSectorIndex = graphicsData.CurrentSectorIndex;

        if(hasStartedOutLap || hasStartedPaceLap)
        {
            this.actualSectorIndex = 0;
            this.newLapSubject.OnNext(new AccTelemetryLap(staticData, graphicsData));
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
            if(physicsData != null)
            {
                this.framesSubject.OnNext(new AccTelemetryFrame(staticData,
                    graphicsData,
                    physicsData,
                    this.actualSectorIndex));
            }
        }

        this.lastGraphicsData = graphicsData;
    }
}