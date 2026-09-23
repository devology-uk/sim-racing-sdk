using System.IO;
using System.Text.Json;
using SimRacingSdk.Pmr.DataManager.Storage;

namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public class SetupMapRepository : ISetupMapRepository
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    private readonly string setupMapsFolder;

    public SetupMapRepository(IDataPathProvider dataPathProvider)
    {
        this.setupMapsFolder = dataPathProvider.GetEntityFolder("setupmaps");
    }

    public PmrSetupMap? FindByCarId(string carId)
    {
        var path = this.PathFor(carId);
        return File.Exists(path) ? JsonSerializer.Deserialize<PmrSetupMap>(File.ReadAllText(path)) : null;
    }

    public void Save(PmrSetupMap setupMap)
    {
        File.WriteAllText(this.PathFor(setupMap.CarId), JsonSerializer.Serialize(setupMap, SerializerOptions));
    }

    private string PathFor(string carId)
    {
        return Path.Combine(this.setupMapsFolder, $"{carId}.json");
    }
}
