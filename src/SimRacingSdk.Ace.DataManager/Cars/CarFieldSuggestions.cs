using SimRacingSdk.DataManager.Shared.Catalog;

namespace SimRacingSdk.Ace.DataManager.Cars;

// The values already in use for the car fields that repeat across the catalog.
public class CarFieldSuggestions
{
    public CarFieldSuggestions(IReadOnlyCollection<CarInfo> cars)
    {
        this.Bodies = DistinctValues.Of(cars, car => car.Body);
        this.DriveLayouts = DistinctValues.Of(cars, car => car.DriveLayout);
        this.EngineLayouts = DistinctValues.Of(cars, car => car.EngineLayout);
        this.Engines = DistinctValues.Of(cars, car => car.Engine);
        this.Manufacturers = DistinctValues.Of(cars, car => car.Manufacturer);
        this.RacingClasses = DistinctValues.Of(cars, car => car.RacingClass);
    }

    public IReadOnlyList<string> Bodies { get; }
    public IReadOnlyList<string> DriveLayouts { get; }
    public IReadOnlyList<string> EngineLayouts { get; }
    public IReadOnlyList<string> Engines { get; }
    public IReadOnlyList<string> Manufacturers { get; }
    public IReadOnlyList<string> RacingClasses { get; }
}
