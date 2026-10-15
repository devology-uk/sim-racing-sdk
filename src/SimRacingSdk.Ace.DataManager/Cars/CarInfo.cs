using System.Text.Json.Serialization;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.Ace.DataManager.Cars;

public record CarInfo : ICatalogEntity
{
    // Null for anything not electric or hybrid.
    public double? BatteryKwh { get; init; }

    public required string Body { get; init; }

    // ACE's car-select grouping, e.g. "Road:Modern:ICE".
    public required string Categories { get; init; }

    public int? Cylinders { get; init; }
    public double? DisplacementL { get; init; }
    public required string DriveLayout { get; init; }
    public required string Engine { get; init; }
    public required string EngineLayout { get; init; }
    public double? FuelTankL { get; init; }
    public int? Gears { get; init; }
    public required string Manufacturer { get; init; }
    public int? MaxRpm { get; init; }
    public required int MaxSpeedKmh { get; init; }

    // The preset id from the ACE Dedicated Server's cars.json (e.g. "preset_m4gt3_mech_1"). Not unique:
    // a few cars have two catalog rows sharing one ModelId, which is a real ACE limitation.
    public required string ModelId { get; init; }

    public required string Name { get; init; }
    public required double PerformanceRating { get; init; }
    public required int PowerPs { get; init; }

    // Empty for road cars.
    public required string RacingClass { get; init; }

    public SetupSchema SetupSchema { get; init; } = new();
    public required int TorqueNm { get; init; }
    public List<string> TyreCompounds { get; init; } = [];

    // Empty unless the same car comes in more than one form, e.g. a race and a road version.
    public required string Variant { get; init; }

    public required int WeightKg { get; init; }
    public required int Year { get; init; }
    public required double ZeroToOneHundredKmh { get; init; }

    [JsonIgnore]
    public string DisplayName =>
        string.IsNullOrEmpty(this.Variant)
            ? $"{this.Manufacturer} {this.Name}"
            : $"{this.Manufacturer} {this.Name} - {this.Variant}";

    [JsonIgnore]
    public string Id => Slug.Create($"{this.DisplayName} {this.Year}");
}
