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

        return entity.Layouts.Any(layout => layout.Name == "Default") ? null : "it needs a layout named Default";
    }
}
