#nullable disable

using SimRacingSdk.Ams2.Monitor.Enums;

namespace SimRacingSdk.Ams2.Monitor.Messages;

public record Ams2MonitorSession
{
    public DateTime? EndTime { get; set; }
    public int ParticipantCount { get; set; }
    public int PlayerLapCount { get; set; }
    public float ScheduledDurationMs { get; init; }
    public uint ScheduledLaps { get; init; }
    public Ams2MonitorSessionType SessionType { get; init; }
    public DateTime StartTime { get; init; }
    public int TotalLapCount { get; set; }
    public string TrackLayout { get; init; }
    public string TrackLocation { get; init; }
}