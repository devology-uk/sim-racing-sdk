using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Ams2.Core.Abstractions;
using SimRacingSdk.Ams2.Core.Models;

namespace SimRacingSdk.Ams2.Demo.TrackExplorer
{
    public partial class TrackExplorerViewModel : ObservableObject
    {
        private readonly IAms2TrackInfoProvider trackInfoProvider;

        [ObservableProperty]
        private Ams2TrackInfo? selectedTrack = null;

        [ObservableProperty]
        private string selectedTrackName = string.Empty;

        public TrackExplorerViewModel(IAms2TrackInfoProvider trackInfoProvider)
        {
            this.trackInfoProvider = trackInfoProvider;
        }

        public ObservableCollection<string> TrackNames { get; } = [];
        public ObservableCollection<Ams2TrackInfo> Tracks { get; } = [];

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
                "ams2-tracks.csv");
            var trackLayoutsFilePath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "ams2-track-layouts.csv");

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

            foreach(var ams2TrackInfo in this.trackInfoProvider.GetTrackInfos())
            {
                var trackInfo =
                    $"{ams2TrackInfo.ShortName},{ams2TrackInfo.FullName},{ams2TrackInfo.Country},{ams2TrackInfo.CountryCode},{ams2TrackInfo.Latitude},{ams2TrackInfo.Longitude},{ams2TrackInfo.AltitudeM}";
                tracksStreamWriter.WriteLine(trackInfo);
                tracksStreamWriter.Flush();

                foreach(var ams2TrackLayoutInfo in ams2TrackInfo.Layouts)
                {
                    var layoutInfo = $"{ams2TrackInfo.ShortName},{ams2TrackLayoutInfo.Ams2TrackId},{ams2TrackLayoutInfo.Name},{ams2TrackLayoutInfo.Corners},{ams2TrackLayoutInfo.Grade},{ams2TrackLayoutInfo.LengthKm},{ams2TrackLayoutInfo.MaxGridSize},{ams2TrackLayoutInfo.TrackType}";
                    layoutsStreamWriter.WriteLine(layoutInfo);
                    layoutsStreamWriter.Flush();
                }
            }
        }
    }
}