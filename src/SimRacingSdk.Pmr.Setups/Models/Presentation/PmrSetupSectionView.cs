namespace SimRacingSdk.Pmr.Setups.Models.Presentation;

public record PmrSetupSectionView
{
    public required IReadOnlyList<PmrSetupFieldView> Fields { get; init; }
    public required string Name { get; init; }
}
