using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Session;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.Pmr.DataManager.Tracks;

// Browses "By Continent" or "All Tracks", mirroring the game's own track-select screen.
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

    protected override string DescribeGaps()
    {
        var missingGameIdCount = this.Entities.Count(track => string.IsNullOrWhiteSpace(track.GameId));
        return $" - {missingGameIdCount} still missing a GameId";
    }

    protected override string GroupOf(TrackInfo entity)
    {
        return entity.Continent;
    }

    protected override IEnumerable<TrackInfo> Sort(IEnumerable<TrackInfo> unsorted)
    {
        return unsorted.OrderBy(track => track.Name);
    }
}
