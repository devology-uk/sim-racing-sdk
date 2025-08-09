#nullable disable

using System.Text.Json.Serialization;
using SimRacingSdk.Core;

namespace SimRacingSdk.Acc.Core.Models.RaceResult;

public record MultiplayerLap
{
    [JsonIgnore]
    public string Sector1Time =>
        this.Splits[0]
            .ValidatedValue()
            .ToTimingStringFromMilliseconds();
    [JsonIgnore]
    public string Sector2Time =>
        this.Splits[1]
            .ValidatedValue()
            .ToTimingStringFromMilliseconds();
    [JsonIgnore]
    public string Sector3Time =>
        this.Splits[2]
            .ValidatedValue()
            .ToTimingStringFromMilliseconds();
    [JsonPropertyName("carId")]
    public int CarId { get; set; }
    [JsonPropertyName("driverIndex")]
    public int DriverIndex { get; set; }
    [JsonPropertyName("isValidForFastest")]
    public bool IsValidForBest { get; set; }
    [JsonPropertyName("lapTime")]
    public long LapTime { get; set; }
    [JsonPropertyName("splits")]
    public List<long> Splits { get; set; }

    public string GetLapTime()
    {
        return this.LapTime.ValidatedValue()
                   .ToTimingStringFromMilliseconds();
    }
}