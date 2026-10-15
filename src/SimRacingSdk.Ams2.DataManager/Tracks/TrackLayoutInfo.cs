using System.Text.Json.Serialization;

namespace SimRacingSdk.Ams2.DataManager.Tracks;

public record TrackLayoutInfo
{
    // The track variation AMS2's shared memory reports. Null means the default: Name with spaces as
    // underscores - set it whenever the game's id differs.
    public string? Ams2TrackId { get; init; }

    public required int Corners { get; init; }

    // Null for base-game layouts.
    public string? DlcPack { get; init; }

    public required string Grade { get; init; }
    public required int LengthM { get; init; }
    public required int MaxGridSize { get; init; }
    public required string Name { get; init; }
    public required string TrackType { get; init; }

    [JsonIgnore]
    public string EffectiveAms2TrackId => this.Ams2TrackId ?? this.Name.Replace(" ", "_");
}
