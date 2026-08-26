using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Setups.Models;

// One discovered .carsetup file plus the car/track identity resolved for it. ACE's setup folders
// are named after the car's display name, not a stable internal id, so CarInfo is resolved from
// the file's own embedded preset string rather than trusted from the folder name - see
// AceSetupProvider.BuildFileInfo.
public class AceSetupFileInfo
{
    public required string CarFolderName { get; init; }
    public required string TrackFolderName { get; init; }
    public required string TrackDisplayName { get; init; }
    public required string FileName { get; init; }
    public required string CarDisplayName { get; init; }
    public AceCarInfo? CarInfo { get; init; }
    public required AceRawCarSetup RawSetup { get; init; }
}
