#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record CarSet
{
    [JsonPropertyName("cars")]
    public List<Car> Cars { get; set; }
}