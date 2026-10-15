using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.DataManager.Shared.Catalog;

namespace SimRacingSdk.Ams2.DataManager.Cars;

public partial class CarEditorViewModel : ObservableObject, ICatalogEditor<CarInfo>
{
    [ObservableProperty]
    private string? ams2CarId;

    [ObservableProperty]
    private string carClass = string.Empty;

    [ObservableProperty]
    private string differential = string.Empty;

    [ObservableProperty]
    private double displacementL;

    [ObservableProperty]
    private string? dlcPack;

    [ObservableProperty]
    private string driveline = string.Empty;

    [ObservableProperty]
    private string electronics = string.Empty;

    [ObservableProperty]
    private string engine = string.Empty;

    [ObservableProperty]
    private string enginePlacement = string.Empty;

    [ObservableProperty]
    private bool hasAdjustableTurbo;

    [ObservableProperty]
    private bool hasBoostButton;

    [ObservableProperty]
    private bool hasDrs;

    [ObservableProperty]
    private bool hasHeadlights;

    [ObservableProperty]
    private bool hasOnBoardBrakeBias;

    [ObservableProperty]
    private bool hasOnBoardRollBars;

    [ObservableProperty]
    private bool hasPitSpeedLimiter;

    [ObservableProperty]
    private string manufacturer = string.Empty;

    [ObservableProperty]
    private string model = string.Empty;

    [ObservableProperty]
    private double powerHp;

    [ObservableProperty]
    private int topSpeedKph;

    [ObservableProperty]
    private double torqueNm;

    [ObservableProperty]
    private string transmission = string.Empty;

    [ObservableProperty]
    private string weightDistribution = string.Empty;

    [ObservableProperty]
    private double weightKg;

    [ObservableProperty]
    private double wheelBaseM;

    public void LoadFrom(CarInfo? car)
    {
        this.Ams2CarId = car?.Ams2CarId;
        this.CarClass = car?.Class ?? string.Empty;
        this.Differential = car?.Differential ?? string.Empty;
        this.DisplacementL = car?.DisplacementL ?? 0;
        this.DlcPack = car?.DlcPack;
        this.Driveline = car?.Driveline ?? string.Empty;
        this.Electronics = car?.Electronics ?? string.Empty;
        this.Engine = car?.Engine ?? string.Empty;
        this.EnginePlacement = car?.EnginePlacement ?? string.Empty;
        this.HasAdjustableTurbo = car?.HasAdjustableTurbo ?? false;
        this.HasBoostButton = car?.HasBoostButton ?? false;
        this.HasDrs = car?.HasDrs ?? false;
        this.HasHeadlights = car?.HasHeadlights ?? false;
        this.HasOnBoardBrakeBias = car?.HasOnBoardBrakeBias ?? false;
        this.HasOnBoardRollBars = car?.HasOnBoardRollBars ?? false;
        this.HasPitSpeedLimiter = car?.HasPitSpeedLimiter ?? false;
        this.Manufacturer = car?.Manufacturer ?? string.Empty;
        this.Model = car?.Model ?? string.Empty;
        this.PowerHp = car?.PowerHp ?? 0;
        this.TopSpeedKph = car?.TopSpeedKph ?? 0;
        this.TorqueNm = car?.TorqueNm ?? 0;
        this.Transmission = car?.Transmission ?? string.Empty;
        this.WeightDistribution = car?.WeightDistribution ?? string.Empty;
        this.WeightKg = car?.WeightKg ?? 0;
        this.WheelBaseM = car?.WheelBaseM ?? 0;
    }

    public void StartInGroup(string group)
    {
        this.CarClass = group;
    }

    public CarInfo ToEntity()
    {
        return new CarInfo
        {
            Ams2CarId = NullIfBlank(this.Ams2CarId),
            Class = this.CarClass,
            Differential = this.Differential,
            DisplacementL = this.DisplacementL,
            DlcPack = NullIfBlank(this.DlcPack),
            Driveline = this.Driveline,
            Electronics = this.Electronics,
            Engine = this.Engine,
            EnginePlacement = this.EnginePlacement,
            HasAdjustableTurbo = this.HasAdjustableTurbo,
            HasBoostButton = this.HasBoostButton,
            HasDrs = this.HasDrs,
            HasHeadlights = this.HasHeadlights,
            HasOnBoardBrakeBias = this.HasOnBoardBrakeBias,
            HasOnBoardRollBars = this.HasOnBoardRollBars,
            HasPitSpeedLimiter = this.HasPitSpeedLimiter,
            Manufacturer = this.Manufacturer,
            Model = this.Model,
            PowerHp = this.PowerHp,
            TopSpeedKph = this.TopSpeedKph,
            TorqueNm = this.TorqueNm,
            Transmission = this.Transmission,
            WeightDistribution = this.WeightDistribution,
            WeightKg = this.WeightKg,
            WheelBaseM = this.WheelBaseM
        };
    }

    private static string? NullIfBlank(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
