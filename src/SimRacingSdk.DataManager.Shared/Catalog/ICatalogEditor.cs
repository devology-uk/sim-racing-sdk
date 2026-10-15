namespace SimRacingSdk.DataManager.Shared.Catalog;

// A mutable, bindable working copy of a catalog entity - the entity itself stays an immutable
// record (its required init-only properties can't be targeted by TwoWay bindings), so editing
// happens here and is only turned back into an entity when the user explicitly saves.
public interface ICatalogEditor<TEntity>
{
    void LoadFrom(TEntity? entity);

    // Pre-fills the grouping field of a new entity with the group being browsed.
    void StartInGroup(string group);

    TEntity ToEntity();
}
