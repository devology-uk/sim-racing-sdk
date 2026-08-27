namespace SimRacingSdk.Acc.Setups.Models;

// One discovered setup file's identity only - car/track/file name, no decoded content. Used for
// hierarchy enumeration (search lists, trees), where the file's actual setup data is never shown -
// decoding every file just to list it would be wasted work. GetSetupFile returns the full
// AccSetupFileInfo for a single selected file instead.
public class AccSetupFileIdentity
{
    public required string CarFolderName { get; init; }
    public required string TrackFolderName { get; init; }
    public required string CarDisplayName { get; init; }
    public required string TrackDisplayName { get; init; }
    public required string FileName { get; init; }
}
