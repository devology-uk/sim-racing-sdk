#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.RaceResult;

public record LeaderBoardLine
{
    [JsonPropertyName("car")]
    public Car Car { get; init; }
    [JsonPropertyName("currentDriver")]
    public Driver CurrentDriver { get; init; }
    [JsonPropertyName("currentDriverIndex")]
    public int CurrentDriverIndex { get; init; }
    [JsonPropertyName("driverTotalTimes")]
    public List<double> DriverTotalTimes { get; init; }
    [JsonPropertyName("missingMandatoryPitstop")]
    public int MissingMandatoryPitstop { get; init; }
    [JsonPropertyName("timing")]
    public Timing Timing { get; init; }
}