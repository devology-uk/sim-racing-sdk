using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
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
            foreach(var trackName in this.trackInfoProvider.GetTrackName())
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
    }
}