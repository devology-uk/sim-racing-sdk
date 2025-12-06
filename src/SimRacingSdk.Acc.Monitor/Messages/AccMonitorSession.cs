#nullable disable

using SimRacingSdk.Acc.Monitor.Abstractions;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccMonitorSession() : AccMonitorMessageBase
{
    public TimeSpan Duration { get; init; }
    public Guid EventId { get; init; }
    public bool IsOnline { get; init; }
    public bool IsRunning { internal set; get; }
    public int NumberOfCars { get; init; }
    public Guid SessionId { get; init; }
    public string SessionType { get; init; }
    public string TrackName { get; init; }
}