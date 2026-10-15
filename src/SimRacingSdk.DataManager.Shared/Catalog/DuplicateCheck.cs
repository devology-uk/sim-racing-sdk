namespace SimRacingSdk.DataManager.Shared.Catalog;

public static class DuplicateCheck
{
    // Only clashes involving the entity being saved count - an existing clash elsewhere in the catalog
    // mustn't block saving an unrelated entity.
    public static string? FirstClash(IEnumerable<string> ownValues, IEnumerable<string> otherValues, StringComparer comparer)
    {
        var others = new HashSet<string>(otherValues, comparer);
        var seen = new HashSet<string>(comparer);
        return ownValues.FirstOrDefault(value => others.Contains(value) || !seen.Add(value));
    }
}
