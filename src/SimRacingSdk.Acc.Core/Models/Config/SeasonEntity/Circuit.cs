#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record Circuit
{
    [JsonPropertyName("baseGrip")]
    public double BaseGrip { get; init; }
    [JsonPropertyName("enabled")]
    public bool Enabled { get; init; }
    [JsonPropertyName("formationTriggerNormalizedRangeStart")]
    public double FormationTriggerNormalizedRangeStart { get; init; }
    [JsonPropertyName("greenFlagTriggerNormalizedRangeEnd")]
    public double GreenFlagTriggerNormalizedRangeEnd { get; init; }
    [JsonPropertyName("greenFlagTriggerNormalizedRangeStart")]
    public double GreenFlagTriggerNormalizedRangeStart { get; init; }
    [JsonPropertyName("pitNumber")]
    public int PitNumber { get; init; }
    [JsonPropertyName("sectorCount")]
    public int SectorCount { get; init; }
}