#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.RaceResult;

public record MultiplayerPenalty
{
    [JsonPropertyName("carId")]
    public int CarId { get; init; }
    [JsonPropertyName("clearedInLap")]
    public int ClearedInLap { get; init; }
    [JsonPropertyName("driverIndex")]
    public int DriverIndex { get; init; }
    [JsonPropertyName("penalty")]
    public string Penalty { get; init; }
    [JsonPropertyName("penaltyValue")]
    public int PenaltyValue { get; init; }
    [JsonPropertyName("reason")]
    public string Reason { get; init; }
    [JsonPropertyName("violationInLap")]
    public int ViolationInLap { get; init; }
}