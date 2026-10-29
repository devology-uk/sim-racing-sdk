using System.Text.Json.Serialization;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.R3e.DataManager.Tracks;

public record TrackInfo : ICatalogEntity
{
    // Owned by the game's r3e-data.json and refreshed by Import From Game.
    public required int GameId { get; init; }
    public required List<TrackLayoutInfo> Layouts { get; init; } = [];

    // The game lists each era of a circuit as its own track, e.g. "Circuit de Spa-Francorchamps - 1995".
    public required string Name { get; init; }

    // Curated by hand; empty or null until captured.
    public string Country { get; init; } = string.Empty;

    // ISO 3166-1 alpha-3 - matches the flag PNG names in SimRacingSdk.Wpf.Shared/Images/Flags.
    public string CountryCode { get; init; } = string.Empty;

    public double? Latitude { get; init; }
    public double? Longitude { get; init; }

    // In r3e-data.json but not in the game's track selector. Kept, not deleted: Import From Game would bring it
    // back, and shared memory can still report it.
    public bool NotInGameSelector { get; init; }

    [JsonIgnore]
    public string DisplayName => this.Name;

    [JsonIgnore]
    public string Id => Slug.Create(this.Name);
}
