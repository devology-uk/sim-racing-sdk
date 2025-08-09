#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record SessionGameplay
{
    [JsonPropertyName("aggroMultiplier")]
    public int AggroMultiplier { get; init; }
    [JsonPropertyName("eventIndex")]
    public int EventIndex { get; init; }
    [JsonPropertyName("playerSessionPostTime")]
    public int PlayerSessionPostTime { get; init; }
    [JsonPropertyName("sessionIndex")]
    public int SessionIndex { get; init; }
    [JsonPropertyName("skillMultiplier")]
    public int SkillMultiplier { get; init; }
    [JsonPropertyName("superpoleVirtualSession")]
    public int SuperpoleVirtualSession { get; init; }
}