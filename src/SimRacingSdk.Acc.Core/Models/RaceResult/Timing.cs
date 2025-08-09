#nullable disable

using System.Text.Json.Serialization;
using SimRacingSdk.Core;

namespace SimRacingSdk.Acc.Core.Models.RaceResult;

public record Timing
{
    [JsonIgnore]
    public string AverageLapTime =>
        (this.LapCount > 0? ((double)this.TotalTime.ValidatedValue()) / this.LapCount: 0)
        .ToTimingStringFromMilliseconds();

    [JsonIgnore]
    public string BestLapTime =>
        this.BestLap.ValidatedValue()
            .ToTimingStringFromMilliseconds();
    [JsonIgnore]
    public string BestSector1 => (this.LapCount > 0? this.BestSplits[0]: 0).ToTimingStringFromMilliseconds();
    [JsonIgnore]
    public string BestSector2 => (this.LapCount > 0? this.BestSplits[1]: 0).ToTimingStringFromMilliseconds();
    [JsonIgnore]
    public string BestSector3 => (this.LapCount > 0? this.BestSplits[2]: 0).ToTimingStringFromMilliseconds();
    [JsonPropertyName("bestLap")]
    public long BestLap { get; init; }
    [JsonPropertyName("bestSplits")]
    public List<int> BestSplits { get; init; }
    [JsonPropertyName("lapCount")]
    public int LapCount { get; init; }
    [JsonPropertyName("lastLap")]
    public long LastLap { get; init; }
    [JsonPropertyName("lastSplitId")]
    public long LastSplitId { get; init; }
    [JsonPropertyName("lastSplits")]
    public List<long> LastSplits { get; init; }
    [JsonPropertyName("totalTime")]
    public long TotalTime { get; init; }
}