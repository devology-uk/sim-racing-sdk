#nullable disable
using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record Driver
{
    [JsonPropertyName("info")]
    public Info Info { get; init; }
}