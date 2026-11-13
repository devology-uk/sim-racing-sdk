using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Session;
using SimRacingSdk.DataManager.Shared.Storage;
using SimRacingSdk.R3e.DataManager.Cars;
using SimRacingSdk.R3e.DataManager.Session;

namespace SimRacingSdk.R3e.DataManager.SetupMaps;

public partial class SetupMapsViewModel : ObservableObject, IUnsavedChangesSaver
{
    private static readonly char[] SearchSeparators = [' '];

    private readonly List<SetupMapCarRow> allCars;
    private readonly ISessionStateStore<R3eSessionState> sessionStateStore;
    private readonly ISetupMapRepository setupMapRepository;
    private readonly IR3eSetupMapsGenerator setupMapsGenerator;

    [ObservableProperty]
    private SetupMapCarRow? copySourceCar;

    [ObservableProperty]
    private CarListFilter filter = CarListFilter.AllCars;

    [ObservableProperty]
    private string progressMessage = string.Empty;

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private SetupMapCarRow? selectedCar;

    [ObservableProperty]
    private string statusMessage = string.Empty;

    public SetupMapsViewModel(
        IEntityRepository<CarInfo> carRepository,
        ISetupMapRepository setupMapRepository,
        IOwnedCarStore ownedCarStore,
        IR3eSetupMapsGenerator setupMapsGenerator,
        ISessionStateStore<R3eSessionState> sessionStateStore)
    {
        this.setupMapRepository = setupMapRepository;
        this.setupMapsGenerator = setupMapsGenerator;
        this.sessionStateStore = sessionStateStore;

        var savedMaps = setupMapRepository.GetAll().ToDictionary(map => map.CarGameId);
        this.allCars = carRepository.GetAll()
            .Where(car => !car.NotInGameSelector)
            .OrderBy(car => car.Name)
            .ThenBy(car => car.Class)
            .Select(car => new SetupMapCarRow(car, ownedCarStore, savedMaps.GetValueOrDefault(car.GameId)))
            .ToList();

        this.RestoreSession();
        this.RefreshMappedCars();
    }

    public ObservableCollection<SetupMapCarRow> Cars { get; } = [];
    public SetupMapEditorViewModel Editor { get; } = new();
    public ObservableCollection<SetupMapCarRow> MappedCars { get; } = [];

    public string UnsavedChangesDescription => $"Your changes to the {this.Editor.CarName} setup map can't be saved: {this.Editor.Problem()}.";

    public bool SaveUnsavedChanges()
    {
        return !this.Editor.HasUnsavedChanges || this.WriteEditor();
    }

    [RelayCommand]
    private void CopyFromSourceCar()
    {
        if(this.SelectedCar is null || this.CopySourceCar is null || this.CopySourceCar == this.SelectedCar)
        {
            return;
        }

        var sourceMap = this.setupMapRepository.FindByCarGameId(this.CopySourceCar.Car.GameId);
        if(sourceMap is null)
        {
            this.StatusMessage = $"{this.CopySourceCar.Car.Name} has no saved setup map to copy.";
            return;
        }

        this.Editor.LoadCopy(this.SelectedCar.Car, sourceMap);
        this.StatusMessage = $"Copied from {this.CopySourceCar.Label} - check it against the game, then Save Setup Map.";
    }

    // Writes saved maps only - save the car being edited first.
    [RelayCommand]
    private void GenerateSdkSetupMaps()
    {
        if(!this.SaveUnsavedChanges())
        {
            return;
        }

        var result = this.setupMapsGenerator.Generate();
        this.StatusMessage = $"Generated {result.MapsWritten} validated map(s) into {result.OutputPath} at {DateTime.Now:HH:mm:ss}.";
    }

    [RelayCommand]
    private void ReloadSetupMap()
    {
        if(this.SelectedCar is null)
        {
            return;
        }

        this.LoadEditor(this.SelectedCar);
        this.StatusMessage = $"Reloaded {this.SelectedCar.Label}, discarding unsaved changes.";
    }

