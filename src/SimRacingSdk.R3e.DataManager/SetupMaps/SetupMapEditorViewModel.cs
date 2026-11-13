using System.ComponentModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.R3e.DataManager.Cars;
using SimRacingSdk.R3e.Setups;

namespace SimRacingSdk.R3e.DataManager.SetupMaps;

// Every car shows the same fixed rows; only their availability, ranges and options change from car to car.
public partial class SetupMapEditorViewModel : ObservableObject
{
    private readonly List<SetupControlRowViewModel> rows;

    // The map as it was loaded or last saved, so switching cars only writes a file when something changed -
    // a car that was only looked at keeps no file.
    private string savedState = string.Empty;

    [ObservableProperty]
    private string carName = string.Empty;

    [ObservableProperty]
    private bool validated;

    public SetupMapEditorViewModel()
    {
        this.rows = R3eSetupControls.All.Select(definition => new SetupControlRowViewModel(definition)).ToList();
        this.Rows = CollectionViewSource.GetDefaultView(this.rows);
        this.Rows.GroupDescriptions.Add(new PropertyGroupDescription(nameof(SetupControlRowViewModel.Tab)));
    }

    public int CarGameId { get; private set; }
    public bool HasUnsavedChanges => this.CarGameId > 0 && SetupMapJson.Serialize(this.ToSetupMap()) != this.savedState;
    public ICollectionView Rows { get; }

    public void LoadSaved(CarInfo car, CapturedSetupMap? savedMap)
    {
        var map = savedMap ?? NewCarDefaults.For(car);
        this.Load(car, map, map.Validated);
        this.MarkSaved();
    }

    // Left unsaved and unvalidated: the source car's rows are a starting point to check against the game.
    public void LoadCopy(CarInfo car, CapturedSetupMap sourceMap)
    {
        this.Load(car, sourceMap, false);
    }

    public void MarkSaved()
    {
        this.savedState = SetupMapJson.Serialize(this.ToSetupMap());
    }

    public string? Problem()
    {
        return this.rows.Select(row => row.Problem()).FirstOrDefault(problem => problem is not null);
    }

    public CapturedSetupMap ToSetupMap()
    {
        return new CapturedSetupMap
        {
            CarGameId = this.CarGameId,
            CarName = this.CarName,
            Validated = this.Validated,
            Controls = this.rows.Select(row => row.ToSetting()).ToList()
        };
    }

    private void Load(CarInfo car, CapturedSetupMap map, bool isValidated)
    {
        this.CarGameId = car.GameId;
        this.CarName = car.Name;
        this.Validated = isValidated;

        foreach(var row in this.rows)
        {
            row.LoadFrom(map.SettingFor(row.Definition.Control));
        }
    }
}
