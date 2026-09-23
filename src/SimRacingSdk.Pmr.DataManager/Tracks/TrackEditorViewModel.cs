using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimRacingSdk.Pmr.DataManager.Tracks;

// A mutable, bindable working copy of a TrackInfo - TrackInfo itself stays an immutable record (its
// required init-only properties can't be targeted by TwoWay bindings), so editing happens here and
// is only turned back into a TrackInfo when the user explicitly saves (mirrors CarEditorViewModel).
public partial class TrackEditorViewModel : ObservableObject
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

    [ObservableProperty]
    private TrackLayoutEditorViewModel? selectedLayout;

    public ObservableCollection<TrackLayoutEditorViewModel> Layouts { get; } = [];

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

        this.Layouts.Clear();
        foreach(var layout in track?.Layouts ?? [])
        {
            this.Layouts.Add(TrackLayoutEditorViewModel.From(layout));
        }

        this.SelectedLayout = this.Layouts.FirstOrDefault();
    }

    public TrackInfo ToTrackInfo()
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
            Layouts = this.Layouts.Select(layout => layout.ToTrackLayoutInfo()).ToList(),
            Longitude = this.Longitude,
            Name = this.Name
        };
    }

    [RelayCommand]
    private void AddLayout()
    {
        var layout = new TrackLayoutEditorViewModel();
        this.Layouts.Add(layout);
        this.SelectedLayout = layout;
    }

    [RelayCommand]
    private void RemoveLayout()
    {
        if(this.SelectedLayout is null)
        {
            return;
        }

        this.Layouts.Remove(this.SelectedLayout);
        this.SelectedLayout = this.Layouts.FirstOrDefault();
    }
}
