namespace SimRacingSdk.Ams2.Monitor.Abstractions;

public interface IAms2Monitor : IDisposable
{
    void Start(string? connectionIdentifier = null);
    void Stop();
}