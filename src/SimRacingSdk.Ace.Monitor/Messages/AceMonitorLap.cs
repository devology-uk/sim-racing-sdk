#nullable disable

using SimRacingSdk.Ace.Monitor.Abstractions;

namespace SimRacingSdk.Ace.Monitor.Messages;

public record AceMonitorLap : AceMonitorMessageBase
{
    public string CarManufacturer { get; init; }
    public string CarModelName { get; init; }
    public int CompletedLaps { get; init; }
    public string DriverName { get; init; }
    public int LastLapTimeMs { get; init; }
    public Guid SessionId { get; init; }

    // Evo's shared memory has no official sector data - these are synthetic splits at 1/3 and
    // 2/3 of NormalizedPosition, not sectors the game itself reports.
    public int? Sector1Ms { get; init; }
    public int? Sector2Ms { get; init; }
    public int? Sector3Ms { get; init; }
    public string TrackName { get; init; }
}
