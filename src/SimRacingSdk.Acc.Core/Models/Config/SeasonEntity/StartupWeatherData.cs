#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record StartupWeatherData
{
    [JsonPropertyName("ambientTemperatureMean")]
    public double AmbientTemperatureMean { get; init; }
    [JsonPropertyName("cosineCoefficients")]
    public List<object> CosineCoefficients { get; init; }
    [JsonPropertyName("isDynamic")]
    public int IsDynamic { get; init; }
    [JsonPropertyName("nHarmonics")]
    public int NHarmonics { get; init; }
    [JsonPropertyName("sineCoefficients")]
    public List<object> SineCoefficients { get; init; }
    [JsonPropertyName("variabilityDeviation")]
    public double VariabilityDeviation { get; init; }
    [JsonPropertyName("weatherBaseDeviation")]
    public double WeatherBaseDeviation { get; init; }
    [JsonPropertyName("weatherBaseMean")]
    public double WeatherBaseMean { get; init; }
    [JsonPropertyName("windDirection")]
    public double WindDirection { get; init; }
    [JsonPropertyName("windDirectionChange")]
    public double WindDirectionChange { get; init; }
    [JsonPropertyName("windHarmonic")]
    public int WindHarmonic { get; init; }
    [JsonPropertyName("windSpeed")]
    public double WindSpeed { get; init; }
    [JsonPropertyName("windSpeedDeviation")]
    public double WindSpeedDeviation { get; init; }
    [JsonPropertyName("windSpeedMean")]
    public double WindSpeedMean { get; init; }
}