    [RelayCommand]
    private void SaveSetupMap()
    {
        if(this.SelectedCar is not null && this.WriteEditor())
        {
            this.StatusMessage = $"Saved setup map for {this.Editor.CarName} at {DateTime.Now:HH:mm:ss}.";
        }
    }

    private void LoadEditor(SetupMapCarRow carRow)
    {
        this.Editor.LoadSaved(carRow.Car, this.setupMapRepository.FindByCarGameId(carRow.Car.GameId));
    }

    private void RefreshCars()
    {
        var searchTerms = this.SearchText.Split(SearchSeparators, StringSplitOptions.RemoveEmptyEntries);
        var selected = this.SelectedCar;

        this.Cars.Clear();
        foreach(var carRow in this.allCars.Where(carRow => this.IsShown(carRow) && carRow.Matches(searchTerms)))
        {
            this.Cars.Add(carRow);
        }

        this.SelectedCar = selected is not null && this.Cars.Contains(selected) ? selected : this.SelectedCar;
    }

    private bool IsShown(SetupMapCarRow carRow)
    {
        return this.Filter switch
        {
            CarListFilter.OwnedCars => carRow.IsOwned,
            CarListFilter.OwnedNotMapped => carRow.IsOwned && !carRow.HasMap,
            CarListFilter.MappedCars => carRow.HasMap,
            _ => true
        };
    }

    private void RefreshMappedCars()
    {
        this.MappedCars.Clear();
        foreach(var carRow in this.allCars.Where(carRow => carRow.HasMap))
        {
            this.MappedCars.Add(carRow);
        }

        var ownedCount = this.allCars.Count(carRow => carRow.IsOwned);
        var validatedCount = this.allCars.Count(carRow => carRow.IsValidated);
        this.ProgressMessage = $"{ownedCount} owned, {this.MappedCars.Count} mapped, {validatedCount} validated";
    }

    private bool WriteEditor()
    {
        var problem = this.Editor.Problem();
        if(problem is not null)
        {
            this.StatusMessage = $"Not saved - {problem}.";
            return false;
        }

        var setupMap = this.Editor.ToSetupMap();
        this.setupMapRepository.Save(setupMap);
        this.Editor.MarkSaved();

        this.allCars.FirstOrDefault(carRow => carRow.Car.GameId == setupMap.CarGameId)?.ShowSavedMap(setupMap);
        this.RefreshMappedCars();
        return true;
    }

    partial void OnFilterChanged(CarListFilter value)
    {
        this.RefreshCars();
        this.sessionStateStore.State.SetupMapsFilter = value.ToString();
        this.sessionStateStore.Save();
    }

    partial void OnSearchTextChanged(string value)
    {
        this.RefreshCars();
        this.sessionStateStore.State.SetupMapsSearchText = value;
        this.sessionStateStore.Save();
    }

    // A null selection comes from the list being refiltered or WPF rebuilding the page, and reselecting the car
    // already in the editor from a refilter or a refused switch - both keep the editor as is.
    partial void OnSelectedCarChanged(SetupMapCarRow? oldValue, SetupMapCarRow? newValue)
    {
        if(newValue is null || newValue.Car.GameId == this.Editor.CarGameId)
        {
            return;
        }

        if(!this.SaveUnsavedChanges())
        {
            this.SelectedCar = oldValue;
            return;
        }

        this.LoadEditor(newValue);
        this.sessionStateStore.State.SetupMapsCarGameId = newValue.Car.GameId;
        this.sessionStateStore.Save();
    }

    private void RestoreSession()
    {
        var state = this.sessionStateStore.State;
        var savedCarGameId = state.SetupMapsCarGameId;
        var savedSearchText = state.SetupMapsSearchText ?? string.Empty;

        if(Enum.TryParse<CarListFilter>(state.SetupMapsFilter, out var savedFilter))
        {
            this.Filter = savedFilter;
        }

        this.SearchText = savedSearchText;
        this.RefreshCars();
        this.SelectedCar = this.Cars.FirstOrDefault(carRow => carRow.Car.GameId == savedCarGameId) ?? this.Cars.FirstOrDefault();
    }
}
