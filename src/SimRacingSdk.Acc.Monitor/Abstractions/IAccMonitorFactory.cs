namespace SimRacingSdk.Acc.Monitor.Abstractions;

public interface IAccMonitorFactory
{
    IAccMonitor Create(string? connectionIdentifier = null);
}