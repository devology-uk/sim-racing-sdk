namespace SimRacingSdk.Pmr.Setups.Models.Presentation;

// Two setups for the same car, each laid out in full, plus only the values that differ.
public record PmrSetupComparison
{
    public required IReadOnlyList<PmrSetupDifference> Differences { get; init; }
    public required PmrSetupView First { get; init; }
    public required PmrSetupView Second { get; init; }
}
