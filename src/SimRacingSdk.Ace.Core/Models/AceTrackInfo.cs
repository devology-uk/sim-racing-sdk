#nullable disable

namespace SimRacingSdk.Ace.Core.Models;

public record AceTrackInfo
{
    public string Continent { get; init; }
    public int Corners { get; init; }
    public string CountryCode { get; init; }
    public string Track { get; init; }
    public string ShortName { get; init; }
    public string Layout { get; init; }
    public float TrackLengthMeters { get; init; }
    public int MaxPitSlot { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}
