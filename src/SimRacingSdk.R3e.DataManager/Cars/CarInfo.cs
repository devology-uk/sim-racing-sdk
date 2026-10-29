using System.Text.Json.Serialization;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.R3e.DataManager.Cars;

public record CarInfo : ICatalogEntity
{
    // Owned by the game's r3e-data.json and refreshed by Import From Game.
    public required string Class { get; init; }

    // The game's own model id - what shared memory reports as ModelId and a results file as carId.
    public required int GameId { get; init; }

    public required string Manufacturer { get; init; }

    // Not unique: the same car appears in several classes and seasons (e.g. BMW M4 GT3 DTM in DTM 2023 and 2024).
    public required string Name { get; init; }

    // Copied from the info line the game shows when a car is selected; null or empty until captured.
    // The model year isn't shown there, but often appears in the class name.

    // One of Drivetrains.All, e.g. "RWD".
    public string Drivetrain { get; init; } = string.Empty;

    // The game shows the engine as "1.8L Inline-4": this is the 1.8.
    public double? EngineCapacityLitres { get; init; }

    // ... and this is the Inline-4.
    public string EngineLayout { get; init; } = string.Empty;

    // In r3e-data.json but not in the game's car selector (esports, safety cars, retired copies), so it has no
    // info line to copy. Kept, not deleted: Import From Game would bring it back, and shared memory can still report it.
    public bool NotInGameSelector { get; init; }

    public int? PowerBhp { get; init; }
    public int? WeightWithDriverKg { get; init; }

    [JsonIgnore]
    public string DisplayName => this.Name;

    [JsonIgnore]
    public string Id => Slug.Create($"{this.Name} {this.GameId}");
}
