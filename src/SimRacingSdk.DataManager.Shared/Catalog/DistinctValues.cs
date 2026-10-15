namespace SimRacingSdk.DataManager.Shared.Catalog;

public static class DistinctValues
{
    // The values already used for a field, for an editable combo box - so curating a new entity picks
    // up the existing spelling ("6 Speed Sequential") rather than inventing a near-duplicate.
    public static IReadOnlyList<string> Of<TEntity>(IEnumerable<TEntity> entities, Func<TEntity, string?> selector)
    {
        return entities.Select(selector)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => value!)
            .Distinct()
            .OrderBy(value => value)
            .ToList();
    }
}
