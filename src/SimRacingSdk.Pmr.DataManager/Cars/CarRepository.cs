using System.IO;
using System.Text.Json;
using SimRacingSdk.Pmr.DataManager.Storage;

namespace SimRacingSdk.Pmr.DataManager.Cars;

public class CarRepository : ICarRepository
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    private readonly string carsFolder;

    public CarRepository(IDataPathProvider dataPathProvider)
    {
        this.carsFolder = dataPathProvider.GetEntityFolder("cars");
    }

    public void Delete(string id)
    {
        var path = this.PathFor(id);
        if(File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public IReadOnlyList<CarInfo> GetAll()
    {
        return Directory.EnumerateFiles(this.carsFolder, "*.json")
            .Select(path => JsonSerializer.Deserialize<CarInfo>(File.ReadAllText(path))!)
            .ToList();
    }

    // Always writes to {car.Id}.json, derived fresh from Manufacturer/Name/Year - a caller changing
    // any of those must Delete the old id itself (see CarsViewModel.SaveCar).
    public void Save(CarInfo car)
    {
        File.WriteAllText(this.PathFor(car.Id), JsonSerializer.Serialize(car, SerializerOptions));
    }

    private string PathFor(string id)
    {
        return Path.Combine(this.carsFolder, $"{id}.json");
    }
}
