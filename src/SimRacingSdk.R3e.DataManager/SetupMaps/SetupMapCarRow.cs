using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.R3e.DataManager.Cars;

namespace SimRacingSdk.R3e.DataManager.SetupMaps;

public partial class SetupMapCarRow : ObservableObject
{
    private readonly IOwnedCarStore ownedCarStore;

    [ObservableProperty]
    private bool hasMap;

    private bool isOwned;

    [ObservableProperty]
    private bool isValidated;

    public SetupMapCarRow(CarInfo car, IOwnedCarStore ownedCarStore, CapturedSetupMap? savedMap)
    {
        this.Car = car;
        this.ownedCarStore = ownedCarStore;
        this.isOwned = ownedCarStore.IsOwned(car.GameId);
        this.ShowSavedMap(savedMap);
    }

    public CarInfo Car { get; }

    public bool IsOwned
    {
        get => this.isOwned;
        set
        {
            if(this.SetProperty(ref this.isOwned, value))
            {
                this.ownedCarStore.SetOwned(this.Car.GameId, value);
            }
        }
    }

    public string Label => $"{this.Car.Name} - {this.Car.Class}";

    public string MapStatus => this.IsValidated ? "Validated" : this.HasMap ? "Mapped" : string.Empty;

    // Every term must appear somewhere, so "gt3 bmw" finds the BMW GT3s in any order.
    public bool Matches(IReadOnlyCollection<string> searchTerms)
    {
        var searchable = $"{this.Car.Name} {this.Car.Manufacturer} {this.Car.Class} {this.Car.GameId}";
        return searchTerms.All(term => searchable.Contains(term, StringComparison.OrdinalIgnoreCase));
    }

    public void ShowSavedMap(CapturedSetupMap? savedMap)
    {
        this.HasMap = savedMap is not null;
        this.IsValidated = savedMap?.Validated ?? false;
        this.OnPropertyChanged(nameof(this.MapStatus));
    }
}
