#nullable disable

namespace SimRacingSdk.Ace.Core.Models;

public record AceTrackInfo
{
    public string Continent { get; init; }
    public string CountryCode { get; init; }
    public double Latitude { get; init; }
    public IList<AceTrackLayoutInfo> Layouts { get; init; }
    public double Longitude { get; init; }

    // The track name ACE's shared memory reports, which is also its setup folder name.
    public string Name { get; init; }

    public string ShortName { get; init; }
}

