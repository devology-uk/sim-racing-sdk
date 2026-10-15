using SimRacingSdk.DataManager.Shared.Catalog;

namespace SimRacingSdk.Ams2.DataManager.Cars;

// The values already in use for the car fields that repeat across the catalog.
public class CarFieldSuggestions
{
    public CarFieldSuggestions(IReadOnlyCollection<CarInfo> cars)
    {
        this.Differentials = DistinctValues.Of(cars, car => car.Differential);
        this.DlcPacks = DistinctValues.Of(cars, car => car.DlcPack);
        this.Drivelines = DistinctValues.Of(cars, car => car.Driveline);
        this.Electronics = DistinctValues.Of(cars, car => car.Electronics);
        this.EnginePlacements = DistinctValues.Of(cars, car => car.EnginePlacement);
        this.Engines = DistinctValues.Of(cars, car => car.Engine);
        this.Manufacturers = DistinctValues.Of(cars, car => car.Manufacturer);
        this.Transmissions = DistinctValues.Of(cars, car => car.Transmission);
    }

    public IReadOnlyList<string> Differentials { get; }
    public IReadOnlyList<string> DlcPacks { get; }
    public IReadOnlyList<string> Drivelines { get; }
    public IReadOnlyList<string> Electronics { get; }
    public IReadOnlyList<string> EnginePlacements { get; }
    public IReadOnlyList<string> Engines { get; }
    public IReadOnlyList<string> Manufacturers { get; }
    public IReadOnlyList<string> Transmissions { get; }
}
