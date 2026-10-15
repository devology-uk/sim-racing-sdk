using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.DataManager.Shared.Catalog;

namespace SimRacingSdk.Ace.DataManager.Cars;

public partial class CarEditorViewModel : ObservableObject, ICatalogEditor<CarInfo>
{
    [ObservableProperty]
    private double? batteryKwh;

    [ObservableProperty]
    private string body = string.Empty;

    [ObservableProperty]
    private string categories = string.Empty;

    [ObservableProperty]
    private int? cylinders;

    [ObservableProperty]
    private double? displacementL;

    [ObservableProperty]
    private string driveLayout = string.Empty;

    [ObservableProperty]
    private string engine = string.Empty;

    [ObservableProperty]
    private string engineLayout = string.Empty;

    [ObservableProperty]
    private double? fuelTankL;

    [ObservableProperty]
    private int? gears;

    [ObservableProperty]
    private string manufacturer = string.Empty;

    [ObservableProperty]
    private int? maxRpm;

    [ObservableProperty]
    private int maxSpeedKmh;

    [ObservableProperty]
    private string modelId = string.Empty;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private double performanceRating;

    [ObservableProperty]
    private int powerPs;

    [ObservableProperty]
    private string racingClass = string.Empty;

    // The checkboxes bind straight into this working copy.
    [ObservableProperty]
    private SetupSchema setupSchema = new();

    [ObservableProperty]
    private int torqueNm;

    // Comma separated, in the order the game lists them.
    [ObservableProperty]
    private string tyreCompounds = string.Empty;

    [ObservableProperty]
    private string variant = string.Empty;

    [ObservableProperty]
    private int weightKg;

    [ObservableProperty]
    private int year;

    [ObservableProperty]
    private double zeroToOneHundredKmh;

    public IReadOnlyList<TyreCompoundMode> TyreCompoundModes { get; } = Enum.GetValues<TyreCompoundMode>();

    public void LoadFrom(CarInfo? car)
    {
        this.BatteryKwh = car?.BatteryKwh;
        this.Body = car?.Body ?? string.Empty;
        this.Categories = car?.Categories ?? string.Empty;
        this.Cylinders = car?.Cylinders;
        this.DisplacementL = car?.DisplacementL;
        this.DriveLayout = car?.DriveLayout ?? string.Empty;
        this.Engine = car?.Engine ?? string.Empty;
        this.EngineLayout = car?.EngineLayout ?? string.Empty;
        this.FuelTankL = car?.FuelTankL;
        this.Gears = car?.Gears;
        this.Manufacturer = car?.Manufacturer ?? string.Empty;
        this.MaxRpm = car?.MaxRpm;
        this.MaxSpeedKmh = car?.MaxSpeedKmh ?? 0;
        this.ModelId = car?.ModelId ?? string.Empty;
        this.Name = car?.Name ?? string.Empty;
        this.PerformanceRating = car?.PerformanceRating ?? 0;
        this.PowerPs = car?.PowerPs ?? 0;
        this.RacingClass = car?.RacingClass ?? string.Empty;
        this.SetupSchema = car?.SetupSchema.Clone() ?? new SetupSchema();
        this.TorqueNm = car?.TorqueNm ?? 0;
        this.TyreCompounds = string.Join(", ", car?.TyreCompounds ?? []);
        this.Variant = car?.Variant ?? string.Empty;
        this.WeightKg = car?.WeightKg ?? 0;
        this.Year = car?.Year ?? DateTime.Now.Year;
        this.ZeroToOneHundredKmh = car?.ZeroToOneHundredKmh ?? 0;
    }

    public void StartInGroup(string group)
    {
        this.Categories = group;
    }

    public CarInfo ToEntity()
    {
        return new CarInfo
        {
            BatteryKwh = this.BatteryKwh,
            Body = this.Body,
            Categories = this.Categories,
            Cylinders = this.Cylinders,
            DisplacementL = this.DisplacementL,
            DriveLayout = this.DriveLayout,
            Engine = this.Engine,
            EngineLayout = this.EngineLayout,
            FuelTankL = this.FuelTankL,
            Gears = this.Gears,
            Manufacturer = this.Manufacturer,
            MaxRpm = this.MaxRpm,
            MaxSpeedKmh = this.MaxSpeedKmh,
            ModelId = this.ModelId,
            Name = this.Name,
            PerformanceRating = this.PerformanceRating,
            PowerPs = this.PowerPs,
            RacingClass = this.RacingClass,
            SetupSchema = this.SetupSchema.Clone(),
            TorqueNm = this.TorqueNm,
            TyreCompounds = this.TyreCompounds.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList(),
            Variant = this.Variant,
            WeightKg = this.WeightKg,
            Year = this.Year,
            ZeroToOneHundredKmh = this.ZeroToOneHundredKmh
        };
    }
}
