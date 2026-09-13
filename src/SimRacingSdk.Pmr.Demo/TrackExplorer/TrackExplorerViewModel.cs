using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.Pmr.Core.Abstractions;
using SimRacingSdk.Pmr.Core.Models;

namespace SimRacingSdk.Pmr.Demo.TrackExplorer;

public partial class TrackExplorerViewModel : ObservableObject
{
    private readonly IPmrTrackInfoProvider trackInfoProvider;

    [ObservableProperty]
    private PmrTrackInfo? selectedLayout;

    [ObservableProperty]
    private string selectedContinent = string.Empty;

    [ObservableProperty]
    private string selectedTrackName = string.Empty;

    public TrackExplorerViewModel(IPmrTrackInfoProvider trackInfoProvider)
    {
        this.trackInfoProvider = trackInfoProvider;
    }

    public ObservableCollection<PmrTrackInfo> Layouts { get; } = [];
    public ObservableCollection<string> Continents { get; } = [];
    public ObservableCollection<string> TrackNames { get; } = [];

    internal void Init()
    {
        this.Continents.Clear();
        foreach(var continent in this.trackInfoProvider.GetContinents())
        {
            this.Continents.Add(continent);
        }

        if(this.Continents.Count > 0)
        {
            this.SelectedContinent = this.Continents[0];
        }
    }

    partial void OnSelectedContinentChanged(string value)
    {
        this.TrackNames.Clear();
        foreach(var trackName in this.trackInfoProvider.GetTrackNamesForContinent(value))
        {
            this.TrackNames.Add(trackName);
        }

        this.SelectedTrackName = this.TrackNames.Count > 0 ? this.TrackNames[0] : string.Empty;
    }

    partial void OnSelectedTrackNameChanged(string value)
    {
        this.Layouts.Clear();
        foreach(var layout in this.trackInfoProvider.GetLayoutsForTrack(value))
        {
            this.Layouts.Add(layout);
        }

        this.SelectedLayout = this.Layouts.Count > 0 ? this.Layouts[0] : null;
    }
}
