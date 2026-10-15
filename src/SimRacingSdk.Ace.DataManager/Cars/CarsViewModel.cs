using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Session;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.Ace.DataManager.Cars;

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
        return entity.Categories;
    }

    protected override void OnEntitiesChanged()
    {
        this.Suggestions = new CarFieldSuggestions(this.Entities);
    }

    protected override IEnumerable<CarInfo> Sort(IEnumerable<CarInfo> unsorted)
    {
        return unsorted.OrderBy(car => car.Manufacturer).ThenBy(car => car.Name).ThenBy(car => car.Variant);
    }

    protected override string? ValidationProblem(CarInfo entity)
    {
        if(string.IsNullOrWhiteSpace(entity.Manufacturer) || string.IsNullOrWhiteSpace(entity.Name))
        {
            return "Manufacturer and Name are both needed";
        }

        if(string.IsNullOrWhiteSpace(entity.ModelId))
        {
            return "Model Id is empty - it's the preset id from the Dedicated Server's cars.json";
        }

        return string.IsNullOrWhiteSpace(entity.Categories)
            ? "Categories is empty. Pick or type the category, then save again"
            : null;
    }
}
