#nullable disable

namespace SimRacingSdk.Acc.Core.Models;

public record AccTrackInfo
{
    public int Corners { get; init; }
    public string CountryTag { get; init; }
    public double Latitude { get; init; }
    public int Length { get; init; }
    public double Longitude { get; init; }
    public string Name { get; init; }
    public string TrackId { get; init; }
}