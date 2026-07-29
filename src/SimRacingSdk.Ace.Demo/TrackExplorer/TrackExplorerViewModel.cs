using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Demo.TrackExplorer;

// Unlike Acc's one-entry-per-track model, Evo's events_practice.json has multiple layouts
// per track, so track selection (left list) and layout selection (right list) are separate steps.
public partial class TrackExplorerViewModel : ObservableObject
{
    private readonly IAceTrackInfoProvider trackInfoProvider;

    [ObservableProperty]
    private AceTrackInfo? selectedLayout = null;

    [ObservableProperty]
    private string selectedContinent = string.Empty;

    [ObservableProperty]
    private string selectedTrackName = string.Empty;

    public TrackExplorerViewModel(IAceTrackInfoProvider trackInfoProvider)
    {
        this.trackInfoProvider = trackInfoProvider;
    }

    public ObservableCollection<AceTrackInfo> Layouts { get; } = [];
    public ObservableCollection<string> Continents { get; } = [];
    public ObservableCollection<string> TrackNames { get; } = [];

    [RelayCommand]
    private void ExportCsv()
    {
        var tracksFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "ace-tracks.csv");

        using var tracksStreamWriter = new StreamWriter(tracksFilePath,
            options: new FileStreamOptions
            {
                Access = FileAccess.Write,
                Mode = FileMode.Create
            });

        foreach(var aceTrackInfo in this.trackInfoProvider.GetTrackInfos())
        {
            var trackInfo =
                $"{aceTrackInfo.Continent},{aceTrackInfo.CountryCode},{aceTrackInfo.Track},{aceTrackInfo.ShortName},{aceTrackInfo.Layout},{aceTrackInfo.TrackLengthMeters},{aceTrackInfo.MaxPitSlot},{aceTrackInfo.Latitude},{aceTrackInfo.Longitude}";
            tracksStreamWriter.WriteLine(trackInfo);
            tracksStreamWriter.Flush();
        }
    }

    public void Init()
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

        this.SelectedTrackName = this.TrackNames.Count > 0? this.TrackNames[0]: string.Empty;
    }

    partial void OnSelectedTrackNameChanged(string value)
    {
        this.Layouts.Clear();
        foreach(var layout in this.trackInfoProvider.GetLayoutsForTrack(value))
        {
            this.Layouts.Add(layout);
        }

        this.SelectedLayout = this.Layouts.Count > 0? this.Layouts[0]: null;
    }
}
