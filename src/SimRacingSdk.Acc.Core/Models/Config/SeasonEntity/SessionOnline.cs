#nullable disable

using System.Text.Json.Serialization;

namespace SimRacingSdk.Acc.Core.Models.Config.SeasonEntity;

public record SessionOnline
{
    [JsonPropertyName("bonnetCameraMode")]
    public int BonnetCameraMode { get; init; }
    [JsonPropertyName("bumperCameraMode")]
    public int BumperCameraMode { get; init; }
    [JsonPropertyName("chaseCameraMode")]
    public int ChaseCameraMode { get; init; }
    [JsonPropertyName("cockpitCameraMode")]
    public int CockpitCameraMode { get; init; }
    [JsonPropertyName("dashCameraMode")]
    public int DashCameraMode { get; init; }
    [JsonPropertyName("eventIndex")]
    public int EventIndex { get; init; }
    [JsonPropertyName("forcePlayerSwap")]
    public int ForcePlayerSwap { get; init; }
    [JsonPropertyName("freeJoinMode")]
    public int FreeJoinMode { get; init; }
    [JsonPropertyName("lagCount")]
    public int LagCount { get; init; }
    [JsonPropertyName("lagSecs")]
    public int LagSecs { get; init; }
    [JsonPropertyName("loopSeason")]
    public int LoopSeason { get; init; }
    [JsonPropertyName("maxPing")]
    public int MaxPing { get; init; }
    [JsonPropertyName("sessionIndex")]
    public int SessionIndex { get; init; }
    [JsonPropertyName("simulateAsOneManTime")]
    public int SimulateAsOneManTeam { get; init; }
    [JsonPropertyName("singleEvents")]
    public int SingleEvents { get; init; }
}