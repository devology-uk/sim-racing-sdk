#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record Assists
{
    [JsonPropertyName("allowAutoSteer")]
    public int AllowAutoSteer { get; init; }
    [JsonPropertyName("autoClutch")]
    public int AutoClutch { get; init; }
    [JsonPropertyName("autoEngineSwitch")]
    public int AutoEngineSwitch { get; init; }
    [JsonPropertyName("autoGear")]
    public int AutoGear { get; init; }
    [JsonPropertyName("autoLights")]
    public int AutoLights { get; init; }
    [JsonPropertyName("autoPitLimiter")]
    public int AutoPitLimiter { get; init; }
    [JsonPropertyName("autoWiper")]
    public int AutoWiper { get; init; }
    [JsonPropertyName("description")]
    public string Description { get; init; }
    [JsonPropertyName("presetIndex")]
    public int PresetIndex { get; init; }
    [JsonPropertyName("showIdealLine")]
    public int ShowIdealLine { get; init; }
    [JsonPropertyName("stabilityControlLevelMax")]
    public double StabilityControlLevelMax { get; init; }
    [JsonPropertyName("stabilityControlLevelMin")]
    public double StabilityControlLevelMin { get; init; }
}