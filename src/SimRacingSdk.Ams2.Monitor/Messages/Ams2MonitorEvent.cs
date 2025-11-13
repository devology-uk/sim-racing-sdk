#nullable disable

namespace SimRacingSdk.Ams2.Monitor.Messages;

public record Ams2MonitorEvent
{
    public DateTime TimeStamp { get; init; }
    public string TrackLayout { get; init; }
    public string TrackLocation { get; init; }
}