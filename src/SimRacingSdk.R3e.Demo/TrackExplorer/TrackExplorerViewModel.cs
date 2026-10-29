using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.R3e.Core.Abstractions;
using SimRacingSdk.R3e.Core.Models;

namespace SimRacingSdk.R3e.Demo.TrackExplorer;

public partial class TrackExplorerViewModel : ObservableObject
{
    private readonly IR3eTrackInfoProvider trackInfoProvider;

    [ObservableProperty]
    private bool isSelectableOnly = true;

    [ObservableProperty]
    private string? selectedCountry;

    [ObservableProperty]
    private R3eTrackLayoutInfo? selectedLayout;

    [ObservableProperty]
    private R3eTrackInfo? selectedTrack;

    public TrackExplorerViewModel(IR3eTrackInfoProvider trackInfoProvider)
    {
        this.trackInfoProvider = trackInfoProvider;
    }

    public ObservableCollection<string> Countries { get; } = [];
    public ObservableCollection<R3eTrackInfo> Tracks { get; } = [];

    internal void Init()
    {
        this.RefreshCountries();
    }

    private IEnumerable<R3eTrackInfo> ShownTracks()
    {
        return this.trackInfoProvider.GetTrackInfos()
                   .Where(track => !this.IsSelectableOnly || !track.NotInGameSelector);
    }

    partial void OnIsSelectableOnlyChanged(bool value)
    {
        this.RefreshCountries();
    }

    partial void OnSelectedCountryChanged(string? value)
    {
        this.Tracks.Clear();
        foreach(var track in this.ShownTracks().Where(track => track.Country == value).OrderBy(track => track.Name))
        {
            this.Tracks.Add(track);
        }

        this.SelectedTrack = this.Tracks.FirstOrDefault();
    }

    partial void OnSelectedTrackChanged(R3eTrackInfo? value)
    {
        this.SelectedLayout = value?.Layouts.FirstOrDefault();
    }

    private void RefreshCountries()
    {
        var previousCountry = this.SelectedCountry;

        this.Countries.Clear();
        foreach(var country in this.ShownTracks().Select(track => track.Country).Distinct().Order())
        {
            this.Countries.Add(country);
        }

        this.SelectedCountry = this.Countries.Contains(previousCountry ?? string.Empty)
            ? previousCountry
            : this.Countries.FirstOrDefault();
    }
}
