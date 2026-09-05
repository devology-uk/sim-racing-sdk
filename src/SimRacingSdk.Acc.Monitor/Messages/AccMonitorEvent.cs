using SimRacingSdk.Acc.Monitor.Abstractions;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccMonitorEvent : AccMonitorMessageBase
{
    public Guid EventId { get; init; }
    public int EventIndex { get; init; }
    public bool IsOnline { get; init; }
    public bool IsRunning { get; internal set; } = true;
    public int NumberOfCars { get; init; }
    public string TrackName { get; init; } = string.Empty;
}
