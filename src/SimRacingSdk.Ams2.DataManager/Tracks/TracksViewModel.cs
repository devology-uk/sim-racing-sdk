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

        var otherTracks = this.Entities.Where(track => track.Id != entity.Id && track.Id != this.SelectedItem?.Id).ToList();

        var clashingTrackId = DuplicateCheck.FirstClash(
            entity.Layouts.Select(layout => layout.EffectiveAms2TrackId),
            otherTracks.SelectMany(track => track.Layouts).Select(layout => layout.EffectiveAms2TrackId),
            StringComparer.Ordinal);
        if(clashingTrackId is not null)
        {
            return $"the AMS2 track id \"{clashingTrackId}\" is used by more than one layout";
        }

        var clashingLocation = DuplicateCheck.FirstClash(
            entity.RawTrackLocations,
            otherTracks.SelectMany(track => track.RawTrackLocations),
            StringComparer.InvariantCultureIgnoreCase);
        return clashingLocation is null ? null : $"the raw track location \"{clashingLocation}\" is listed on more than one track";
    }
}
