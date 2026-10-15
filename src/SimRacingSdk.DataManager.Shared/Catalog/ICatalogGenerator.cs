namespace SimRacingSdk.DataManager.Shared.Catalog;

// Regenerates an entity's whole surface in the game's Core project (model, interface and provider)
// from the captured data. The only way catalog data reaches Core, and only ever on an explicit
// user action. Returns the provider's path.
public interface ICatalogGenerator<in TEntity>
{
    string Generate(IReadOnlyCollection<TEntity> entities);
}
