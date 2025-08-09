#nullable disable

using System.Text.Json.Serialization;
using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Core;

namespace SimRacingSdk.Acc.Core.Models.RaceResult;

public record SessionDef
{
    [JsonIgnore]
    public string Duration =>
        this.SessionDuration.ValidatedValue()
            .ToTimeString();
    [JsonIgnore]
    public string StartTime => (this.DateHour * 60 + this.DateMinute).ToTimeString();
    [JsonPropertyName("dateHour")]
    public int DateHour { get; init; }
    [JsonPropertyName("dateMinute")]
    public int DateMinute { get; init; }
    [JsonPropertyName("dynamicTrackMultiplier")]
    public double DynamicTrackMultiplier { get; init; }
    [JsonPropertyName("overtimeDuration")]
    public long OvertimeDuration { get; init; }
    [JsonPropertyName("preSessionDuration")]
    public long PreSessionDuration { get; init; }
    [JsonPropertyName("raceDay")]
    public RaceDay RaceDay { get; init; }
    [JsonPropertyName("round")]
    public long Round { get; init; }
    [JsonPropertyName("sessionDuration")]
    public long SessionDuration { get; init; }
    [JsonPropertyName("sessionType")]
    public RaceSessionType SessionType { get; init; }
    [JsonPropertyName("timeMultiplier")]
    public double TimeMultiplier { get; init; }
    [JsonPropertyName("trackStatus")]
    public TrackStatus TrackStatus { get; init; }
}