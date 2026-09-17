#nullable disable

namespace SimRacingSdk.Pmr.Core.Models;

public record PmrCarInfo
{
    public double EngineDisplacementLitres { get; init; }

    // Front/Mid/Rear Engine - separate from PowertrainLayout (the drivetrain) since the game's
    // own car-select screen shows them as two distinct facts, not one combined string.
    public string EngineLayout { get; init; }

    // Cylinder configuration as the game names it - "V8", "Straight 6", "Flat-6", "4-Rotor" etc.
    public string EngineType { get; init; }

    // Null when the game's own vdef and vset files carry no fuel-tank spec at all - seen only
    // on the Hypercar/LMDh class, which is regulated by an energy allocation rather than a
    // simple litres figure (Mike's steer, matching LMU's Virtual Energy concept). Every other
    // "0" in the vdef's own FuelCapacityLitres field is backed by a real value in that car's
    // default.vset (key "ice-fuel-capacity"), which PmrCatalogImport already falls back to.
    public double? FuelCapacityLitres { get; init; }

    // Usually equal to Manufacturer - overridden where a manufacturer trades under more than
    // one name for logo purposes (e.g. "Mercedes-AMG"/"Mercedes-Benz" both use the one generic
    // Mercedes logo), so consumers can bind straight to this instead of Manufacturer for the
    // CarManufacturerImageSourceConverter lookup.
    public string IconFileName { get; init; }

    // Derived from Manufacturer + Name, not a game-internal identifier - pmr-cars.csv is the
    // catalog's source of truth and carries no ID column of its own (see PmrCatalogImport).
    public string Id { get; init; }

    public string Manufacturer { get; init; }
    public string Name { get; init; }
    public double PowerKw { get; init; }

    // FWD/RWD/4WD - the drivetrain, as distinct from EngineLayout above.
    public string PowertrainLayout { get; init; }

    public double TorqueNm { get; init; }
    public string Transmission { get; init; }
    public string VehicleClass { get; init; }
    public double WeightKg { get; init; }
    public int Year { get; init; }

    public string DisplayName => $"{this.Manufacturer} {this.Name}";
}
