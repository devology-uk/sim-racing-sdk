using System.IO;
using System.Text.Json;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.R3e.DataManager.SetupMaps;

// Files are named by the game's model id, not the car's name, so a rename from Import From Game can't orphan a map.
public class SetupMapRepository : ISetupMapRepository
{
    private readonly string setupMapsFolder;

    public SetupMapRepository(IDataPathProvider dataPathProvider)
    {
        this.setupMapsFolder = dataPathProvider.GetEntityFolder("setupmaps");
    }

    public CapturedSetupMap? FindByCarGameId(int carGameId)
    {
        var path = this.PathFor(carGameId);
        return File.Exists(path) ? Read(path) : null;
    }

    public IReadOnlyList<CapturedSetupMap> GetAll()
    {
        return Directory.EnumerateFiles(this.setupMapsFolder, "*.json").Select(Read).ToList();
    }

    public void Save(CapturedSetupMap setupMap)
    {
        File.WriteAllText(this.PathFor(setupMap.CarGameId), SetupMapJson.Serialize(setupMap));
    }

    private static CapturedSetupMap Read(string path)
    {
        return JsonSerializer.Deserialize<CapturedSetupMap>(File.ReadAllText(path), SetupMapJson.Options)!;
    }

    private string PathFor(int carGameId)
    {
        return Path.Combine(this.setupMapsFolder, $"{carGameId}.json");
    }
}
