using System.Text.Json;
using SimRacingSdk.DataManager.Shared.Storage;

namespace SimRacingSdk.R3e.DataManager.GameData;

// Saves one merged entity and counts the outcome. Entities are compared by their JSON, which is
// what's stored, so nested lists compare by content rather than by reference.
public class CatalogMerge<TEntity> where TEntity : class, ICatalogEntity
{
    private readonly IEntityRepository<TEntity> repository;
    private int added;
    private int unchanged;
    private int updated;

    public CatalogMerge(IEntityRepository<TEntity> repository)
    {
        this.repository = repository;
    }

    public void Apply(TEntity? existing, TEntity merged)
    {
        if(existing is not null && HaveSameContent(existing, merged))
        {
            this.unchanged++;
            return;
        }

        this.repository.Save(merged);
        if(existing is null)
        {
            this.added++;
            return;
        }

        if(existing.Id != merged.Id)
        {
            this.repository.Delete(existing.Id);
        }

        this.updated++;
    }

    public ImportResult Result(int notInGame)
    {
        return new ImportResult
        {
            Added = this.added,
            NotInGame = notInGame,
            Unchanged = this.unchanged,
            Updated = this.updated
        };
    }

    private static bool HaveSameContent(TEntity first, TEntity second)
    {
        return JsonSerializer.Serialize(first) == JsonSerializer.Serialize(second);
    }
}
