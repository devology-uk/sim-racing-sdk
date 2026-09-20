#nullable disable

namespace SimRacingSdk.Pmr.Core.Models;

public record PmrTrackInfo
{
    public double AltitudeMetersAmsl { get; init; }
    public string Continent { get; init; }
    public string Country { get; init; }

    // ISO 3166-1 alpha-3 - matches the flag PNG names in SimRacingSdk.Wpf.Shared/Images/Flags.
    public string CountryCode { get; init; }

    // The game's own track id (e.g. "ID_SpaFrancorchamps"). Shared by every layout of the track -
    // the game has no id of its own for a layout.
    public string GameId { get; init; }

    public int GridSize { get; init; }
    public double Latitude { get; init; }

    // Derived from LayoutName, not a game-internal identifier.
    public string LayoutId { get; init; }

    public string LayoutName { get; init; }
    public double LengthMeters { get; init; }
    public double Longitude { get; init; }
    public int Turns { get; init; }

    // Derived from TrackName - see the LayoutId comment above.
    public string TrackId { get; init; }

    public string TrackName { get; init; }
}
