using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.DataManager.Shared.Catalog;

namespace SimRacingSdk.Pmr.DataManager.Cars;

public partial class CarEditorViewModel : ObservableObject, ICatalogEditor<CarInfo>
{
    // Tracks what IconFileName was last auto-derived from Manufacturer, so typing a manufacturer
    // name keeps the icon in sync live - but the moment IconFileName no longer matches this (the
    // user typed their own override, e.g. "mercedes" for "Mercedes-AMG"), further Manufacturer
    // edits stop clobbering it.
    private string lastAutoIconFileName = string.Empty;

    [ObservableProperty]
    private string author = string.Empty;

    [ObservableProperty]
    private double engineDisplacementLitres;

    [ObservableProperty]
    private string engineLayout = string.Empty;

    [ObservableProperty]
    private string engineType = string.Empty;

    [ObservableProperty]
    private double? fuelCapacityLitres;

    [ObservableProperty]
    private string? gameId;

    [ObservableProperty]
    private string iconFileName = string.Empty;

    [ObservableProperty]
    private string manufacturer = string.Empty;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private double powerKw;

    [ObservableProperty]
    private string powertrainLayout = string.Empty;

    [ObservableProperty]
    private double torqueNm;

    [ObservableProperty]
    private string transmission = string.Empty;

    [ObservableProperty]
    private string vehicleClass = string.Empty;

    [ObservableProperty]
    private double weightKg;

    [ObservableProperty]
    private int year;

    public void LoadFrom(CarInfo? car)
    {
        this.Author = car?.Author ?? string.Empty;
        this.EngineDisplacementLitres = car?.EngineDisplacementLitres ?? 0;
        this.EngineLayout = car?.EngineLayout ?? string.Empty;
        this.EngineType = car?.EngineType ?? string.Empty;
        this.FuelCapacityLitres = car?.FuelCapacityLitres;
        this.GameId = car?.GameId;
        this.Manufacturer = car?.Manufacturer ?? string.Empty;
        this.IconFileName = car?.IconFileName ?? string.Empty;
        this.Name = car?.Name ?? string.Empty;
        this.PowerKw = car?.PowerKw ?? 0;
        this.PowertrainLayout = car?.PowertrainLayout ?? string.Empty;
        this.TorqueNm = car?.TorqueNm ?? 0;
        this.Transmission = car?.Transmission ?? string.Empty;
        this.VehicleClass = car?.VehicleClass ?? string.Empty;
        this.WeightKg = car?.WeightKg ?? 0;
        this.Year = car?.Year ?? DateTime.Now.Year;
    }

    public void StartInGroup(string group)
    {
        this.VehicleClass = group;
    }

    public CarInfo ToEntity()
    {
        return new CarInfo
        {
            Author = this.Author,
            EngineDisplacementLitres = this.EngineDisplacementLitres,
            EngineLayout = this.EngineLayout,
            EngineType = this.EngineType,
            FuelCapacityLitres = this.FuelCapacityLitres,
            GameId = this.GameId,
            IconFileName = this.IconFileName,
            Manufacturer = this.Manufacturer,
            Name = this.Name,
            PowerKw = this.PowerKw,
            PowertrainLayout = this.PowertrainLayout,
            TorqueNm = this.TorqueNm,
            Transmission = this.Transmission,
            VehicleClass = this.VehicleClass,
            WeightKg = this.WeightKg,
            Year = this.Year
        };
    }

    partial void OnManufacturerChanged(string value)
    {
        if(this.IconFileName == this.lastAutoIconFileName)
        {
            this.lastAutoIconFileName = DefaultIconFileName(value);
            this.IconFileName = this.lastAutoIconFileName;
        }
    }

    private static string DefaultIconFileName(string manufacturer)
    {
        return manufacturer.Replace(' ', '-').ToLowerInvariant();
    }
}
