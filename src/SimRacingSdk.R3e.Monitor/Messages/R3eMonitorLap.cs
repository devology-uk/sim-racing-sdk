#nullable disable

using SimRacingSdk.R3e.Monitor.Abstractions;
using SimRacingSdk.R3e.SharedMemory.Enums;

namespace SimRacingSdk.R3e.Monitor.Messages;

public record R3eMonitorLap : R3eMonitorMessageBase
{
    public string CarClass { get; init; }
    public string CarManufacturer { get; init; }
    public string CarModelName { get; init; }
    public int CarNumber { get; init; }
    public int CompletedLaps { get; init; }
    public string DriverName { get; init; }
    public R3eFinishStatus FinishStatus { get; init; }
    public bool IsLocalPlayer { get; init; }

    // R3E finishes the whole field the moment the player takes the flag, so cars still on track get a final lap
    // with no time that they never really drove.
    public bool IsSimulatedFinish { get; init; }

    // A lap the game timed and didn't invalidate. Out laps report valid but have no time, so they aren't valid here;
    // an invalidated race lap keeps its time, so a time alone doesn't make a lap valid either.
    public bool IsValid { get; init; }
    public int? LapTimeMs { get; init; }
    public string LayoutName { get; init; }
    public int LiveryId { get; init; }
    public int ModelId { get; init; }
    public int Place { get; init; }
    public int PlaceClass { get; init; }
    public int? Sector1Ms { get; init; }
    public int? Sector2Ms { get; init; }
    public int? Sector3Ms { get; init; }
    public Guid SessionId { get; init; }
    public int SlotId { get; init; }
    public int TeamId { get; init; }
    public string TrackName { get; init; }
    public int? UserId { get; init; }
}
