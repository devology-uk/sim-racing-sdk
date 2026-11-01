#nullable disable

using SimRacingSdk.R3e.Monitor.Abstractions;
using SimRacingSdk.R3e.SharedMemory.Enums;

namespace SimRacingSdk.R3e.Monitor.Messages;

public record R3eMonitorSession : R3eMonitorMessageBase
{
    public string Country { get; init; }
    public string CountryCode { get; init; }
    public int? EventIndex { get; init; }
    public R3eGameMode GameMode { get; init; }
    public bool IsOnline { get; init; }
    public bool IsRunning { internal set; get; }
    public int LayoutId { get; init; }
    public float LayoutLengthMeters { get; init; }
    public string LayoutName { get; init; }
    public int NumberOfCars { get; init; }
    public int? NumberOfLaps { get; init; }
    public string PlayerName { get; init; }
    public int? PlayerUserId { get; init; }
    public Guid SessionId { get; init; }
    public int? SessionIteration { get; init; }
    public R3eSessionLengthFormat SessionLengthFormat { get; init; }
    public float? SessionTimeDurationSeconds { get; init; }
    public R3eSessionType SessionType { get; init; }
    public int TrackId { get; init; }
    public string TrackName { get; init; }
}
