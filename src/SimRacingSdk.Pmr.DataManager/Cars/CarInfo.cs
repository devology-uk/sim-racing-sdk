using System.Text.Json.Serialization;

namespace SimRacingSdk.Pmr.DataManager.Cars;

public record CarInfo
{
    // Which mod author/studio made the car - currently the same value ("Straight 4 Studios") for
    // every car since the game only ships its own content so far, but PMR supports mods.
    public required string Author { get; init; }

    public required double EngineDisplacementLitres { get; init; }

    // Rear/Front/Mid Engine - separate from PowertrainLayout since the game's own car screen shows
    // them as two distinct facts under one "Layout" heading, not one combined string.
    public required string EngineLayout { get; init; }

    // Cylinder configuration as the game names it - "V6", "V6 Twin Turbo" etc.
    public required string EngineType { get; init; }

    // Null for cars regulated by an energy allocation rather than a simple tank size (Hypercar/
    // LMDh) rather than a genuinely missing value - matters for endurance-race fuel calculations.
    public double? FuelCapacityLitres { get; init; }

    // The game's own vehicle id (e.g. "ID_BMW_M_Hybrid_V8"), not shown in-game - found in
    // Documents\My Games\ProjectMotorRacing\savegame1\stats.xml. Needed to match a saved setup's
    // <vehicleId>-<setupName>.vset filename to a car. Null until looked up for a newly added car.
    public string? GameId { get; init; }

    // Matches CarManufacturerImageSourceConverter's own lookup rule (lowercase, spaces to hyphens)
    // against SimRacingSdk.Wpf.Shared/Images/Manufacturers - usually just the lowercased
    // Manufacturer, overridden where a manufacturer trades under more than one name but shares one
    // logo asset (e.g. "Mercedes-AMG"/"Mercedes-Benz" both use "mercedes.png"). The editor defaults
    // this from Manufacturer automatically; overriding it for cases like that is a manual, per-game
    // judgement call for whoever's curating that game's data.
    public required string IconFileName { get; init; }

    public required string Manufacturer { get; init; }
    public required string Name { get; init; }
    public required double PowerKw { get; init; }

    // RWD/FWD/AWD - the drivetrain, as distinct from EngineLayout above.
    public required string PowertrainLayout { get; init; }

    public required double TorqueNm { get; init; }
    public required string Transmission { get; init; }
    public required string VehicleClass { get; init; }
    public required double WeightKg { get; init; }
    public required int Year { get; init; }

    [JsonIgnore]
    public string DisplayName => $"{this.Manufacturer} {this.Name}";

    // The JSON file's name, derived from Manufacturer + Name + Year - includes Year because the old
    // PmrCarInfoProvider catalog already has two cars sharing a Manufacturer + Name (the 2024 and
    // 1969 Chevrolet Camaro, different eras of the same nameplate), which would otherwise collide.
    [JsonIgnore]
    public string Id => Slug.Create($"{this.Manufacturer} {this.Name} {this.Year}");
}
