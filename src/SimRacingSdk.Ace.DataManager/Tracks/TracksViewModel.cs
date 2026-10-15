using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Session;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.Ace.DataManager.Tracks;

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
        return entity.Continent;
    }

    protected override IEnumerable<TrackInfo> Sort(IEnumerable<TrackInfo> unsorted)
    {
        return unsorted.OrderBy(track => track.ShortName);
    }

    protected override string? ValidationProblem(TrackInfo entity)
    {
        if(string.IsNullOrWhiteSpace(entity.Name))
        {
            return "Name is empty";
        }

        if(string.IsNullOrWhiteSpace(entity.Continent))
        {
            return "Continent is empty";
        }

        var otherTracks = this.Entities.Where(track => track.Id != entity.Id && track.Id != this.SelectedItem?.Id);
        var clashingShortName = DuplicateCheck.FirstClash([entity.ShortName],
            otherTracks.Select(track => track.ShortName),
            StringComparer.Ordinal);
        if(clashingShortName is not null)
        {
            return $"another track already has the Short Name \"{clashingShortName}\"";
        }

        var clashingLayout = DuplicateCheck.FirstClash(entity.Layouts.Select(layout => layout.Name), [], StringComparer.Ordinal);
        return clashingLayout is null ? null : $"the layout \"{clashingLayout}\" is listed twice";
    }
}
