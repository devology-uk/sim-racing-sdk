namespace SimRacingSdk.Lmu.Setups.Models;

// One discovered .svm file plus the car identity resolved from its own //VEH= comment - LMU's
// Settings folder has no per-car subfolder, so identity can only come from opening the file itself.
public class LmuSetupFileInfo
{
    public required string TrackFolderName { get; init; }
    public required string FileName { get; init; }
    public required string CarIdentifier { get; init; }
    public required string CarDisplayName { get; init; }
    public required LmuSetupFile Setup { get; init; }
}
