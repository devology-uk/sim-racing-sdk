using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Acc.Core.Models;

namespace SimRacingSdk.Acc.Demo.TrackExplorer;

public partial class TrackExplorerViewModel : ObservableObject
{
    private readonly IAccTrackInfoProvider trackInfoProvider;

    [ObservableProperty]
    private AccTrackInfo? selectedTrack = null;

    [ObservableProperty]
    private string selectedTrackName = string.Empty;

    public TrackExplorerViewModel(IAccTrackInfoProvider trackInfoProvider)
    {
        this.trackInfoProvider = trackInfoProvider;
    }

    public ObservableCollection<string> TrackNames { get; } = [];
    public ObservableCollection<AccTrackInfo> Tracks { get; } = [];

    [RelayCommand]
    private void ExportCsv()
    {
        var tracksFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "acc-tracks.csv");

        using var tracksStreamWriter = new StreamWriter(tracksFilePath,
            options: new FileStreamOptions
            {
                Access = FileAccess.Write,
                Mode = FileMode.Create
            });

        foreach(var accTrackInfo in this.trackInfoProvider.GetTrackInfos())
        {
            var trackInfo =
                $"{accTrackInfo.TrackId},{accTrackInfo.ShortName},{accTrackInfo.FullName},{accTrackInfo.Country},{accTrackInfo.CountryCode},{accTrackInfo.Latitude},{accTrackInfo.Longitude},{accTrackInfo.Corners},{accTrackInfo.Length}";
            tracksStreamWriter.WriteLine(trackInfo);
            tracksStreamWriter.Flush();
        }
    }

    public void Init()
    {
        foreach(var trackName in this.trackInfoProvider.GetTrackNames())
        {
            this.TrackNames.Add(trackName);
        }

        if(this.TrackNames.Count > 0)
        {
            this.SelectedTrackName = this.TrackNames[0];
        }
    }

    partial void OnSelectedTrackNameChanged(string value)
    {
        this.SelectedTrack = this.trackInfoProvider.FindByFullName(value);
    }
}