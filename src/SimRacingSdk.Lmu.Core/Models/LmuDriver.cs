namespace SimRacingSdk.Lmu.Core.Models;

public record LmuDriver
{
    public IList<LmuDriverLap> Laps { get; } = new List<LmuDriverLap>();
    public double BestLap { get; init; }
    public string? CarClass { get; init; }
    public int CarNumber { get; init; }
    public string? CarType { get; init; }
    public string? Category { get; init; }
    public int ClassGridPosition { get; init; }
    public int ClassPosition { get; init; }
    public int Connected { get; init; }
    public LmuControlAndAids? ControlAndAids { get; set; }
    public string? FinishStatus { get; init; }
    public double FinishTime { get; init; }
    public int GridPosition { get; init; }
    public int IsPlayer { get; init; }
    public int LapCount { get; init; }
    public int LapRankIncludingDiscos { get; init; }
    public string? Name { get; init; }
    public int PitStops { get; init; }
    public int Position { get; init; }
    public int ServerScored { get; init; }
    public string? TeamName { get; init; }
    public string? UpgradeCode { get; init; }
    public string? VehicleFile { get; init; }
    public string? VehicleName { get; init; }
}