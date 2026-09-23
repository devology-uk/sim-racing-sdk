#nullable disable

namespace SimRacingSdk.Pmr.Core.Models;

public record PmrTrackLayoutInfo
{
    public int GridSize { get; init; }
    public double LengthMeters { get; init; }
    public string Name { get; init; }
    public int Turns { get; init; }
}
