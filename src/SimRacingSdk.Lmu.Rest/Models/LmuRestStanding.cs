namespace SimRacingSdk.Lmu.Rest.Models;

// One car in watch/standings. Times are seconds, -1 when not set yet. Unlike shared memory, every car carries
// its fuel and virtual energy fractions.
public record LmuRestStanding
{
    public double BestLapTime { get; init; }
    public double BestSectorTime1 { get; init; }
    public double BestSectorTime2 { get; init; }
    public string CarClass { get; init; } = string.Empty;
    public string CarId { get; init; } = string.Empty;
    public string CarNumber { get; init; } = string.Empty;
    public string CountLapFlag { get; init; } = string.Empty;
    public string DriverName { get; init; } = string.Empty;
    public double EstimatedLapTime { get; init; }
    public string FinishStatus { get; init; } = string.Empty;
    public string Flag { get; init; } = string.Empty;
    public double FuelFraction { get; init; }
    public string FullTeamName { get; init; } = string.Empty;
    public bool HasFocus { get; init; }
    public bool InGarageStall { get; init; }
    public double LapDistance { get; init; }
    public int LapsBehindLeader { get; init; }
    public int LapsCompleted { get; init; }
    public double LastLapTime { get; init; }
    public int Penalties { get; init; }
    public string PitState { get; init; } = string.Empty;
    public int Pitstops { get; init; }
    public bool Pitting { get; init; }
    public bool Player { get; init; }
    public int Position { get; init; }
    public string Sector { get; init; } = string.Empty;
    public int SlotId { get; init; }
    public double TimeBehindLeader { get; init; }
    public double TimeBehindNext { get; init; }
    public double VeFraction { get; init; }
    public string VehicleFilename { get; init; } = string.Empty;
    public string VehicleName { get; init; } = string.Empty;
}
