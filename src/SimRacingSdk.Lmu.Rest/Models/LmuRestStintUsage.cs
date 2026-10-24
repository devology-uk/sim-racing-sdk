namespace SimRacingSdk.Lmu.Rest.Models;

// One entry per driver per stint in strategy/usage, keyed by driver name. Fuel and Tyres are only present for
// the player; Ve (0-1) is present for every driver. Tyres are percentages, FL, FR, RL, RR.
public record LmuRestStintUsage
{
    public double? Fuel { get; init; }
    public int Lap { get; init; }
    public bool Pit { get; init; }
    public int Stint { get; init; }
    public IReadOnlyList<double>? Tyres { get; init; }
    public double Ve { get; init; }
}
