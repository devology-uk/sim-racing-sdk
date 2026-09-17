#nullable disable

namespace SimRacingSdk.Pmr.Core.Models;

public record PmrTrackInfo
{
    public double AltitudeMetersAmsl { get; init; }
    public string Continent { get; init; }
    public string Country { get; init; }

    // ISO 3166-1 alpha-3 - matches the flag PNG names in SimRacingSdk.Wpf.Shared/Images/Flags.
    public string CountryCode { get; init; }

    public int GridSize { get; init; }
    public double Latitude { get; init; }

    // Derived from LayoutName, not a game-internal identifier - pmr-tracks.csv is the catalog's
    // source of truth and carries no ID column of its own (see PmrCatalogImport).
    public string LayoutId { get; init; }

    public string LayoutName { get; init; }
    public double LengthMeters { get; init; }
    public double Longitude { get; init; }
    public int Turns { get; init; }

    // Derived from TrackName - see the LayoutId comment above.
    public string TrackId { get; init; }

    public string TrackName { get; init; }
}
