namespace SimRacingSdk.DataManager.Shared.Storage;

public interface IEntityRepository<TEntity> where TEntity : ICatalogEntity
{
    void Delete(string id);
    IReadOnlyList<TEntity> GetAll();
    void Save(TEntity entity);
}
