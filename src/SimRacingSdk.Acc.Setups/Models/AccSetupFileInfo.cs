namespace SimRacingSdk.Acc.Setups.Models;

// One discovered setup file, already parsed and decoded to real units via the car's AccSetupMap.
public class AccSetupFileInfo
{
    public required string CarFolderName { get; init; }
    public required string TrackFolderName { get; init; }
    public required string CarDisplayName { get; init; }
    public required string TrackDisplayName { get; init; }
    public required string FileName { get; init; }
    public required AccDecodedSetup DecodedSetup { get; init; }
}
