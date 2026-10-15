using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.DataManager.Shared.Catalog;

namespace SimRacingSdk.Pmr.DataManager.Tracks;

public partial class TrackEditorViewModel : ObservableObject, ICatalogEditor<TrackInfo>
{
    [ObservableProperty]
    private double altitudeMeters;

    [ObservableProperty]
    private string author = string.Empty;

    [ObservableProperty]
    private string continent = string.Empty;

    [ObservableProperty]
    private string country = string.Empty;

    [ObservableProperty]
    private string countryCode = string.Empty;

    [ObservableProperty]
    private string? gameId;

    [ObservableProperty]
    private double latitude;

    [ObservableProperty]
    private double longitude;

    [ObservableProperty]
    private string name = string.Empty;

    public EditableCollection<TrackLayoutEditorViewModel> Layouts { get; } = new();

    public void LoadFrom(TrackInfo? track)
    {
        this.AltitudeMeters = track?.AltitudeMeters ?? 0;
        this.Author = track?.Author ?? string.Empty;
        this.Continent = track?.Continent ?? string.Empty;
        this.Country = track?.Country ?? string.Empty;
        this.CountryCode = track?.CountryCode ?? string.Empty;
        this.GameId = track?.GameId;
        this.Latitude = track?.Latitude ?? 0;
        this.Longitude = track?.Longitude ?? 0;
        this.Name = track?.Name ?? string.Empty;
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
            AltitudeMeters = this.AltitudeMeters,
            Author = this.Author,
            Continent = this.Continent,
            Country = this.Country,
            CountryCode = this.CountryCode,
            GameId = this.GameId,
            Latitude = this.Latitude,
            Layouts = this.Layouts.Items.Select(layout => layout.ToTrackLayoutInfo()).ToList(),
            Longitude = this.Longitude,
            Name = this.Name
        };
    }
}
