namespace SimRacingSdk.Abstractions
{
    public interface IGameDetector : IDisposable
    {
        IObservable<bool> Start(double updateIntervalMs = 1000);
    }
}