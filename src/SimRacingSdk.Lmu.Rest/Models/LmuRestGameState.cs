using System.Text.Json.Serialization;

namespace SimRacingSdk.Lmu.Rest.Models;

// sessions/GetGameState. Answers 503 in the main menu. With a replay loaded it still reported
// playerVehicleLoaded and !isReplayActive (2026-10-24), so it can't be trusted to say a car is on track.
public record LmuRestGameState
{
    [JsonPropertyName("closeestWeatherNode")]
    public LmuRestWeatherNode? ClosestWeatherNode { get; init; }

    public string ExitGarageState { get; init; } = string.Empty;
    public string GamePhase { get; init; } = string.Empty;
    public bool InControlOfVehicle { get; init; }
    public bool InMonitor { get; init; }
    public bool IsReplayActive { get; init; }
    public string MultiStintState { get; init; } = string.Empty;
    public double PitEntryDist { get; init; }
    public string PitState { get; init; } = string.Empty;
    public bool PlayerVehicleLoaded { get; init; }
    public bool RaceFinished { get; init; }
    public string TeamVehicleState { get; init; } = string.Empty;
    public double TimeOfDay { get; init; }
}
