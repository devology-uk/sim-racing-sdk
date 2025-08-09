#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record Graphics
{
    [JsonPropertyName("cockpitLevelMirrorMax")]
    public int CockpitLevelMirrorMax { get; init; }
    [JsonPropertyName("cockpitLevelMirrorMin")]
    public int CockpitLevelMirrorMin { get; init; }
    [JsonPropertyName("rainSprayLevelMax")]
    public int RainSprayLevelMax { get; init; }
    [JsonPropertyName("rainSprayLevelMin")]
    public int RainSprayLevelMin { get; init; }
    [JsonPropertyName("rainWindShieldLevelMax")]
    public int RainWindShieldLevelMax { get; init; }
    [JsonPropertyName("rainWindShieldLevelMin")]
    public int RainWindShieldLevelMin { get; init; }
}