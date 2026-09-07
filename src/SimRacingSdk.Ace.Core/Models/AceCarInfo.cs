#nullable disable

namespace SimRacingSdk.Ace.Core.Models;

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
    public string ModelId { get; init; }
    public string Name { get; init; }
    public double PerformanceRating { get; init; }
    public int PowerPs { get; init; }
    public string RacingClass { get; init; }
    public AceSetupSchema SetupSchema { get; init; } = new();
    public int TorqueNm { get; init; }
    public string[] TyreCompounds { get; init; } = [];
    public string Variant { get; init; }
    public int WeightKg { get; init; }
    public int Year { get; init; }
    public double ZeroToOneHundredKmh { get; init; }

    public string DisplayName =>
        string.IsNullOrEmpty(this.Variant)
            ? $"{this.Manufacturer} {this.Name}"
            : $"{this.Manufacturer} {this.Name} - {this.Variant}";

    // Matches the CarModel string Ace's shared memory actually reports (e.g. "BMW M4 GT3 Evo") -
    // unlike DisplayName, never includes Variant, since shared memory has no variant concept.
    public string AceName => $"{this.Manufacturer} {this.Name}";
}
