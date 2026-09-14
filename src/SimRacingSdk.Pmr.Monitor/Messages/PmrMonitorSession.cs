#nullable disable

using SimRacingSdk.Pmr.Monitor.Abstractions;

namespace SimRacingSdk.Pmr.Monitor.Messages;

public record PmrMonitorSession() : PmrMonitorMessageBase
{
    public float AmbientTemperatureCelsius { get; init; }
    public float DurationSeconds { get; init; }
    public string GameMode { get; init; }
    public bool IsLaps { get; init; }
    public bool IsRunning { internal set; get; }
    public string Layout { get; init; }
    public byte NumberOfParticipants { get; init; }
    public float OvertimeSeconds { get; init; }
    public string Season { get; init; }
    public Guid SessionId { get; init; }
    public string SessionType { get; init; }
    public string TrackName { get; init; }
    public float TrackTemperatureCelsius { get; init; }
    public string Weather { get; init; }
}
