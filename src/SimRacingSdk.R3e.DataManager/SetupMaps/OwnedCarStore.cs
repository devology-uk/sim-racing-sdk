using System.IO;
using System.Text.Json;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.R3e.DataManager.SetupMaps;

// One file beside the entity folders (data/r3e/owned-cars.json), holding the owned cars' game ids.
public class OwnedCarStore : IOwnedCarStore
{
    private readonly SortedSet<int> ownedCarGameIds;
    private readonly string path;

    public OwnedCarStore(IDataPathProvider dataPathProvider)
    {
        var gameDataFolder = Path.GetDirectoryName(dataPathProvider.GetEntityFolder("setupmaps"))!;
        this.path = Path.Combine(gameDataFolder, "owned-cars.json");
        this.ownedCarGameIds = this.Load();
    }

    public bool IsOwned(int carGameId)
    {
        return this.ownedCarGameIds.Contains(carGameId);
    }

    public void SetOwned(int carGameId, bool isOwned)
    {
        var changed = isOwned ? this.ownedCarGameIds.Add(carGameId) : this.ownedCarGameIds.Remove(carGameId);
        if(changed)
        {
            File.WriteAllText(this.path, JsonSerializer.Serialize(this.ownedCarGameIds, SetupMapJson.Options));
        }
    }

    private SortedSet<int> Load()
    {
        return File.Exists(this.path)
            ? new SortedSet<int>(JsonSerializer.Deserialize<List<int>>(File.ReadAllText(this.path)) ?? [])
            : [];
    }
}
