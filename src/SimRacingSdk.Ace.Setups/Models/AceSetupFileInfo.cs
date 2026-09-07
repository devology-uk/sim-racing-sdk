using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Setups.Models;

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
