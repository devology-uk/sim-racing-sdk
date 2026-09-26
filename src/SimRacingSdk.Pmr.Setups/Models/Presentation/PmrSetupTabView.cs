namespace SimRacingSdk.Pmr.Setups.Models.Presentation;

public record PmrSetupTabView
{
    public required string Name { get; init; }
    public required IReadOnlyList<PmrSetupSectionView> Sections { get; init; }
}
