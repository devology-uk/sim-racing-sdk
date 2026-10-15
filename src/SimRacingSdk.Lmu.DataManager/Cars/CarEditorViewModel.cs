using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.DataManager.Shared.Catalog;

namespace SimRacingSdk.Lmu.DataManager.Cars;

public partial class CarEditorViewModel : ObservableObject, ICatalogEditor<CarInfo>
{
    [ObservableProperty]
    private string category = string.Empty;

    [ObservableProperty]
    private string carClass = string.Empty;

    [ObservableProperty]
    private string displayName = string.Empty;

    [ObservableProperty]
    private string engine = string.Empty;

    [ObservableProperty]
    private int heightMm;

    [ObservableProperty]
    private int lengthMm;

    [ObservableProperty]
    private string manufacturer = string.Empty;

    [ObservableProperty]
    private int maxFuel;

    [ObservableProperty]
    private int maxRpm;

    [ObservableProperty]
    private string? modelId;

    [ObservableProperty]
    private int powerBhp;

    [ObservableProperty]
    private int powerKw;

    [ObservableProperty]
    private string resultCarType = string.Empty;

    [ObservableProperty]
    private string transmission = string.Empty;

    [ObservableProperty]
    private int weightKg;

    [ObservableProperty]
    private int widthMm;

    [ObservableProperty]
    private int year;

    public void LoadFrom(CarInfo? car)
    {
        this.Category = car?.Category ?? string.Empty;
        this.CarClass = car?.Class ?? string.Empty;
        this.DisplayName = car?.DisplayName ?? string.Empty;
        this.Engine = car?.Engine ?? string.Empty;
        this.HeightMm = car?.HeightMm ?? 0;
        this.LengthMm = car?.LengthMm ?? 0;
        this.Manufacturer = car?.Manufacturer ?? string.Empty;
        this.MaxFuel = car?.MaxFuel ?? 0;
        this.MaxRpm = car?.MaxRpm ?? 0;
        this.ModelId = car?.ModelId;
        this.PowerBhp = car?.PowerBhp ?? 0;
        this.PowerKw = car?.PowerKw ?? 0;
        this.ResultCarType = car?.ResultCarType ?? string.Empty;
        this.Transmission = car?.Transmission ?? string.Empty;
        this.WeightKg = car?.WeightKg ?? 0;
        this.WidthMm = car?.WidthMm ?? 0;
        this.Year = car?.Year ?? DateTime.Now.Year;
    }

    public void StartInGroup(string group)
    {
        this.CarClass = group;
    }

    public CarInfo ToEntity()
    {
        return new CarInfo
        {
            Category = this.Category,
            Class = this.CarClass,
            DisplayName = this.DisplayName,
            Engine = this.Engine,
            HeightMm = this.HeightMm,
            LengthMm = this.LengthMm,
            Manufacturer = this.Manufacturer,
            MaxFuel = this.MaxFuel,
            MaxRpm = this.MaxRpm,
            ModelId = string.IsNullOrWhiteSpace(this.ModelId) ? null : this.ModelId,
            PowerBhp = this.PowerBhp,
            PowerKw = this.PowerKw,
            ResultCarType = this.ResultCarType,
            Transmission = this.Transmission,
            WeightKg = this.WeightKg,
            WidthMm = this.WidthMm,
            Year = this.Year
        };
    }
}
