using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Session;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.Lmu.DataManager.Tracks;

public class TracksViewModel : CatalogPageViewModel<TrackInfo, TrackEditorViewModel>
{
    public TracksViewModel(
        IEntityRepository<TrackInfo> trackRepository,
        ICatalogGenerator<TrackInfo> trackProviderGenerator,
        ISessionStateStore sessionStateStore)
        : base("Tracks", new TrackEditorViewModel(), trackRepository, trackProviderGenerator, sessionStateStore)
    {
        this.Initialise();
    }

    protected override string GroupOf(TrackInfo entity)
    {
        return entity.Country;
    }

    protected override IEnumerable<TrackInfo> Sort(IEnumerable<TrackInfo> unsorted)
    {
        return unsorted.OrderBy(track => track.Name);
    }

    protected override string? ValidationProblem(TrackInfo entity)
    {
        if(string.IsNullOrWhiteSpace(entity.Name))
        {
            return "Name is empty";
        }

        if(string.IsNullOrWhiteSpace(entity.Country))
        {
            return "Country is empty";
        }

        if(entity.Layouts.All(layout => layout.Name != "Default"))
        {
            return "it needs a layout named Default";
        }

        var duplicateRawTrackName = this.FindDuplicateRawTrackName(entity);
        return duplicateRawTrackName is null
            ? null
            : $"the raw track name \"{duplicateRawTrackName}\" is listed on more than one layout";
    }

    // A raw track name must lead to exactly one layout, in this track or any other.
    private string? FindDuplicateRawTrackName(TrackInfo entity)
    {
        var otherTracks = this.Entities.Where(track => track.Id != entity.Id && track.Id != this.SelectedItem?.Id);
        return DuplicateCheck.FirstClash(
            entity.Layouts.SelectMany(layout => layout.RawTrackNames),
            otherTracks.SelectMany(track => track.Layouts).SelectMany(layout => layout.RawTrackNames),
            StringComparer.InvariantCultureIgnoreCase);
    }
}
