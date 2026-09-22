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

    // Always writes to {car.Id}.json, derived fresh from the car's current Manufacturer/Name -
    // renaming either one leaves the old file behind rather than renaming it in place. Not handled
    // yet since there's no edit UI to trigger it; deal with it when that exists.
    public void Save(CarInfo car)
    {
        File.WriteAllText(this.PathFor(car.Id), JsonSerializer.Serialize(car, SerializerOptions));
    }

    private string PathFor(string id)
    {
        return Path.Combine(this.carsFolder, $"{id}.json");
    }
}
