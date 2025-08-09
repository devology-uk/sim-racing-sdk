#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record SessionRealism
{
    [JsonPropertyName("brakeFading")]
    public int BrakeFading { get; init; }
    [JsonPropertyName("damageRate")]
    public double DamageRate { get; init; }
    [JsonPropertyName("description")]
    public string Description { get; init; }
    [JsonPropertyName("eventIndex")]
    public int EventIndex { get; init; }
    [JsonPropertyName("fastRollingStart")]
    public int FastRollingStart { get; init; }
    [JsonPropertyName("isTeleportAllowed")]
    public int IsTeleportAllowed { get; init; }
    [JsonPropertyName("penalties")]
    public int Penalties { get; init; }
    [JsonPropertyName("presetIndex")]
    public int PresetIndex { get; init; }
    [JsonPropertyName("sessionIndex")]
    public int SessionIndex { get; init; }
    [JsonPropertyName("tyreFuelWear")]
    public int TyreFuelWear { get; init; }
    [JsonPropertyName("unlimitedTyreSet")]
    public int UnlimitedTyreSet { get; init; }
}