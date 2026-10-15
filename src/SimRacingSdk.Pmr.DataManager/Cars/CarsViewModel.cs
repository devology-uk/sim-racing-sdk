using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Session;
using SimRacingSdk.DataManager.Shared.Storage;
using SimRacingSdk.Pmr.DataManager.SetupMaps;

namespace SimRacingSdk.Pmr.DataManager.Cars;

// Browses "By Class" or "All Cars", mirroring the game's own car-select screen.
public class CarsViewModel : CatalogPageViewModel<CarInfo, CarEditorViewModel>
{
    private readonly ISetupMapRepository setupMapRepository;

    public CarsViewModel(
        IEntityRepository<CarInfo> carRepository,
        ICatalogGenerator<CarInfo> carProviderGenerator,
        ISetupMapRepository setupMapRepository,
        ISessionStateStore sessionStateStore)
        : base("Cars", new CarEditorViewModel(), carRepository, carProviderGenerator, sessionStateStore)
    {
        this.setupMapRepository = setupMapRepository;
        this.Initialise();
    }

    protected override string DescribeGaps()
    {
        var missingGameIdCount = this.Entities.Count(car => string.IsNullOrWhiteSpace(car.GameId));
        return $" - {missingGameIdCount} still missing a GameId";
    }

    protected override string GroupOf(CarInfo entity)
    {
        return entity.VehicleClass;
    }

    protected override void OnItemRenamed(string originalId, string newId)
    {
        this.setupMapRepository.ReassignCar(originalId, newId);
    }

    protected override IEnumerable<CarInfo> Sort(IEnumerable<CarInfo> unsorted)
    {
        return unsorted.OrderBy(car => car.Manufacturer).ThenBy(car => car.Name);
    }

    protected override string? ValidationProblem(CarInfo entity)
    {
        return string.IsNullOrWhiteSpace(entity.VehicleClass)
            ? "Vehicle Class is empty. Pick or type the class, then save again"
            : null;
    }
}
