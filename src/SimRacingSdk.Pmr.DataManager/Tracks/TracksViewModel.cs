using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimRacingSdk.Pmr.DataManager.Tracks;

public partial class TracksViewModel : ObservableObject
{
    private readonly IPmrTrackProviderGenerator pmrTrackProviderGenerator;
    private readonly List<TrackInfo> tracks;
    private readonly ITrackRepository trackRepository;

    [ObservableProperty]
    private TrackBrowseMode browseMode = TrackBrowseMode.ByContinent;

    [ObservableProperty]
    private string generateStatusMessage = string.Empty;

    [ObservableProperty]
    private string? selectedContinent;

    [ObservableProperty]
    private TrackInfo? selectedTrack;

    public TracksViewModel(ITrackRepository trackRepository, IPmrTrackProviderGenerator pmrTrackProviderGenerator)
    {
        this.trackRepository = trackRepository;
        this.pmrTrackProviderGenerator = pmrTrackProviderGenerator;
        this.tracks = this.trackRepository.GetAll().ToList();
        this.RefreshContinents();
        this.SelectedContinent = this.Continents.FirstOrDefault();
    }

    // Not a fixed list - derived from whatever continents are actually present in the saved
    // tracks, so a new grouping shows up here the moment a track using it is saved, with no code
    // change needed (mirrors CarsViewModel.Classes).
    public ObservableCollection<string> Continents { get; } = [];

    public TrackEditorViewModel Editor { get; } = new();
    public bool IsByContinentMode => this.BrowseMode == TrackBrowseMode.ByContinent;
    public ObservableCollection<TrackInfo> VisibleTracks { get; } = [];

    [RelayCommand]
    private void GenerateCoreProvider()
    {
        var path = this.pmrTrackProviderGenerator.Generate(this.tracks);
        var missingGameIdCount = this.tracks.Count(track => string.IsNullOrWhiteSpace(track.GameId));
        this.GenerateStatusMessage =
            $"Generated {this.tracks.Count} tracks to {path} at {DateTime.Now:HH:mm:ss} "
            + $"- {missingGameIdCount} still missing a GameId.";
    }

    [RelayCommand]
    private void NewTrack()
    {
        this.SelectedTrack = null;
        this.Editor.LoadFrom(null);
        if(this.BrowseMode == TrackBrowseMode.ByContinent && this.SelectedContinent is not null)
        {
            this.Editor.Continent = this.SelectedContinent;
        }
    }

    [RelayCommand]
    private void SaveTrack()
    {
        var track = this.Editor.ToTrackInfo();
        this.trackRepository.Save(track);

        this.tracks.RemoveAll(existing => existing.Id == track.Id);
        this.tracks.Add(track);

        this.RefreshContinents();
        this.RefreshVisibleTracks();
        this.SelectedTrack = this.VisibleTracks.FirstOrDefault(visibleTrack => visibleTrack.Id == track.Id);
    }

    [RelayCommand]
    private void SetBrowseMode(TrackBrowseMode mode)
    {
        this.BrowseMode = mode;
    }

    partial void OnBrowseModeChanged(TrackBrowseMode value)
    {
        this.OnPropertyChanged(nameof(this.IsByContinentMode));
        this.RefreshVisibleTracks();
    }

    partial void OnSelectedContinentChanged(string? value)
    {
        this.RefreshVisibleTracks();
    }

    partial void OnSelectedTrackChanged(TrackInfo? value)
    {
        this.Editor.LoadFrom(value);
    }

    private void RefreshContinents()
    {
        var distinctContinents = this.tracks.Select(track => track.Continent).Distinct().OrderBy(continent => continent);

        this.Continents.Clear();
        foreach(var continent in distinctContinents)
        {
            this.Continents.Add(continent);
        }
    }

    private void RefreshVisibleTracks()
    {
        this.VisibleTracks.Clear();

        var tracksToShow = this.BrowseMode == TrackBrowseMode.ByContinent
            ? this.tracks.Where(track => track.Continent == this.SelectedContinent)
            : this.tracks;

        foreach(var track in tracksToShow.OrderBy(track => track.Name))
        {
            this.VisibleTracks.Add(track);
        }

        this.SelectedTrack = this.VisibleTracks.FirstOrDefault();
    }
}
