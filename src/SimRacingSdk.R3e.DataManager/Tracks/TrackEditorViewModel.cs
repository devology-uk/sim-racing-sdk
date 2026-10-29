using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.DataManager.Shared.Catalog;

namespace SimRacingSdk.R3e.DataManager.Tracks;

public partial class TrackEditorViewModel : ObservableObject, ICatalogEditor<TrackInfo>
{
    [ObservableProperty]
    private string country = string.Empty;

    [ObservableProperty]
    private string countryCode = string.Empty;

    [ObservableProperty]
    private int gameId;

    [ObservableProperty]
    private double? latitude;

    [ObservableProperty]
    private double? longitude;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private bool notInGameSelector;

    public EditableCollection<TrackLayoutEditorViewModel> Layouts { get; } = new();

    public void LoadFrom(TrackInfo? track)
    {
        this.Country = track?.Country ?? string.Empty;
        this.CountryCode = track?.CountryCode ?? string.Empty;
        this.GameId = track?.GameId ?? 0;
        this.Latitude = track?.Latitude;
        this.Longitude = track?.Longitude;
        this.Name = track?.Name ?? string.Empty;
        this.NotInGameSelector = track?.NotInGameSelector ?? false;
        this.Layouts.Replace(track?.Layouts.Select(TrackLayoutEditorViewModel.From) ?? []);
    }

    public void StartInGroup(string group)
    {
        this.Country = group;
    }

    public TrackInfo ToEntity()
    {
        return new TrackInfo
        {
            Country = this.Country,
            CountryCode = this.CountryCode,
            GameId = this.GameId,
            Latitude = this.Latitude,
            Layouts = this.Layouts.Items.Select(layout => layout.ToTrackLayoutInfo()).ToList(),
            Longitude = this.Longitude,
            Name = this.Name,
            NotInGameSelector = this.NotInGameSelector
        };
    }
}
