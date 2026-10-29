using SimRacingSdk.DataManager.Shared.Catalog;

namespace SimRacingSdk.R3e.DataManager.Cars;

// The values already in use for the car fields that repeat across the catalog.
public class CarFieldSuggestions
{
    public CarFieldSuggestions(IReadOnlyCollection<CarInfo> cars)
    {
        this.EngineLayouts = DistinctValues.Of(cars, car => car.EngineLayout);
    }

    public IReadOnlyList<string> EngineLayouts { get; }
}
