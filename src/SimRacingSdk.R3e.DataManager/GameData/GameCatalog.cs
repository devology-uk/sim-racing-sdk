namespace SimRacingSdk.R3e.DataManager.GameData;

// What the game's own r3e-data.json says about its content: ids and names only, no specs.
public record GameCatalog
{
    public required IReadOnlyList<GameCar> Cars { get; init; }
    public required IReadOnlyList<GameTrack> Tracks { get; init; }
}

public record GameCar
{
    public required string Class { get; init; }
    public required int GameId { get; init; }
    public required string Manufacturer { get; init; }
    public required string Name { get; init; }
}

public record GameTrack
{
    public required int GameId { get; init; }
    public required IReadOnlyList<GameLayout> Layouts { get; init; }
    public required string Name { get; init; }
}

public record GameLayout
{
    public required int GameId { get; init; }
    public required int GridSize { get; init; }
    public required string Name { get; init; }
}
