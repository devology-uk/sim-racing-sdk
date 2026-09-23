using System.Text.Json.Serialization;

namespace SimRacingSdk.Pmr.DataManager.Tracks;

public record TrackInfo
{
    public required double AltitudeMeters { get; init; }

    // Which mod author/studio made the track - currently the same value ("Straight 4 Studios") for
    // every track since the game only ships its own content so far, but PMR supports mods.
    public required string Author { get; init; }

    public required string Continent { get; init; }
    public required string Country { get; init; }

    // ISO 3166-1 alpha-3 - matches the flag PNG names in SimRacingSdk.Wpf.Shared/Images/Flags.
    public required string CountryCode { get; init; }

    // The game's own track id (e.g. "ID_SpaFrancorchamps"), shared by every layout of the track -
    // the game has no id of its own for an individual layout. Null until looked up for a newly
    // added track.
    public string? GameId { get; init; }

    public required double Latitude { get; init; }
    public required List<TrackLayoutInfo> Layouts { get; init; } = [];
    public required double Longitude { get; init; }
    public required string Name { get; init; }

    // The JSON file's name, derived from Name - Nurburgring's Nordschleife variants are their own
    // distinct tracks with their own Name here, not layouts of one track, so no extra
    // disambiguation beyond Name is needed.
    [JsonIgnore]
    public string Id => Slug.Create(this.Name);
}
