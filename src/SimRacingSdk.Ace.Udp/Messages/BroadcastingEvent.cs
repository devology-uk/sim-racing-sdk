#nullable disable

using SimRacingSdk.Ace.Udp.Enums;

namespace SimRacingSdk.Ace.Udp.Messages;

public record BroadcastingEvent
{
    public BroadcastingEventType BroadcastingEventType { get; init; }
    public CarInfo CarData { get; internal set; }
    public int CarId { get; init; }
    public string Message { get; init; }
    public int TimeMs { get; init; }
}
