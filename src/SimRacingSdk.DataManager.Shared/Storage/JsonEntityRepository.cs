using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SimRacingSdk.DataManager.Shared.Storage;

// One JSON file per entity under data/<game>/<entityName>/, so a single edit shows up as a small,
// obvious git diff.
public class JsonEntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : ICatalogEntity
{
    // Relaxed escaping keeps names like "Autódromo José Carlos Pace" readable in the file.
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private readonly string folder;

    public JsonEntityRepository(IDataPathProvider dataPathProvider, string entityName)
    {
        this.folder = dataPathProvider.GetEntityFolder(entityName);
    }

    public void Delete(string id)
    {
        var path = this.PathFor(id);
        if(File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public IReadOnlyList<TEntity> GetAll()
    {
        return Directory.EnumerateFiles(this.folder, "*.json")
            .Select(path => JsonSerializer.Deserialize<TEntity>(File.ReadAllText(path))!)
            .ToList();
    }

    // Always writes to {entity.Id}.json - a caller changing a field the Id derives from must
    // Delete the old id itself (see CatalogPageViewModel.Save).
    public void Save(TEntity entity)
    {
        File.WriteAllText(this.PathFor(entity.Id), JsonSerializer.Serialize(entity, SerializerOptions));
    }

    private string PathFor(string id)
    {
        return Path.Combine(this.folder, $"{id}.json");
    }
}
