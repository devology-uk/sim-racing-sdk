using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Pmr.DataManager.Session;
using SimRacingSdk.Pmr.DataManager.SetupMaps;

namespace SimRacingSdk.Pmr.DataManager.Cars;

public partial class CarsViewModel : ObservableObject
{
    private readonly ICarRepository carRepository;
    private readonly List<CarInfo> cars;
    private readonly IPmrCarProviderGenerator pmrCarProviderGenerator;
    private readonly ISessionStateStore sessionStateStore;
    private readonly ISetupMapRepository setupMapRepository;

    [ObservableProperty]
    private string saveStatusMessage = string.Empty;

    [ObservableProperty]
    private CarBrowseMode browseMode = CarBrowseMode.ByClass;

    [ObservableProperty]
    private string generateStatusMessage = string.Empty;

    [ObservableProperty]
    private CarInfo? selectedCar;

    [ObservableProperty]
    private string? selectedClass;

    public CarsViewModel(ICarRepository carRepository, IPmrCarProviderGenerator pmrCarProviderGenerator,
        ISetupMapRepository setupMapRepository,
        ISessionStateStore sessionStateStore)
    {
        this.carRepository = carRepository;
        this.pmrCarProviderGenerator = pmrCarProviderGenerator;
        this.setupMapRepository = setupMapRepository;
        this.sessionStateStore = sessionStateStore;
        this.cars = this.carRepository.GetAll().ToList();
        this.RefreshClasses();
        this.RestoreSession();
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
        var originalId = this.SelectedCar?.Id;
        this.carRepository.Save(car);

        if(originalId is not null && originalId != car.Id)
        {
            this.RemoveRenamedCar(originalId, car.Id);
        }

        this.cars.RemoveAll(existing => existing.Id == car.Id);
        this.cars.Add(car);

        this.RefreshClasses();
        this.RefreshVisibleCars();
        this.SelectedCar = this.VisibleCars.FirstOrDefault(visibleCar => visibleCar.Id == car.Id);
        this.SaveStatusMessage = $"Saved {car.Manufacturer} {car.Name} at {DateTime.Now:HH:mm:ss}.";
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
        this.sessionStateStore.State.CarsBrowseMode = value.ToString();
        this.sessionStateStore.Save();
    }

    partial void OnSelectedCarChanged(CarInfo? value)
    {
        this.Editor.LoadFrom(value);
        this.sessionStateStore.State.CarsCarId = value?.Id;
        this.sessionStateStore.Save();
    }

    partial void OnSelectedClassChanged(string? value)
    {
        this.RefreshVisibleCars();
        this.sessionStateStore.State.CarsClass = value;
        this.sessionStateStore.Save();
    }

    // Reads every saved value up front - applying each one fires change handlers that overwrite the
    // rest of the saved state (e.g. picking a class selects its first car).
    private void RestoreSession()
    {
        var state = this.sessionStateStore.State;
        var savedBrowseMode = state.CarsBrowseMode;
        var savedClass = state.CarsClass;
        var savedCarId = state.CarsCarId;

        if(Enum.TryParse<CarBrowseMode>(savedBrowseMode, out var browseMode))
        {
            this.BrowseMode = browseMode;
        }

        this.SelectedClass = this.Classes.Contains(savedClass ?? string.Empty) ? savedClass : this.Classes.FirstOrDefault();
        this.SelectedCar = this.VisibleCars.FirstOrDefault(car => car.Id == savedCarId) ?? this.SelectedCar;
    }

    // Id is derived from Manufacturer/Name/Year, so editing any of them moves the car to a new file.
    private void RemoveRenamedCar(string originalId, string newId)
    {
        this.carRepository.Delete(originalId);
        this.setupMapRepository.ReassignCar(originalId, newId);
        this.cars.RemoveAll(existing => existing.Id == originalId);
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
