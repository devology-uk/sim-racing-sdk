using SimRacingSdk.DataManager.Shared.Storage;
using SimRacingSdk.R3e.DataManager.GameData;

namespace SimRacingSdk.R3e.DataManager.Cars;

public class CarCatalogImporter : ICatalogImporter<CarInfo>
{
    private readonly IGameCatalogReader gameCatalogReader;
    private readonly IEntityRepository<CarInfo> repository;

    public CarCatalogImporter(IGameCatalogReader gameCatalogReader, IEntityRepository<CarInfo> repository)
    {
        this.gameCatalogReader = gameCatalogReader;
        this.repository = repository;
    }

    public ImportResult Import()
    {
        var gameCars = this.gameCatalogReader.Read().Cars;
        var capturedCars = this.repository.GetAll().ToDictionary(car => car.GameId);
        var merge = new CatalogMerge<CarInfo>(this.repository);

        foreach(var gameCar in gameCars)
        {
            var capturedCar = capturedCars.GetValueOrDefault(gameCar.GameId);
            merge.Apply(capturedCar, this.Merge(capturedCar, gameCar));
        }

        var gameIds = gameCars.Select(car => car.GameId).ToHashSet();
        return merge.Result(capturedCars.Keys.Count(gameId => !gameIds.Contains(gameId)));
    }

    private CarInfo Merge(CarInfo? capturedCar, GameCar gameCar)
    {
        if(capturedCar is null)
        {
            return new CarInfo
            {
                Class = gameCar.Class,
                GameId = gameCar.GameId,
                Manufacturer = gameCar.Manufacturer,
                Name = gameCar.Name
            };
        }

        return capturedCar with
        {
            Class = gameCar.Class,
            Manufacturer = gameCar.Manufacturer,
            Name = gameCar.Name
        };
    }
}
