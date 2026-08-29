#nullable disable

using SimRacingSdk.Ace.Monitor.Abstractions;

namespace SimRacingSdk.Ace.Monitor.Messages;

public record AceMonitorSession() : AceMonitorMessageBase
{
    public TimeSpan Duration { get; init; }
    public bool IsOnline { get; init; }
    public bool IsRunning { internal set; get; }
    public int NumberOfCars { get; init; }
    public Guid SessionId { get; init; }
    public string SessionType { internal set; get; }
    public string TrackName { get; init; }
}
