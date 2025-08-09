#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record Car
{
    [JsonPropertyName("drivers")]
    public List<Driver> Drivers { get; init; }
    [JsonPropertyName("info")]
    public Info Info { get; init; }
}