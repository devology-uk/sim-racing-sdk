#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record StartupWeatherStatus
{
    [JsonPropertyName("ambientTemperature")]
    public double AmbientTemperature { get; init; }
    [JsonPropertyName("cloudLevel")]
    public double CloudLevel { get; init; }
    [JsonPropertyName("rainLevel")]
    public double RainLevel { get; init; }
    [JsonPropertyName("readTemperature")]
    public double RoadTemperature { get; init; }
    [JsonPropertyName("windDirection")]
    public double WindDirection { get; init; }
    [JsonPropertyName("windSpeed")]
    public double WindSpeed { get; init; }
}