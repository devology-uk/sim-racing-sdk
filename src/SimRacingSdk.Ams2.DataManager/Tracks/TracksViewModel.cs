using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Session;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.Ams2.DataManager.Tracks;

public class TracksViewModel : CatalogPageViewModel<TrackInfo, TrackEditorViewModel>
{
    private LayoutFieldSuggestions suggestions = new([]);

    public TracksViewModel(
        IEntityRepository<TrackInfo> trackRepository,
        ICatalogGenerator<TrackInfo> trackProviderGenerator,
        ISessionStateStore sessionStateStore)
        : base("Tracks", new TrackEditorViewModel(), trackRepository, trackProviderGenerator, sessionStateStore)
    {
        this.Initialise();
    }

    public LayoutFieldSuggestions Suggestions
    {
        get => this.suggestions;
        private set => this.SetProperty(ref this.suggestions, value);
    }

    protected override string GroupOf(TrackInfo entity)
    {
        return entity.Country;
    }

    protected override void OnEntitiesChanged()
    {
        this.Suggestions = new LayoutFieldSuggestions(this.Entities);
    }

    protected override IEnumerable<TrackInfo> Sort(IEnumerable<TrackInfo> unsorted)
    {
        return unsorted.OrderBy(track => track.ShortName);
    }

    protected override string? ValidationProblem(TrackInfo entity)
    {
        if(string.IsNullOrWhiteSpace(entity.ShortName))
        {
            return "Short Name is empty";
        }

        if(string.IsNullOrWhiteSpace(entity.Country))
        {
            return "Country is empty";
        }

        var tracks = this.Entities.Where(track => track.Id != entity.Id && track.Id != this.SelectedItem?.Id)
            .Append(entity)
            .ToList();

        var duplicateTrackId = FirstDuplicate(tracks.SelectMany(track => track.Layouts).Select(layout => layout.EffectiveAms2TrackId));
        if(duplicateTrackId is not null)
        {
            return $"the AMS2 track id \"{duplicateTrackId}\" is used by more than one layout";
        }

        var duplicateLocation = FirstDuplicate(tracks.SelectMany(track => track.RawTrackLocations));
        return duplicateLocation is null ? null : $"the raw track location \"{duplicateLocation}\" is listed on more than one track";
    }

    // AMS2 reports one id per layout and one location per track, so each must lead to exactly one entry.
    private static string? FirstDuplicate(IEnumerable<string> values)
    {
        return values.GroupBy(value => value, StringComparer.InvariantCultureIgnoreCase)
            .FirstOrDefault(group => group.Count() > 1)
            ?.Key;
    }
}
