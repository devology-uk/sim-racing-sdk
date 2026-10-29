namespace SimRacingSdk.R3e.DataManager.Tracks;

public record TrackLayoutInfo
{
    // Owned by the game's r3e-data.json and refreshed by Import From Game. GameId is what shared
    // memory reports as LayoutId; GridSize is the layout's MaxNumberOfVehicles.
    public required int GameId { get; init; }
    public required int GridSize { get; init; }
    public required string Name { get; init; }

    // Copied from the game's layout tile, which shows it in km; null until captured.
    public double? LengthKm { get; init; }
}
