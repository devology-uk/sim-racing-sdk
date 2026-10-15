using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.DataManager.Shared.Catalog;

namespace SimRacingSdk.Ace.DataManager.Tracks;

public partial class TrackEditorViewModel : ObservableObject, ICatalogEditor<TrackInfo>
{
    [ObservableProperty]
    private string continent = string.Empty;

    [ObservableProperty]
    private string countryCode = string.Empty;

    [ObservableProperty]
    private double latitude;

    [ObservableProperty]
    private double longitude;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string shortName = string.Empty;

    public EditableCollection<TrackLayoutEditorViewModel> Layouts { get; } = new();

    public void LoadFrom(TrackInfo? track)
    {
        this.Continent = track?.Continent ?? string.Empty;
        this.CountryCode = track?.CountryCode ?? string.Empty;
        this.Latitude = track?.Latitude ?? 0;
        this.Longitude = track?.Longitude ?? 0;
        this.Name = track?.Name ?? string.Empty;
        this.ShortName = track?.ShortName ?? string.Empty;
        this.Layouts.Replace((track?.Layouts ?? []).Select(TrackLayoutEditorViewModel.From));
    }

    public void StartInGroup(string group)
    {
        this.Continent = group;
    }

    public TrackInfo ToEntity()
    {
        return new TrackInfo
        {
            Continent = this.Continent,
            CountryCode = this.CountryCode,
            Latitude = this.Latitude,
            Layouts = this.Layouts.Items.Select(layout => layout.ToTrackLayoutInfo()).ToList(),
            Longitude = this.Longitude,
            Name = this.Name,
            ShortName = this.ShortName
        };
    }
}
