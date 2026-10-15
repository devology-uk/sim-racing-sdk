using System.Text.Json.Serialization;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.Ams2.DataManager.Cars;

public record CarInfo : ICatalogEntity
{
    // Null means the default: Model with spaces as underscores. Only set it when AMS2's own id differs.
    public string? Ams2CarId { get; init; }

    public required string Class { get; init; }
    public required string Differential { get; init; }
    public required double DisplacementL { get; init; }

    // Null for base-game cars.
    public string? DlcPack { get; init; }

    public required string Driveline { get; init; }
    public required string Electronics { get; init; }
    public required string Engine { get; init; }
    public required string EnginePlacement { get; init; }
    public required bool HasAdjustableTurbo { get; init; }
    public required bool HasBoostButton { get; init; }
    public required bool HasDrs { get; init; }
    public required bool HasHeadlights { get; init; }
    public required bool HasOnBoardBrakeBias { get; init; }
    public required bool HasOnBoardRollBars { get; init; }
    public required bool HasPitSpeedLimiter { get; init; }
    public required string Manufacturer { get; init; }

    // Matches the vehicle name AMS2's shared memory reports, apart from BOP suffixes such as
    // " - Low Downforce" (see FindByRawVehicleName).
    public required string Model { get; init; }

    public required double PowerHp { get; init; }
    public required int TopSpeedKph { get; init; }
    public required double TorqueNm { get; init; }
    public required string Transmission { get; init; }

    // Front/rear percentages as the game shows them, e.g. "53.0/47.0".
    public required string WeightDistribution { get; init; }

    public required double WeightKg { get; init; }
    public required double WheelBaseM { get; init; }

    [JsonIgnore]
    public string DisplayName => this.Model;

    // Includes Class because some cars appear in two classes (e.g. Oreca 07 in LMP2 Gen 1 and Gen 2).
    [JsonIgnore]
    public string Id => Slug.Create($"{this.Model} {this.Class}");
}
