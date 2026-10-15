using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Demo.TrackExplorer;

public partial class TrackExplorerViewModel : ObservableObject
{
    private readonly IAceTrackInfoProvider trackInfoProvider;

    [ObservableProperty]
    private string selectedContinent = string.Empty;

    [ObservableProperty]
    private AceTrackLayoutInfo? selectedLayout = null;

    [ObservableProperty]
    private AceTrackInfo? selectedTrack = null;

    public TrackExplorerViewModel(IAceTrackInfoProvider trackInfoProvider)
    {
        this.trackInfoProvider = trackInfoProvider;
    }

    public ObservableCollection<string> Continents { get; } = [];
    public ObservableCollection<AceTrackLayoutInfo> Layouts { get; } = [];
    public ObservableCollection<AceTrackInfo> Tracks { get; } = [];

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
            foreach(var layout in aceTrackInfo.Layouts)
            {
                var trackInfo =
                    $"{aceTrackInfo.Continent},{aceTrackInfo.CountryCode},{aceTrackInfo.Name},{aceTrackInfo.ShortName},{layout.Name},{layout.LengthMeters},{layout.Corners},{layout.MaxPitSlot},{aceTrackInfo.Latitude},{aceTrackInfo.Longitude}";
                tracksStreamWriter.WriteLine(trackInfo);
            }
        }

        tracksStreamWriter.Flush();
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
        this.Tracks.Clear();
        foreach(var track in this.trackInfoProvider.GetTrackInfosForContinent(value).OrderBy(t => t.Name))
        {
            this.Tracks.Add(track);
        }

        this.SelectedTrack = this.Tracks.FirstOrDefault();
    }

    partial void OnSelectedTrackChanged(AceTrackInfo? value)
    {
        this.Layouts.Clear();
        foreach(var layout in value?.Layouts ?? [])
        {
            this.Layouts.Add(layout);
        }

        this.SelectedLayout = this.Layouts.FirstOrDefault();
    }
}
