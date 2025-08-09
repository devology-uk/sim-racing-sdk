#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.RaceResult;

public record MultiplayerSession
{
    [JsonPropertyName("laps")]
    public List<MultiplayerLap> Laps { get; init; }
    [JsonPropertyName("metaData")]
    public string MetaData { get; init; }
    [JsonPropertyName("penalties")]
    public List<MultiplayerPenalty> Penalties { get; init; }
    [JsonPropertyName("post_race_penalties")]
    public List<MultiplayerPenalty> PostRacePenalties { get; init; }
    [JsonPropertyName("raceWeekendIndex")]
    public int RaceWeekendIndex { get; init; }
    [JsonPropertyName("serverName")]
    public string ServerName { get; init; }
    [JsonPropertyName("sessionIndex")]
    public int SessionIndex { get; init; }
    [JsonPropertyName("sessionResult")]
    public MultiplayerSessionResult SessionResult { get; init; }
    [JsonPropertyName("sessionType")]
    public string SessionType { get; init; }
    [JsonPropertyName("trackName")]
    public string TrackName { get; init; }

    internal Car GetCar(int carId)
    {
        var allCars = this.SessionResult.LeaderBoardLines.Select(l => l.Car)
                          .ToList();
        return allCars?.FirstOrDefault(c => c.CarId == carId)!;
    }

    internal Driver GetDriver(int carId, int driverIndex)
    {
        var car = this.GetCar(carId);

        return car?.Drivers.FirstOrDefault()!;
    }
}