namespace SimRacingSdk.R3e.Setups.Models.Files;

// A saved setup at CarSetups\<track>\<layout>\<file>.svm. The folders use the game's display names.
public record R3eSetupFileInfo
{
    public required DateTime CreatedAt { get; init; }
    public required string LayoutFolderName { get; init; }
    public required DateTime ModifiedAt { get; init; }
    public required R3eSetupFileName Name { get; init; }
    public required string TrackFolderName { get; init; }

    public string RelativePath => Path.Combine(this.TrackFolderName, this.LayoutFolderName, this.Name.FileName);
}
