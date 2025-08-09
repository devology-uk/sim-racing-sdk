#nullable disable

using System.Text.Json.Serialization;
using SimRacingSdk.Core;

namespace SimRacingSdk.Acc.Core.Models.RaceResult;

public record MultiplayerSessionResult
{
    [JsonIgnore]
    public string BestLapTime => this.BestLap.ToTimingStringFromMilliseconds();
    [JsonPropertyName("bestLap")]
    public long BestLap { get; init; }
    [JsonPropertyName("bestSplits")]
    public long[] BestSplits { get; init; }
    [JsonPropertyName("isWetSession")]
    public bool IsWetSession { get; init; }
    [JsonPropertyName("leaderBoardLines")]
    public List<LeaderBoardLine> LeaderBoardLines { get; init; }
    [JsonPropertyName("type")]
    public int Type { get; init; }
}