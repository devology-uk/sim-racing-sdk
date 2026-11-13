using SimRacingSdk.R3e.Setups.Models;

namespace SimRacingSdk.R3e.DataManager.SetupMaps;

// One file per car (data/r3e/setupmaps/{CarGameId}.json): the SDK's map plus what only the Data Manager needs.
public record CapturedSetupMap : R3eSetupMap
{
    // Only there so the file is readable; the car is identified by CarGameId.
    public required string CarName { get; init; }

    // Set once every row has been checked against the game's setup screen - only validated maps ship in the SDK.
    public bool Validated { get; init; }
}
