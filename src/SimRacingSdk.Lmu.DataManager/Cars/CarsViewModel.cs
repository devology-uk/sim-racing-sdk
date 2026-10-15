using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Session;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.Lmu.DataManager.Cars;

public class CarsViewModel : CatalogPageViewModel<CarInfo, CarEditorViewModel>
{
    public CarsViewModel(
        IEntityRepository<CarInfo> carRepository,
        ICatalogGenerator<CarInfo> carProviderGenerator,
        ISessionStateStore sessionStateStore)
        : base("Cars", new CarEditorViewModel(), carRepository, carProviderGenerator, sessionStateStore)
    {
        this.Initialise();
    }

    protected override string DescribeGaps()
    {
        var missingModelIdCount = this.Entities.Count(car => car.ModelId is null);
        return $" - {missingModelIdCount} still missing a ModelId";
    }

    protected override string GroupOf(CarInfo entity)
    {
        return entity.Class;
    }

    protected override IEnumerable<CarInfo> Sort(IEnumerable<CarInfo> unsorted)
    {
        return unsorted.OrderBy(car => car.Manufacturer).ThenBy(car => car.DisplayName);
    }

    protected override string? ValidationProblem(CarInfo entity)
    {
        if(string.IsNullOrWhiteSpace(entity.DisplayName))
        {
            return "Display Name is empty";
        }

        return string.IsNullOrWhiteSpace(entity.Class) ? "Class is empty. Pick or type the class, then save again" : null;
    }
}
