using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SimRacingSdk.Pmr.DataManager;

public static partial class Slug
{
    public static string Create(string value)
    {
        return NonAlphaNumeric().Replace(RemoveDiacritics(value).ToLowerInvariant(), "-").Trim('-');
    }

    // Decomposes an accented character into a base letter plus a combining mark (e.g. "ü" becomes
    // "u" + U+0308) and drops the mark, so a name like "Nürburgring" slugs to "nurburgring" rather
    // than "n-rburgring" - the whole point of a slug is to stay human-readable as a file name.
    private static string RemoveDiacritics(string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder();

        foreach(var character in normalized)
        {
            if(CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(character);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }

    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex NonAlphaNumeric();
}
