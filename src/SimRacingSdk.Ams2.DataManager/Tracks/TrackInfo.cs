using System.Text.Json.Serialization;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.Ams2.DataManager.Tracks;

public record TrackInfo : ICatalogEntity
{
    public required int AltitudeM { get; init; }
    public required string Country { get; init; }

    // ISO 3166-1 alpha-3 - matches the flag PNG names in SimRacingSdk.Wpf.Shared/Images/Flags.
    public required string CountryCode { get; init; }

    public required string FullName { get; init; }
    public required double Latitude { get; init; }
    public required List<TrackLayoutInfo> Layouts { get; init; } = [];
    public required double Longitude { get; init; }

    // The exact track locations AMS2's shared memory reports where they don't reduce to ShortName
    // (e.g. "Nurburgring_2025" for "Nürburgring"). Matched before ShortName.
    public List<string> RawTrackLocations { get; init; } = [];

    public required string ShortName { get; init; }

    [JsonIgnore]
    public string DisplayName => this.ShortName;

    [JsonIgnore]
    public string Id => Slug.Create(this.ShortName);
}
