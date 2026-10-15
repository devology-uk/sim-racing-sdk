using System.Text.Json.Serialization;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.Ace.DataManager.Tracks;

public record TrackInfo : ICatalogEntity
{
    public required string Continent { get; init; }

    // ISO 3166-1 alpha-3 - matches the flag PNG names in SimRacingSdk.Wpf.Shared/Images/Flags.
    public required string CountryCode { get; init; }

    public required double Latitude { get; init; }
    public required List<TrackLayoutInfo> Layouts { get; init; } = [];
    public required double Longitude { get; init; }

    // The track name ACE's shared memory reports, which is also its setup folder name.
    public required string Name { get; init; }

    public required string ShortName { get; init; }

    [JsonIgnore]
    public string DisplayName => this.ShortName;

    [JsonIgnore]
    public string Id => Slug.Create(this.Name);
}
