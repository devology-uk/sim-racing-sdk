using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Pmr.DataManager.Cars;
using SimRacingSdk.Pmr.DataManager.Session;

namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public partial class SetupMapsViewModel : ObservableObject
{
    private readonly IDefaultSetupFieldApplier defaultSetupFieldApplier;
    private readonly ISessionStateStore sessionStateStore;
    private readonly ISetupMapRepository setupMapRepository;
    private readonly IPmrSetupMapsGenerator setupMapsGenerator;

    [ObservableProperty]
    private CarInfo? copySourceCar;

    [ObservableProperty]
    private CarInfo? selectedCar;

    [ObservableProperty]
    private int selectedTabIndex;

    [ObservableProperty]
    private string statusMessage = string.Empty;

    public SetupMapsViewModel(
        ICarRepository carRepository,
        ISetupMapRepository setupMapRepository,
        IDefaultSetupFieldApplier defaultSetupFieldApplier,
        IPmrSetupMapsGenerator setupMapsGenerator,
        ISessionStateStore sessionStateStore)
    {
        this.setupMapRepository = setupMapRepository;
        this.defaultSetupFieldApplier = defaultSetupFieldApplier;
        this.setupMapsGenerator = setupMapsGenerator;
        this.sessionStateStore = sessionStateStore;

        foreach(var car in carRepository.GetAll().OrderBy(car => car.Manufacturer).ThenBy(car => car.Name))
        {
            this.Cars.Add(car);
        }

        this.RestoreSession();
    }

    public ObservableCollection<CarInfo> Cars { get; } = [];
    public SetupMapEditorViewModel Editor { get; } = new();

    // Works on what's in the editor, not the file - nothing is written until Save Setup Map.
    [RelayCommand]
    private void AddDefaultFieldsToSelectedCar()
    {
        if(this.SelectedCar is null)
        {
            return;
        }

        var map = this.Editor.ToSetupMap();
        var added = this.defaultSetupFieldApplier.AddMissingFields(map);
        this.Editor.LoadFrom(this.SelectedCar.Id, map);
        this.StatusMessage = $"Added {added} default field(s) to {this.SelectedCar.Manufacturer} {this.SelectedCar.Name} - review, then Save Setup Map.";
    }

    [RelayCommand]
    private void ApplyForceFeedbackToAllCars()
    {
        var carsUpdated = 0;
        var fieldsChanged = 0;

        foreach(var car in this.Cars)
        {
            var map = this.setupMapRepository.FindByCarId(car.Id) ?? EmptySetupMap(car.Id);
            var changed = this.defaultSetupFieldApplier.ApplyForceFeedback(map);
            if(changed == 0)
            {
                continue;
            }

            this.setupMapRepository.Save(map);
            carsUpdated++;
            fieldsChanged += changed;
        }

        this.StatusMessage = $"Updated {fieldsChanged} Force Feedback field(s) across {carsUpdated} car(s) at {DateTime.Now:HH:mm:ss}.";

        if(this.SelectedCar is not null)
        {
            this.Editor.LoadFrom(this.SelectedCar.Id, this.setupMapRepository.FindByCarId(this.SelectedCar.Id));
        }
    }

    // Loads the source car's fields into the editor only - nothing is written until Save, so the
    // copy can be adjusted (or abandoned by picking another car) first.
    [RelayCommand]
    private void CopyFromSourceCar()
    {
        if(this.SelectedCar is null || this.CopySourceCar is null || this.CopySourceCar.Id == this.SelectedCar.Id)
        {
            return;
        }

        var sourceMap = this.setupMapRepository.FindByCarId(this.CopySourceCar.Id);
        if(sourceMap is null)
        {
            this.StatusMessage = $"{this.CopySourceCar.Manufacturer} {this.CopySourceCar.Name} has no setup map to copy.";
            return;
        }

        this.Editor.LoadFrom(this.SelectedCar.Id, sourceMap);
        this.StatusMessage = $"Copied from {this.CopySourceCar.Manufacturer} {this.CopySourceCar.Name} - adjust, then Save Setup Map.";
    }

    // Writes saved maps only - save the car being edited first.
    [RelayCommand]
    private void GenerateSdkSetupMaps()
    {
        var outputPath = this.setupMapsGenerator.Generate(this.Cars);
        this.StatusMessage = $"Generated {outputPath} at {DateTime.Now:HH:mm:ss}.";
    }

    // Discards any unsaved edits - used after the JSON file has been corrected outside the app.
    [RelayCommand]
    private void ReloadSetupMap()
    {
        if(this.SelectedCar is null)
        {
            return;
        }

        this.Editor.LoadFrom(this.SelectedCar.Id, this.setupMapRepository.FindByCarId(this.SelectedCar.Id));
        this.StatusMessage = $"Reloaded setup map for {this.SelectedCar.Manufacturer} {this.SelectedCar.Name} at {DateTime.Now:HH:mm:ss}.";
    }

    [RelayCommand]
    private void SaveSetupMap()
    {
        if(this.SelectedCar is null)
        {
            return;
        }

        this.setupMapRepository.Save(this.Editor.ToSetupMap());
        this.StatusMessage = $"Saved setup map for {this.SelectedCar.Manufacturer} {this.SelectedCar.Name} at {DateTime.Now:HH:mm:ss}.";
    }

    private static PmrSetupMap EmptySetupMap(string carId)
    {
        return new PmrSetupMap
        {
            CarId = carId,
            EngineAndDrivetrain = [],
            SteeringWheel = [],
            Suspension = [],
            TyresAndChassis = []
        };
    }

    partial void OnSelectedCarChanged(CarInfo? value)
    {
        this.Editor.LoadFrom(value?.Id ?? string.Empty, value is null ? null : this.setupMapRepository.FindByCarId(value.Id));
        this.sessionStateStore.State.SetupMapsCarId = value?.Id;
        this.sessionStateStore.Save();
    }

    partial void OnSelectedTabIndexChanged(int value)
    {
        this.sessionStateStore.State.SetupMapsTabIndex = value;
        this.sessionStateStore.Save();
    }

    private void RestoreSession()
    {
        var state = this.sessionStateStore.State;
        var savedTabIndex = state.SetupMapsTabIndex;

        this.SelectedCar = this.Cars.FirstOrDefault(car => car.Id == state.SetupMapsCarId) ?? this.Cars.FirstOrDefault();
        this.SelectedTabIndex = savedTabIndex;
    }
}
