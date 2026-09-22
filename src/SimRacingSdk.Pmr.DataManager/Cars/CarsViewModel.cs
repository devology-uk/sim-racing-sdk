using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimRacingSdk.Pmr.DataManager.Cars;

public partial class CarsViewModel : ObservableObject
{
    private readonly ICarRepository carRepository;
    private readonly List<CarInfo> cars;
    private readonly IPmrCarProviderGenerator pmrCarProviderGenerator;

    [ObservableProperty]
    private CarBrowseMode browseMode = CarBrowseMode.ByClass;

    [ObservableProperty]
    private string generateStatusMessage = string.Empty;

    [ObservableProperty]
    private CarInfo? selectedCar;

    [ObservableProperty]
    private string? selectedClass;

    public CarsViewModel(ICarRepository carRepository, IPmrCarProviderGenerator pmrCarProviderGenerator)
    {
        this.carRepository = carRepository;
        this.pmrCarProviderGenerator = pmrCarProviderGenerator;
        this.cars = this.carRepository.GetAll().ToList();
        this.RefreshClasses();
        this.SelectedClass = this.Classes.FirstOrDefault();
    }

    // Not a fixed list - derived from whatever classes are actually present in the saved cars, so a
    // new DLC class shows up here the moment a car using it is saved, with no code change needed.
    public ObservableCollection<string> Classes { get; } = [];

    public CarEditorViewModel Editor { get; } = new();
    public bool IsByClassMode => this.BrowseMode == CarBrowseMode.ByClass;
    public ObservableCollection<CarInfo> VisibleCars { get; } = [];

    [RelayCommand]
    private void GenerateCoreProvider()
    {
        var path = this.pmrCarProviderGenerator.Generate(this.cars);
        var missingGameIdCount = this.cars.Count(car => string.IsNullOrWhiteSpace(car.GameId));
        this.GenerateStatusMessage =
            $"Generated {this.cars.Count} cars to {path} at {DateTime.Now:HH:mm:ss} "
            + $"- {missingGameIdCount} still missing a GameId.";
    }

    [RelayCommand]
    private void NewCar()
    {
        this.SelectedCar = null;
        this.Editor.LoadFrom(null);
        if(this.BrowseMode == CarBrowseMode.ByClass && this.SelectedClass is not null)
        {
            this.Editor.VehicleClass = this.SelectedClass;
        }
    }

    [RelayCommand]
    private void SaveCar()
    {
        var car = this.Editor.ToCarInfo();
        this.carRepository.Save(car);

        this.cars.RemoveAll(existing => existing.Id == car.Id);
        this.cars.Add(car);

        this.RefreshClasses();
        this.RefreshVisibleCars();
        this.SelectedCar = this.VisibleCars.FirstOrDefault(visibleCar => visibleCar.Id == car.Id);
    }

    [RelayCommand]
    private void SetBrowseMode(CarBrowseMode mode)
    {
        this.BrowseMode = mode;
    }

    partial void OnBrowseModeChanged(CarBrowseMode value)
    {
        this.OnPropertyChanged(nameof(this.IsByClassMode));
        this.RefreshVisibleCars();
    }

    partial void OnSelectedCarChanged(CarInfo? value)
    {
        this.Editor.LoadFrom(value);
    }

    partial void OnSelectedClassChanged(string? value)
    {
        this.RefreshVisibleCars();
    }

    private void RefreshClasses()
    {
        var distinctClasses = this.cars.Select(car => car.VehicleClass).Distinct().OrderBy(vehicleClass => vehicleClass);

        this.Classes.Clear();
        foreach(var vehicleClass in distinctClasses)
        {
            this.Classes.Add(vehicleClass);
        }
    }

    private void RefreshVisibleCars()
    {
        this.VisibleCars.Clear();

        var carsToShow = this.BrowseMode == CarBrowseMode.ByClass
            ? this.cars.Where(car => car.VehicleClass == this.SelectedClass)
            : this.cars;

        foreach(var car in carsToShow.OrderBy(car => car.Manufacturer).ThenBy(car => car.Name))
        {
            this.VisibleCars.Add(car);
        }

        this.SelectedCar = this.VisibleCars.FirstOrDefault();
    }
}
