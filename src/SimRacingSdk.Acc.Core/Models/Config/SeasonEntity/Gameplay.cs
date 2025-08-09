#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record Gameplay
{
    [JsonPropertyName("highlightRollingStartLocation")]
    public int HighlightRollingStartLocation { get; init; }
    [JsonPropertyName("highlightTargetLocation")]
    public int HighlightTargetLocation { get; init; }
    [JsonPropertyName("showFuelAlert")]
    public int ShowFuelAlert { get; init; }
    [JsonPropertyName("showRadar")]
    public int ShowRadar { get; init; }
    [JsonPropertyName("showTyreTempAlert")]
    public int ShowTyreTempAlert { get; init; }
    [JsonPropertyName("showVirtualFlags")]
    public int ShowVirtualFlags { get; init; }
    [JsonPropertyName("showVirtualMirror")]
    public int ShowVirtualMirror { get; init; }
}