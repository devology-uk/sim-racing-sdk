using SimRacingSdk.Lmu.Core.Abstractions;

namespace SimRacingSdk.Lmu.Core.Models;

public record LmuDrivingSession
{
    public IList<LmuDriver> Drivers { get; } = new List<LmuDriver>();
    public IList<LmuStreamEvent> Stream { get; } = new List<LmuStreamEvent>();
    public int DateTimeValue { get; init; }
    public int FormationAndStart { get; init; }
    public int Laps { get; init; }
    public int Minutes { get; init; }
    public int MostLapsCompleted { get; init; }
    public string? SessionName { get; init; }
    public string? SessionType { get; init; }
    public string? TimeString { get; init; }
}