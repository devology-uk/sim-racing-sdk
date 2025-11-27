using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.Core.Models;

namespace SimRacingSdk.Lmu.Demo.TrackExplorer
{
    public partial class TrackExplorerViewModel : ObservableObject
    {
        private readonly ILmuTrackInfoProvider trackInfoProvider;

        [ObservableProperty]
        private LmuTrackInfo? selectedTrack = null;

        [ObservableProperty]
        private string selectedTrackName = string.Empty;

        public TrackExplorerViewModel(ILmuTrackInfoProvider trackInfoProvider)
        {
            this.trackInfoProvider = trackInfoProvider;
        }

        public ObservableCollection<string> TrackNames { get; } = [];
        public ObservableCollection<LmuTrackInfo> Tracks { get; } = [];

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
            this.SelectedTrack = this.trackInfoProvider.FindTrackByShortName(value);
        }

        [RelayCommand]
        private void ExportCsv()
        {
            var tracksFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "lmu-tracks.csv");
            var trackLayoutsFilePath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "lmu-track-layouts.csv");

            using var tracksStreamWriter = new StreamWriter(tracksFilePath,
                options: new FileStreamOptions
                {
                    Access = FileAccess.Write,
                    Mode = FileMode.Create
                });
            using var layoutsStreamWriter = new StreamWriter(trackLayoutsFilePath,
                options: new FileStreamOptions
                {
                    Access = FileAccess.Write,
                    Mode = FileMode.Create
                });

            foreach(var lmuTrackInfo in this.trackInfoProvider.GetTrackInfos())
            {
                var trackInfo =
                    $"{lmuTrackInfo.ShortName},{lmuTrackInfo.Name},{lmuTrackInfo.Country},{lmuTrackInfo.CountryCode},{lmuTrackInfo.Latitude},{lmuTrackInfo.Longitude}";
                tracksStreamWriter.WriteLine(trackInfo);
                tracksStreamWriter.Flush();

                foreach(var lmuTrackLayoutInfo in lmuTrackInfo.Layouts)
                {
                    var layoutInfo = $"{lmuTrackInfo.ShortName},{lmuTrackLayoutInfo.Name},{lmuTrackLayoutInfo.Name},{lmuTrackLayoutInfo.Corners},{lmuTrackLayoutInfo.LengthKm}";
                    layoutsStreamWriter.WriteLine(layoutInfo);
                    layoutsStreamWriter.Flush();
                }
            }
        }
    }
}