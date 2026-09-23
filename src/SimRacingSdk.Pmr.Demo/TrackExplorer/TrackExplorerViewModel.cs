using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.Pmr.Core.Abstractions;
using SimRacingSdk.Pmr.Core.Models;

namespace SimRacingSdk.Pmr.Demo.TrackExplorer;

public partial class TrackExplorerViewModel : ObservableObject
{
    private readonly IPmrTrackInfoProvider trackInfoProvider;

    [ObservableProperty]
    private PmrTrackLayoutInfo? selectedLayout;

    [ObservableProperty]
    private string selectedContinent = string.Empty;

    [ObservableProperty]
    private PmrTrackInfo? selectedTrack;

    public TrackExplorerViewModel(IPmrTrackInfoProvider trackInfoProvider)
    {
        this.trackInfoProvider = trackInfoProvider;
    }

    public ObservableCollection<string> Continents { get; } = [];
    public ObservableCollection<PmrTrackInfo> Tracks { get; } = [];

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
        this.Tracks.Clear();
        foreach(var track in this.trackInfoProvider.GetTrackInfosForContinent(value))
        {
            this.Tracks.Add(track);
        }

        this.SelectedTrack = this.Tracks.FirstOrDefault();
    }

    partial void OnSelectedTrackChanged(PmrTrackInfo? value)
    {
        this.SelectedLayout = value?.Layouts.FirstOrDefault();
    }
}
