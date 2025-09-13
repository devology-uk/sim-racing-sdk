#nullable disable

using SimRacingSdk.Acc.Monitor.Abstractions;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccMonitorSession(string EventId, string SessionType) : AccMonitorMessageBase
{
    public TimeSpan Duration { get; internal set; }
    public bool IsOnline { get; internal set; }
    public int NumberOfCars { get; internal set; }
}