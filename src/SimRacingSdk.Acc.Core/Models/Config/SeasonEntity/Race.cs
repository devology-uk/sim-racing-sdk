#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record Race
{
    [JsonPropertyName("eventRules")]
    public EventRules EventRules { get; init; }
    [JsonPropertyName("eventType")]
    public int EventType { get; init; }
    [JsonPropertyName("sessions")]
    public List<Session> Sessions { get; init; }
}