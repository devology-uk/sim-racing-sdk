using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Session;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.Ams2.DataManager.Cars;

public class CarsViewModel : CatalogPageViewModel<CarInfo, CarEditorViewModel>
{
    private CarFieldSuggestions suggestions = new([]);

    public CarsViewModel(
        IEntityRepository<CarInfo> carRepository,
        ICatalogGenerator<CarInfo> carProviderGenerator,
        ISessionStateStore sessionStateStore)
        : base("Cars", new CarEditorViewModel(), carRepository, carProviderGenerator, sessionStateStore)
    {
        this.Initialise();
    }

    public CarFieldSuggestions Suggestions
    {
        get => this.suggestions;
        private set => this.SetProperty(ref this.suggestions, value);
    }

    protected override string GroupOf(CarInfo entity)
    {
        return entity.Class;
    }

    protected override void OnEntitiesChanged()
    {
        this.Suggestions = new CarFieldSuggestions(this.Entities);
    }

    protected override IEnumerable<CarInfo> Sort(IEnumerable<CarInfo> unsorted)
    {
        return unsorted.OrderBy(car => car.Manufacturer).ThenBy(car => car.Model);
    }

    protected override string? ValidationProblem(CarInfo entity)
    {
        if(string.IsNullOrWhiteSpace(entity.Model))
        {
            return "Model is empty";
        }

        return string.IsNullOrWhiteSpace(entity.Class) ? "Class is empty. Pick or type the class, then save again" : null;
    }
}
