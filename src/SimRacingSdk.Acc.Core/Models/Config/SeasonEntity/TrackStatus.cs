#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record TrackStatus
{
    [JsonPropertyName("idealLineGrip")]
    public double IdealLineGrip { get; init; }
    [JsonPropertyName("marblesLevel")]
    public double MarblesLevel { get; init; }
    [JsonPropertyName("outsideLineGrip")]
    public double OutsideLineGrip { get; init; }
    [JsonPropertyName("puddlesLevel")]
    public double PuddlesLevel { get; init; }
    [JsonPropertyName("wetDryLineLevel")]
    public double WetDryLineLevel { get; init; }
    [JsonPropertyName("wetLevel")]
    public double WetLevel { get; init; }
}