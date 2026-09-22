using System.Text.RegularExpressions;

namespace SimRacingSdk.Pmr.DataManager;

public static partial class Slug
{
    public static string Create(string value)
    {
        return NonAlphaNumeric().Replace(value.ToLowerInvariant(), "-").Trim('-');
    }

    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex NonAlphaNumeric();
}
