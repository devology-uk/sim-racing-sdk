#nullable disable

namespace SimRacingSdk.Ams2.Core.Models;

public record Ams2TrackInfo
{
    public int AltitudeM { get; init; }
    public string Country { get; init; }
    public string CountryCode { get; init; }
    public string FullName { get; init; }
    public double Latitude { get; init; }
    public IList<Ams2TrackLayoutInfo> Layouts { get; init; }
    public double Longitude { get; init; }
    public string ShortName { get; init; }
}