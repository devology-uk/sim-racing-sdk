#nullable disable
using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.RaceResult;

public record Car
{
    [JsonPropertyName("carGroup")]
    public string CarGroup { get; init; }
    [JsonPropertyName("carGuid")]
    public long CarGuid { get; init; }
    [JsonPropertyName("carId")]
    public int CarId { get; init; }
    [JsonPropertyName("carModel")]
    public int CarModel { get; init; }
    [JsonPropertyName("cupCategory")]
    public int CupCategory { get; init; }
    [JsonPropertyName("drivers")]
    public List<Driver> Drivers { get; init; }
    [JsonPropertyName("nationality")]
    public int Nationality { get; init; }
    [JsonPropertyName("raceNumber")]
    public int RaceNumber { get; init; }
    [JsonPropertyName("teamGuid")]
    public long TeamGuid { get; init; }
    [JsonPropertyName("teamName")]
    public string TeamName { get; init; }

    public Driver GetDriverByIndex(int index)
    {
        return this.Drivers[index];
    }
}