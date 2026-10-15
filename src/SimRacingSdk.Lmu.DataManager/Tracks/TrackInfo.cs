using System.Text.Json.Serialization;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.Lmu.DataManager.Tracks;

public record TrackInfo : ICatalogEntity
{
    public required string Country { get; init; }

    // ISO 3166-1 alpha-3 - matches the flag PNG names in SimRacingSdk.Wpf.Shared/Images/Flags.
    public required string CountryCode { get; init; }

    public required double Latitude { get; init; }

    // Every track has a "Default" layout - result files name the venue only for that one, and
    // FindTrackAndLayoutByRawTrackName falls back to it.
    public required List<TrackLayoutInfo> Layouts { get; init; } = [];

    public required double Longitude { get; init; }

    // The venue name a result file reports, e.g. "Circuit de la Sarthe".
    public required string Name { get; init; }

    public required string ShortName { get; init; }

    [JsonIgnore]
    public string DisplayName => this.Name;

    [JsonIgnore]
    public string Id => Slug.Create(this.Name);
}
