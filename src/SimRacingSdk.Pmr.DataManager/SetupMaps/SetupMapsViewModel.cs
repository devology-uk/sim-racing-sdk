using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Pmr.DataManager.Cars;
using SimRacingSdk.Pmr.DataManager.Session;

namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public partial class SetupMapsViewModel : ObservableObject, IUnsavedChangesSaver
{
    private static readonly TimeSpan AutosaveInterval = TimeSpan.FromSeconds(10);

    private readonly DispatcherTimer autosaveTimer = new() { Interval = AutosaveInterval };
    private readonly IDefaultSetupFieldApplier defaultSetupFieldApplier;
    private readonly ISessionStateStore sessionStateStore;
    private readonly ISetupMapRepository setupMapRepository;
    private readonly IPmrSetupMapsGenerator setupMapsGenerator;

    // The editor car's file as it was when loaded or last saved - a difference means it was edited
    // outside the app, and saving would silently write those edits away.
    private string loadedFileState = string.Empty;

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
        ISessionStateStore sessionStateStore,
        IUserConfirmation userConfirmation)
    {
        this.Editor = new SetupMapEditorViewModel(userConfirmation);
        this.setupMapRepository = setupMapRepository;
        this.defaultSetupFieldApplier = defaultSetupFieldApplier;
        this.setupMapsGenerator = setupMapsGenerator;
        this.sessionStateStore = sessionStateStore;

        foreach(var car in carRepository.GetAll().OrderBy(car => car.Manufacturer).ThenBy(car => car.Name))
        {
            this.Cars.Add(car);
        }

        this.RestoreSession();

        this.autosaveTimer.Tick += this.OnAutosaveTimerTick;
        this.autosaveTimer.Start();
    }

    public ObservableCollection<CarInfo> Cars { get; } = [];
    public SetupMapEditorViewModel Editor { get; }

    public string UnsavedChangesDescription =>
        $"Your changes to the {this.DisplayNameFor(this.Editor.CarId)} setup map can't be saved because its file was changed "
        + "outside the Data Manager.";

    public bool SaveUnsavedChanges()
    {
        return !this.Editor.HasUnsavedChanges || this.WriteEditor();
    }

    // Works on what's in the editor, not the file - written by Save Setup Map or the next autosave.
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
        if(!this.SaveUnsavedChanges())
        {
            return;
        }

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

        this.LoadEditorFromFile(this.SelectedCar);
    }

    // Loads the source car's fields into the editor - written by Save Setup Map or the next autosave.
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

        this.Editor.LoadFrom(this.SelectedCar.Id, sourceMap with { Validated = false });
        this.StatusMessage = $"Copied from {this.CopySourceCar.Manufacturer} {this.CopySourceCar.Name} - adjust, then Save Setup Map.";
    }

    // Writes saved maps only - save the car being edited first.
    [RelayCommand]
    private void GenerateSdkSetupMaps()
    {
        var result = this.setupMapsGenerator.Generate(this.Cars);
        this.StatusMessage = $"Generated {result.MapsWritten} validated map(s) into {result.OutputPath} at {DateTime.Now:HH:mm:ss}.";
    }

    // Discards any unsaved edits - used after the JSON file has been corrected outside the app.
    [RelayCommand]
    private void ReloadSetupMap()
    {
        if(this.SelectedCar is null)
        {
            return;
        }

        this.LoadEditorFromFile(this.SelectedCar);
        this.StatusMessage = $"Reloaded setup map for {this.SelectedCar.Manufacturer} {this.SelectedCar.Name} at {DateTime.Now:HH:mm:ss}.";
    }

    [RelayCommand]
    private void SaveSetupMap()
    {
        if(this.SelectedCar is null)
        {
            return;
        }

        if(this.WriteEditor())
        {
            this.StatusMessage = $"Saved setup map for {this.SelectedCar.Manufacturer} {this.SelectedCar.Name} at {DateTime.Now:HH:mm:ss}.";
        }
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

    private static string StateOf(PmrSetupMap? setupMap)
    {
        return setupMap is null ? string.Empty : JsonSerializer.Serialize(setupMap);
    }

    private string DisplayNameFor(string carId)
    {
        var car = this.Cars.FirstOrDefault(candidate => candidate.Id == carId);
        return car is null ? carId : $"{car.Manufacturer} {car.Name}";
    }

    private bool IsFileChangedOutsideApp()
    {
        return StateOf(this.setupMapRepository.FindByCarId(this.Editor.CarId)) != this.loadedFileState;
    }

    private void LoadEditorFromFile(CarInfo? car)
    {
        var setupMap = car is null ? null : this.setupMapRepository.FindByCarId(car.Id);
        this.Editor.LoadSaved(car?.Id ?? string.Empty, setupMap);
        this.loadedFileState = StateOf(setupMap);
    }

    private void OnAutosaveTimerTick(object? sender, EventArgs args)
    {
        if(this.Editor.HasUnsavedChanges && this.WriteEditor())
        {
            this.StatusMessage = $"Autosaved setup map for {this.DisplayNameFor(this.Editor.CarId)} at {DateTime.Now:HH:mm:ss}.";
        }
    }

    private bool WriteEditor()
    {
        if(this.Editor.CarId.Length == 0)
        {
            return true;
        }

        if(this.IsFileChangedOutsideApp())
        {
            this.StatusMessage = $"Not saved - the {this.DisplayNameFor(this.Editor.CarId)} setup map file was changed outside the "
                                 + "Data Manager. Reload to pick those changes up (edits made here since are discarded).";
            return false;
        }

        this.setupMapRepository.Save(this.Editor.ToSetupMap());
        this.Editor.MarkSaved();
        this.loadedFileState = StateOf(this.setupMapRepository.FindByCarId(this.Editor.CarId));
        return true;
    }

    // A null selection comes from WPF rebuilding the page, not from the user - keep the editor as is.
    partial void OnSelectedCarChanged(CarInfo? value)
    {
        if(value is null)
        {
            return;
        }

        this.SaveUnsavedChanges();
        this.LoadEditorFromFile(value);
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
