using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.DataManager.Shared.Catalog;

namespace SimRacingSdk.Lmu.DataManager.Tracks;

public partial class TrackEditorViewModel : ObservableObject, ICatalogEditor<TrackInfo>
{
    [ObservableProperty]
    private string country = string.Empty;

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
        this.Country = track?.Country ?? string.Empty;
        this.CountryCode = track?.CountryCode ?? string.Empty;
        this.Latitude = track?.Latitude ?? 0;
        this.Longitude = track?.Longitude ?? 0;
        this.Name = track?.Name ?? string.Empty;
        this.ShortName = track?.ShortName ?? string.Empty;
        this.Layouts.Replace(track is null ? [NewDefaultLayout()] : track.Layouts.Select(TrackLayoutEditorViewModel.From));
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
            Latitude = this.Latitude,
            Layouts = this.Layouts.Items.Select(layout => layout.ToTrackLayoutInfo()).ToList(),
            Longitude = this.Longitude,
            Name = this.Name,
            ShortName = this.ShortName
        };
    }

    private static TrackLayoutEditorViewModel NewDefaultLayout()
    {
        return new TrackLayoutEditorViewModel { Name = "Default" };
    }
}
