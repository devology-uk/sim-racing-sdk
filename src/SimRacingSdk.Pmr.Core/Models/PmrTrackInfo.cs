#nullable disable

namespace SimRacingSdk.Pmr.Core.Models;

public record PmrTrackInfo
{
    public double AltitudeMetersAmsl { get; init; }
    public string City { get; init; }
    public string Continent { get; init; }
    public string Country { get; init; }

    // ISO 3166-1 alpha-3 - matches the flag PNG names in SimRacingSdk.Wpf.Shared/Images/Flags.
    public string CountryCode { get; init; }

    public string Direction { get; init; }
    public int GridSize { get; init; }
    public double Latitude { get; init; }
    public string LayoutId { get; init; }
    public string LayoutName { get; init; }
    public double LengthMeters { get; init; }
    public double Longitude { get; init; }
    public double MaxOvertimeSeconds { get; init; }
    public double PitSpeedLimitMetersPerSecond { get; init; }
    public int Turns { get; init; }
    public string TrackFolder { get; init; }
    public string TrackId { get; init; }
    public string TrackName { get; init; }
    public double TimeZoneUtcOffset { get; init; }
    public int TrackYear { get; init; }
}
