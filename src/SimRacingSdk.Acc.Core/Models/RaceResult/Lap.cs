#nullable disable

using System.Text.Json.Serialization;
using SimRacingSdk.Core;

namespace SimRacingSdk.Acc.Core.Models.RaceResult;

public record Lap
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
    [JsonIgnore]
    public string Timestamp => this.TimestampMS.ToTimingStringFromMilliseconds();

    [JsonPropertyName("carId")]
    public int CarId { get; set; }
    [JsonPropertyName("driverId")]
    public int DriverId { get; set; }
    [JsonPropertyName("flags")]
    public int Flags { get; set; }
    [JsonPropertyName("fuel")]
    public double Fuel { get; set; }
    [JsonPropertyName("isValidForBest")]
    public bool IsValidForBest { get; set; }
    [JsonPropertyName("lapTime")]
    public long LapTime { get; set; }
    [JsonPropertyName("splits")]
    public List<long> Splits { get; set; }
    [JsonPropertyName("timestampMS")]
    public double TimestampMS { get; set; }

    public string GetLapTime()
    {
        return this.LapTime.ValidatedValue()
                   .ToTimingStringFromMilliseconds();
    }
}