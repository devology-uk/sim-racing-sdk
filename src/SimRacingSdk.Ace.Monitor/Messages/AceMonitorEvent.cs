using SimRacingSdk.Ace.Monitor.Abstractions;

namespace SimRacingSdk.Ace.Monitor.Messages;

public record AceMonitorEvent : AceMonitorMessageBase
{
    public Guid EventId { get; init; }
    public bool IsOnline { get; init; }
    public bool IsRunning { get; internal set; } = true;
    public int NumberOfCars { get; init; }
    public string TrackName { get; init; } = string.Empty;
}
