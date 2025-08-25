namespace SimRacingSdk.Acc.Monitor.Abstractions;

public interface IAccMonitor: IDisposable
{
    void Start();
    void Stop();
}