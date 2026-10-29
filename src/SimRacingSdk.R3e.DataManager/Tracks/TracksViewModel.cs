using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Session;
using SimRacingSdk.DataManager.Shared.Storage;
using SimRacingSdk.R3e.DataManager.GameData;

namespace SimRacingSdk.R3e.DataManager.Tracks;

public partial class TracksViewModel : CatalogPageViewModel<TrackInfo, TrackEditorViewModel>
{
    private const string NoCountryGroup = "(no country yet)";

    private readonly ICatalogImporter<TrackInfo> trackImporter;

    [ObservableProperty]
    private string importStatusMessage = string.Empty;

    [ObservableProperty]
    private bool isSelectableOnly = true;

    public TracksViewModel(
        IEntityRepository<TrackInfo> trackRepository,
        ICatalogGenerator<TrackInfo> trackProviderGenerator,
        ICatalogImporter<TrackInfo> trackImporter,
        ISessionStateStore sessionStateStore)
        : base("Tracks", new TrackEditorViewModel(), trackRepository, trackProviderGenerator, sessionStateStore)
    {
        this.trackImporter = trackImporter;
        this.Initialise();
    }

    protected override string DescribeGaps()
    {
        var selectableTracks = this.Entities.Where(track => !track.NotInGameSelector).ToList();
        var missingCountryCount = selectableTracks.Count(track => string.IsNullOrWhiteSpace(track.Country));
        var missingLengthCount = selectableTracks.SelectMany(track => track.Layouts).Count(layout => layout.LengthKm is null);
        var missingLocationCount = selectableTracks.Count(track => track.Latitude is null || track.Longitude is null);
        return $" - of {selectableTracks.Count} selectable tracks, {missingCountryCount} still missing a country, "
               + $"{missingLocationCount} a location, {missingLengthCount} layouts a length";
    }

    protected override bool IsShown(TrackInfo entity)
    {
        return !this.IsSelectableOnly || !entity.NotInGameSelector;
    }

    // Imported tracks have no country until one is captured, so they still need a group to browse.
    protected override string GroupOf(TrackInfo entity)
    {
        return string.IsNullOrWhiteSpace(entity.Country) ? NoCountryGroup : entity.Country;
    }

    protected override IEnumerable<TrackInfo> Sort(IEnumerable<TrackInfo> unsorted)
    {
        return unsorted.OrderBy(track => track.Name);
    }

    protected override string? ValidationProblem(TrackInfo entity)
    {
        if(entity.GameId <= 0)
        {
            return "it has no game id. Tracks come from Import From Game";
        }

        return entity.Country == NoCountryGroup ? "Country can't be the placeholder group name" : null;
    }

    partial void OnIsSelectableOnlyChanged(bool value)
    {
        this.RefreshShownEntities();
    }

    [RelayCommand]
    private void ImportFromGame()
    {
        try
        {
            var result = this.trackImporter.Import();
            this.Reload();
            this.ImportStatusMessage = result.Describe("tracks");
        }
        catch(Exception exception) when(exception is IOException or JsonException or KeyNotFoundException)
        {
            this.ImportStatusMessage = $"Import failed - {exception.Message}";
        }
    }
}
