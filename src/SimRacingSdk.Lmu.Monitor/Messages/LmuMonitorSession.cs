#nullable disable

using SimRacingSdk.Lmu.Monitor.Abstractions;
using SimRacingSdk.Lmu.SharedMemory.Enums;

namespace SimRacingSdk.Lmu.Monitor.Messages;

public record LmuMonitorSession() : LmuMonitorMessageBase
{
    public double EndEt { get; init; }
    public bool IsRunning { internal set; get; }
    public int MaxLaps { get; init; }
    public int NumberOfCars { get; init; }
    public Guid SessionId { get; init; }
    public LmuSessionType SessionType { get; init; }
    public string TrackName { get; init; }
}
