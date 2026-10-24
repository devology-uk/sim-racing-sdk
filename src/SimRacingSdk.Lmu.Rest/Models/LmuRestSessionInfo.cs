namespace SimRacingSdk.Lmu.Rest.Models;

// watch/sessionInfo. Answers in every state, with an empty body in the main menu.
public record LmuRestSessionInfo
{
    public double AmbientTemp { get; init; }
    public double AveragePathWetness { get; init; }
    public double CurrentEventTime { get; init; }
    public double DarkCloud { get; init; }
    public double EndEventTime { get; init; }
    public int GamePhase { get; init; }
    public bool InRealtime { get; init; }
    public double LapDistance { get; init; }
    public long MaximumLaps { get; init; }
    public int NumberOfVehicles { get; init; }
    public string PlayerName { get; init; } = string.Empty;
    public double Raining { get; init; }
    public IReadOnlyList<string> SectorFlag { get; init; } = [];
    public string ServerName { get; init; } = string.Empty;
    public string Session { get; init; } = string.Empty;
    public double TimeRemainingInGamePhase { get; init; }
    public string TrackName { get; init; } = string.Empty;
    public double TrackTemp { get; init; }
    public string YellowFlagState { get; init; } = string.Empty;
}
