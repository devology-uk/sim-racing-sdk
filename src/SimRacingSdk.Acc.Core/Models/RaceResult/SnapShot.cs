#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.RaceResult;

public record SnapShot
{
    [JsonPropertyName("bestlap")]
    public long Bestlap { get; set; }
    [JsonPropertyName("bestSplits")]
    public List<long> BestSplits { get; set; }
    [JsonPropertyName("isWetSession")]
    public int IsWetSession { get; set; }
    [JsonPropertyName("leaderBoardLines")]
    public List<LeaderBoardLine> LeaderBoardLines { get; set; }
    [JsonPropertyName("type")]
    public int Type { get; set; }

    internal LeaderBoardLine GetLeaderBoardLineByCarId(int carId)
    {
        return this.LeaderBoardLines.FirstOrDefault(l => l.Car.CarId == carId)!;
    }

    internal LeaderBoardLine GetPlayerLeaderBoardLine()
    {
        return this.LeaderBoardLines.First(e => e.CurrentDriver.PlayerId != "0");
    }
}