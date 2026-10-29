#nullable disable

namespace SimRacingSdk.R3e.Core.Models;

public record R3eCarInfo
{
    public string Class { get; init; }
    // As the game shows it, e.g. "RWD".
    public string Drivetrain { get; init; }
    // The game shows the engine as "1.8L Inline-4": capacity 1.8, layout "Inline-4".
    public double? EngineCapacityLitres { get; init; }
    public string EngineLayout { get; init; }
    // The game's own model id, as shared memory and results files report it.
    public int GameId { get; init; }
    public string Manufacturer { get; init; }
    public string Name { get; init; }
    // Known to the game but not offered in its car selector (esports, safety cars, retired copies).
    public bool NotInGameSelector { get; init; }
    public int? PowerBhp { get; init; }
    public int? WeightWithDriverKg { get; init; }
}
