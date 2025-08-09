#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record Event
{
    [JsonPropertyName("carSet")]
    public CarSet CarSet { get; init; }
    [JsonPropertyName("circuit")]
    public Circuit Circuit { get; init; }
    [JsonPropertyName("graphics")]
    public Graphics Graphics { get; init; }
    [JsonPropertyName("race")]
    public Race Race { get; init; }
    [JsonPropertyName("startupWeatherData")]
    public StartupWeatherData StartupWeatherData { get; init; }
    [JsonPropertyName("startupWeatherStatus")]
    public StartupWeatherStatus StartupWeatherStatus { get; init; }
    [JsonPropertyName("trackName")]
    public string TrackName { get; init; }
}