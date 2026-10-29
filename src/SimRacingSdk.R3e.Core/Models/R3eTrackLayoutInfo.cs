#nullable disable

namespace SimRacingSdk.R3e.Core.Models;

public record R3eTrackLayoutInfo
{
    // The game's own layout id, as shared memory reports it.
    public int GameId { get; init; }
    public int GridSize { get; init; }
    public double? LengthKm { get; init; }
    public string Name { get; init; }
}
