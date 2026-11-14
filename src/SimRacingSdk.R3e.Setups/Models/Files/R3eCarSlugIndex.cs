using System.Text.RegularExpressions;
using SimRacingSdk.R3e.Core.Abstractions;
using SimRacingSdk.R3e.Core.Models;

namespace SimRacingSdk.R3e.Setups.Models.Files;

// Maps the slug the game puts at the front of a setup file name back to the catalog cars it can mean. Runs of
// underscores are collapsed on both sides, because it isn't known how the game slugs names with brackets or
// dashes next to spaces.
public partial class R3eCarSlugIndex
{
    public const char Separator = '_';

    private readonly IReadOnlyDictionary<string, IReadOnlyList<R3eCarInfo>> carsBySlug;

    public R3eCarSlugIndex(IR3eCarInfoProvider carInfoProvider)
    {
        this.carsBySlug = carInfoProvider.GetCarInfos()
                                         .Where(car => !string.IsNullOrWhiteSpace(car.Name))
                                         .GroupBy(car => Slug(car.Name))
                                         .Where(group => group.Key.Length > 0)
                                         .ToDictionary(group => group.Key,
                                             group => (IReadOnlyList<R3eCarInfo>)group.ToList());
    }

    public static string Slug(string text)
    {
        var slug = NonAlphanumeric().Replace(text.ToLowerInvariant(), Separator.ToString());
        return RepeatedSeparator().Replace(slug, Separator.ToString())
                                  .Trim(Separator);
    }

    public IReadOnlyList<R3eCarInfo> CarsFor(string carSlug)
    {
        return this.carsBySlug.TryGetValue(carSlug, out var cars) ? cars : [];
    }

    public string? FindLongestCarSlugPrefix(string fileSlug)
    {
        return this.carsBySlug.Keys
                   .Where(carSlug => fileSlug.StartsWith(carSlug + Separator, StringComparison.Ordinal))
                   .MaxBy(carSlug => carSlug.Length);
    }

    [GeneratedRegex("[^a-z0-9]")]
    private static partial Regex NonAlphanumeric();

    [GeneratedRegex("_{2,}")]
    private static partial Regex RepeatedSeparator();
}
