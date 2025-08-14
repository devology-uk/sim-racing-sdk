using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.SharedMemory;

public class AccTelemetryConnection : IAccTelemetryConnection
{
    private readonly ReplaySubject<AccTelemetryFrame> framesSubject = new();
    private readonly IAccSharedMemoryProvider sharedMemoryProvider;
    private IDisposable? updateSubscription;

    public AccTelemetryConnection(IAccSharedMemoryProvider sharedMemoryProvider)
    {
        this.sharedMemoryProvider = sharedMemoryProvider;
    }

    public IObservable<AccTelemetryFrame> Frames => this.framesSubject.AsObservable();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Start(double updateIntervalMs)
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

    private void OnNextUpdate(long index)
    {
        var staticData = this.sharedMemoryProvider.ReadStaticData();
        var graphicsData = this.sharedMemoryProvider.ReadGraphicsData();
        var physicsData = this.sharedMemoryProvider.ReadPhysicsData();
        if(staticData == null || graphicsData == null || physicsData == null)
        {
            return;
        }

        var actualSectorIndex = graphicsData.CurrentSectorIndex;
        this.framesSubject.OnNext(new AccTelemetryFrame(staticData,
            graphicsData,
            physicsData,
            actualSectorIndex));
    }
}