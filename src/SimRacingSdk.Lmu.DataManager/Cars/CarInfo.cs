using System.Text.Json.Serialization;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.Lmu.DataManager.Cars;

public record CarInfo : ICatalogEntity
{
    // The regulation the car is built to (LMH, LMDh, LMGT3, GTE, LMP2, LMP3) - SRT's fuel
    // calculator decides whether Virtual Energy applies from this.
    public required string Category { get; init; }

    // The game's own class code (HY, GT3, GTE, P2, P3), as its car-select screen groups them.
    public required string Class { get; init; }

    public required string DisplayName { get; init; }
    public required string Engine { get; init; }
    public required int HeightMm { get; init; }
    public required int LengthMm { get; init; }
    public required string Manufacturer { get; init; }

    // Litres, whole number.
    public required int MaxFuel { get; init; }

    // Engine rev limiter RPM.
    public required int MaxRpm { get; init; }

    // The vehicle-folder segment from a setup file's //VEH= comment (e.g. "911GT3R_2024"). Null
    // until a setup file for the car has been seen.
    public string? ModelId { get; init; }

    public required int PowerBhp { get; init; }
    public required int PowerKw { get; init; }

    // The CarType a result file reports - doesn't always match DisplayName.
    public required string ResultCarType { get; init; }

    public required string Transmission { get; init; }
    public required int WeightKg { get; init; }
    public required int WidthMm { get; init; }

    // The model year as the game represents the car, not necessarily its real-world homologation year.
    public required int Year { get; init; }

    [JsonIgnore]
    public string Id => Slug.Create(this.DisplayName);
}
