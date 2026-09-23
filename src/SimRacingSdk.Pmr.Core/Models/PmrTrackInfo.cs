#nullable disable

namespace SimRacingSdk.Pmr.Core.Models;

public record PmrTrackInfo
{
    public double AltitudeMetersAmsl { get; init; }
    public string Continent { get; init; }
    public string Country { get; init; }
    public string CountryCode { get; init; }

    // The game's own track id (e.g. "ID_SpaFrancorchamps") - shared by every layout, since the
    // game has no id of its own for an individual layout.
    public string GameId { get; init; }

    public double Latitude { get; init; }
    public IList<PmrTrackLayoutInfo> Layouts { get; init; }
    public double Longitude { get; init; }
    public string Name { get; init; }
}
