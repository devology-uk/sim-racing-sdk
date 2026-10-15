using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.DataManager.Shared.Catalog;

namespace SimRacingSdk.Ams2.DataManager.Tracks;

public partial class TrackEditorViewModel : ObservableObject, ICatalogEditor<TrackInfo>
{
    [ObservableProperty]
    private int altitudeM;

    [ObservableProperty]
    private string country = string.Empty;

    [ObservableProperty]
    private string countryCode = string.Empty;

    [ObservableProperty]
    private string fullName = string.Empty;

    [ObservableProperty]
    private double latitude;

    [ObservableProperty]
    private double longitude;

    // One raw track location per line.
    [ObservableProperty]
    private string rawTrackLocations = string.Empty;

    [ObservableProperty]
    private string shortName = string.Empty;

    public EditableCollection<TrackLayoutEditorViewModel> Layouts { get; } = new();

    public void LoadFrom(TrackInfo? track)
    {
        this.AltitudeM = track?.AltitudeM ?? 0;
        this.Country = track?.Country ?? string.Empty;
        this.CountryCode = track?.CountryCode ?? string.Empty;
        this.FullName = track?.FullName ?? string.Empty;
        this.Latitude = track?.Latitude ?? 0;
        this.Longitude = track?.Longitude ?? 0;
        this.RawTrackLocations = string.Join(Environment.NewLine, track?.RawTrackLocations ?? []);
        this.ShortName = track?.ShortName ?? string.Empty;
        this.Layouts.Replace((track?.Layouts ?? []).Select(TrackLayoutEditorViewModel.From));
    }

    public void StartInGroup(string group)
    {
        this.Country = group;
    }

    public TrackInfo ToEntity()
    {
        return new TrackInfo
        {
            AltitudeM = this.AltitudeM,
            Country = this.Country,
            CountryCode = this.CountryCode,
            FullName = this.FullName,
            Latitude = this.Latitude,
            Layouts = this.Layouts.Items.Select(layout => layout.ToTrackLayoutInfo()).ToList(),
            Longitude = this.Longitude,
            RawTrackLocations = this.RawTrackLocations
                .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .ToList(),
            ShortName = this.ShortName
        };
    }
}
