#nullable disable

namespace SimRacingSdk.Ace.Core.Models;

// Sourced from Mike's own manually-gathered ace-cars.csv (repo root, populated 2026-07-28 from
// Evo's in-game car list) - far richer than the earlier cars.json-derived {Name, DisplayName}
// shape. ModelId is not yet known for any car (Evo's UI/files don't surface it) so it is null
// until Mike finds a way to map it - see AceCarInfoProvider.FindByModelId.
public record AceCarInfo
{
    public string Body { get; init; }
    public double? BatteryKwh { get; init; }
    public string Categories { get; init; }
    public int? Cylinders { get; init; }
    public double? DisplacementL { get; init; }
    public string DriveLayout { get; init; }
    public string Engine { get; init; }
    public string EngineLayout { get; init; }
    public double? FuelTankL { get; init; }
    public int? Gears { get; init; }
    public string Manufacturer { get; init; }
    public int? MaxRpm { get; init; }
    public int MaxSpeedKmh { get; init; }
    public int? ModelId { get; init; }
    public string Name { get; init; }
    public double PerformanceRating { get; init; }
    public int PowerPs { get; init; }
    public string RacingClass { get; init; }
    public int TorqueNm { get; init; }
    public string Variant { get; init; }
    public int WeightKg { get; init; }
    public int Year { get; init; }
    public double ZeroToOneHundredKmh { get; init; }

    public string DisplayName =>
        string.IsNullOrEmpty(this.Variant)
            ? $"{this.Manufacturer} {this.Name}"
            : $"{this.Manufacturer} {this.Name} - {this.Variant}";
}
