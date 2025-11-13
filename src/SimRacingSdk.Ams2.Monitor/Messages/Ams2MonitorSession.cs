using SimRacingSdk.Ams2.Monitor.Enums;

namespace SimRacingSdk.Ams2.Monitor.Messages;

public record Ams2MonitorSession
{
    public DateTime? EndTime { get; set; }
    public int LapCount { get; set; }
    public int ParticipantCount { get; set; }
    public float ScheduledDurationMs { get; init; }
    public uint ScheduledLaps { get; init; }
    public Ams2MonitorSessionType SessionType { get; init; }
    public DateTime StartTime { get; init; }
}