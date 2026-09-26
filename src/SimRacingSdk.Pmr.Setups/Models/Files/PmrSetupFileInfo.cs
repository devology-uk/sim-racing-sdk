namespace SimRacingSdk.Pmr.Setups.Models.Files;

public record PmrSetupFileInfo
{
    public required DateTime CreatedAt { get; init; }
    public required DateTime ModifiedAt { get; init; }
    public required PmrSetupFileName Name { get; init; }
}
