using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.Pmr.DataManager.Session;

namespace SimRacingSdk.Pmr.DataManager.Tracks;

public partial class TracksViewModel : ObservableObject
{
    private readonly IPmrTrackProviderGenerator pmrTrackProviderGenerator;
    private readonly ISessionStateStore sessionStateStore;
    private readonly List<TrackInfo> tracks;
    private readonly ITrackRepository trackRepository;

    [ObservableProperty]
    private TrackBrowseMode browseMode = TrackBrowseMode.ByContinent;

    [ObservableProperty]
    private string generateStatusMessage = string.Empty;

    [ObservableProperty]
    private string saveStatusMessage = string.Empty;

    [ObservableProperty]
    private string? selectedContinent;

    [ObservableProperty]
    private TrackInfo? selectedTrack;

    public TracksViewModel(
        ITrackRepository trackRepository,
        IPmrTrackProviderGenerator pmrTrackProviderGenerator,
        ISessionStateStore sessionStateStore)
    {
        this.trackRepository = trackRepository;
        this.pmrTrackProviderGenerator = pmrTrackProviderGenerator;
        this.sessionStateStore = sessionStateStore;
        this.tracks = this.trackRepository.GetAll().ToList();
        this.RefreshContinents();
        this.RestoreSession();
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
        this.SaveStatusMessage = $"Saved {track.Name} at {DateTime.Now:HH:mm:ss}.";
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
        this.sessionStateStore.State.TracksBrowseMode = value.ToString();
        this.sessionStateStore.Save();
    }

    partial void OnSelectedContinentChanged(string? value)
    {
        this.RefreshVisibleTracks();
        this.sessionStateStore.State.TracksContinent = value;
        this.sessionStateStore.Save();
    }

    partial void OnSelectedTrackChanged(TrackInfo? value)
    {
        this.Editor.LoadFrom(value);
        this.sessionStateStore.State.TracksTrackId = value?.Id;
        this.sessionStateStore.Save();
    }

    // Reads every saved value up front - see CarsViewModel.RestoreSession.
    private void RestoreSession()
    {
        var state = this.sessionStateStore.State;
        var savedBrowseMode = state.TracksBrowseMode;
        var savedContinent = state.TracksContinent;
        var savedTrackId = state.TracksTrackId;

        if(Enum.TryParse<TrackBrowseMode>(savedBrowseMode, out var browseMode))
        {
            this.BrowseMode = browseMode;
        }

        this.SelectedContinent = this.Continents.Contains(savedContinent ?? string.Empty)
            ? savedContinent
            : this.Continents.FirstOrDefault();
        this.SelectedTrack = this.VisibleTracks.FirstOrDefault(track => track.Id == savedTrackId) ?? this.SelectedTrack;
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
