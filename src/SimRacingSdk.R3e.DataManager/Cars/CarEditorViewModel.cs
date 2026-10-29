using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.DataManager.Shared.Catalog;

namespace SimRacingSdk.R3e.DataManager.Cars;

public partial class CarEditorViewModel : ObservableObject, ICatalogEditor<CarInfo>
{
    [ObservableProperty]
    private string carClass = string.Empty;

    [ObservableProperty]
    private string drivetrain = string.Empty;

    [ObservableProperty]
    private double? engineCapacityLitres;

    [ObservableProperty]
    private string engineLayout = string.Empty;

    [ObservableProperty]
    private int gameId;

    [ObservableProperty]
    private string manufacturer = string.Empty;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private bool notInGameSelector;

    [ObservableProperty]
    private int? powerBhp;

    [ObservableProperty]
    private int? weightWithDriverKg;

    public void LoadFrom(CarInfo? car)
    {
        this.CarClass = car?.Class ?? string.Empty;
        // A combo box can't select an empty value, so it would go on showing the previous car's drive without
        // this ever holding it.
        this.Drivetrain = string.IsNullOrEmpty(car?.Drivetrain) ? Drivetrains.Default : car.Drivetrain;
        this.EngineCapacityLitres = car?.EngineCapacityLitres;
        this.EngineLayout = car?.EngineLayout ?? string.Empty;
        this.GameId = car?.GameId ?? 0;
        this.Manufacturer = car?.Manufacturer ?? string.Empty;
        this.Name = car?.Name ?? string.Empty;
        this.NotInGameSelector = car?.NotInGameSelector ?? false;
        this.PowerBhp = car?.PowerBhp;
        this.WeightWithDriverKg = car?.WeightWithDriverKg;
    }

    public void CopySpecFrom(CarInfo donor)
    {
        this.Drivetrain = string.IsNullOrEmpty(donor.Drivetrain) ? Drivetrains.Default : donor.Drivetrain;
        this.EngineCapacityLitres = donor.EngineCapacityLitres;
        this.EngineLayout = donor.EngineLayout;
        this.PowerBhp = donor.PowerBhp;
        this.WeightWithDriverKg = donor.WeightWithDriverKg;
    }

    public void StartInGroup(string group)
    {
        this.CarClass = group;
    }

    public CarInfo ToEntity()
    {
        return new CarInfo
        {
            Class = this.CarClass,
            Drivetrain = this.Drivetrain,
            EngineCapacityLitres = this.EngineCapacityLitres,
            EngineLayout = this.EngineLayout.Trim(),
            GameId = this.GameId,
            Manufacturer = this.Manufacturer,
            Name = this.Name,
            NotInGameSelector = this.NotInGameSelector,
            PowerBhp = this.PowerBhp,
            WeightWithDriverKg = this.WeightWithDriverKg
        };
    }
}
