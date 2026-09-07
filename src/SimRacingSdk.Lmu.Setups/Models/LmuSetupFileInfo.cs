namespace SimRacingSdk.Lmu.Setups.Models;

public class LmuSetupFileInfo
{
    public required string TrackFolderName { get; init; }
    public required string FileName { get; init; }
    public required string CarIdentifier { get; init; }
    public required string CarDisplayName { get; init; }
    public required LmuSetupFile Setup { get; init; }
}
