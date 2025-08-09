#nullable disable

using System.Text.Json.Serialization;
using SimRacingSdk.Acc.Core.Enums;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record Session
{
    [JsonPropertyName("dateHour")]
    public int DateHour { get; init; }
    [JsonPropertyName("dateMinute")]
    public int DateMinute { get; init; }
    [JsonPropertyName("dynamicTrackMultiplier")]
    public double DynamicTrackMultiplier { get; init; }
    [JsonPropertyName("overtimeDuration")]
    public int OvertimeDuration { get; init; }
    [JsonPropertyName("preSessionDuration")]
    public int PreSessionDuration { get; init; }
    [JsonPropertyName("raceDay")]
    public RaceDay RaceDay { get; init; }
    [JsonPropertyName("sessionDuration")]
    public int Round { get; init; }
    [JsonPropertyName("sessionDuration")]
    public long SessionDuration { get; init; }
    [JsonPropertyName("sessionType")]
    public int SessionType { get; init; }
    [JsonPropertyName("timeMultiplier")]
    public double TimeMultiplier { get; init; }
    [JsonPropertyName("trackStatus")]
    public TrackStatus TrackStatus { get; init; }
}