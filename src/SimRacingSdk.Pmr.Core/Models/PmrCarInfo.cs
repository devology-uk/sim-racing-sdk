#nullable disable

namespace SimRacingSdk.Pmr.Core.Models;

public record PmrCarInfo
{
    public string ClassString { get; init; }
    public string DescriptionKey { get; init; }
    public string EngineName { get; init; }

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

    public string Id { get; init; }
    public string Layout { get; init; }
    public string Manufacturer { get; init; }
    public string Name { get; init; }
    public double PowerBhp { get; init; }
    public string Region { get; init; }
    public double TorqueNm { get; init; }
    public string Transmission { get; init; }
    public string VehicleClass { get; init; }
    public string VehiclePath { get; init; }
    public double WeightKg { get; init; }
    public int Year { get; init; }

    public string DisplayName => $"{this.Manufacturer} {this.Name}";
}
