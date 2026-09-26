namespace SimRacingSdk.Pmr.Setups.Models.Presentation;

public record PmrSetupFieldView
{
    public required string Name { get; init; }
    public required IReadOnlyList<PmrSetupValueView> Values { get; init; }
}
