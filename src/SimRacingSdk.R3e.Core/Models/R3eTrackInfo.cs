#nullable disable

namespace SimRacingSdk.R3e.Core.Models;

public record R3eTrackInfo
{
    public string Country { get; init; }
    public string CountryCode { get; init; }
    // The game's own track id, as shared memory reports it.
    public int GameId { get; init; }
    public double? Latitude { get; init; }
    public IList<R3eTrackLayoutInfo> Layouts { get; init; }
    public double? Longitude { get; init; }
    public string Name { get; init; }
    // Known to the game but not offered in its track selector.
    public bool NotInGameSelector { get; init; }
}
