#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record EventRules
{
    [JsonPropertyName("driversStintTimeSec")]
    public int DriverStintTimeSec { get; init; }
    [JsonPropertyName("isMandatoryPitstopRefuellingRequired")]
    public bool IsMandatoryPitstopRefuellingRequired { get; init; }
    [JsonPropertyName("isMandatoryPitstopSwapDriverRequired")]
    public bool IsMandatoryPitstopSwapDriverRequired { get; init; }
    [JsonPropertyName("isMandatoryPitstopTyreChangeRequired")]
    public bool IsMandatoryPitstopTyreChangeRequired { get; init; }
    [JsonPropertyName("isRefuellingAllowedInRace")]
    public bool IsRefuellingAllowedInRace { get; init; }
    [JsonPropertyName("isRefuellingTimeFixed")]
    public bool IsRefuellingTimeFixed { get; init; }
    [JsonPropertyName("mandatoryPitstopCount")]
    public int MandatoryPitstopCount { get; init; }
    [JsonPropertyName("maxDriversCount")]
    public int MaxDriversCount { get; init; }
    [JsonPropertyName("maxTotalDrivingTime")]
    public int MaxTotalDrivingTime { get; init; }
    [JsonPropertyName("pitWindowLengthSec")]
    public int PitWindowLengthSec { get; init; }
    [JsonPropertyName("qualifyStandingType")]
    public int QualifyStandingType { get; init; }
    [JsonPropertyName("superpoleMaxCar")]
    public int SuperpoleMaxCar { get; init; }
    [JsonPropertyName("tyreSetCount")]
    public int TyreSetCount { get; init; }
}