using SimRacingSdk.R3e.Core.Models;

namespace SimRacingSdk.R3e.Setups.Models.Files;

// The game names a saved setup <car>_<setup name>.svm, both slugged: lower case with every other character an
// underscore. Several cars can share a slug (the same car in different series or years), so a name resolves to
// all of them. A file whose car slug isn't in the catalog keeps its whole name as both car and setup name.
public record R3eSetupFileName
{
    public const string Extension = ".svm";

    public required IReadOnlyList<R3eCarInfo> Cars { get; init; }
    public required string CarSlug { get; init; }
    public required string FileName { get; init; }
    public required string SetupName { get; init; }

    public string CarDisplayName => this.Cars.Count > 0 ? this.Cars[0].Name : this.CarSlug;

    public static R3eSetupFileName Parse(string fileName, R3eCarSlugIndex carSlugIndex)
    {
        var slug = R3eCarSlugIndex.Slug(Path.GetFileNameWithoutExtension(fileName));
        var carSlug = carSlugIndex.FindLongestCarSlugPrefix(slug);
        if(carSlug == null)
        {
            return new R3eSetupFileName
            {
                Cars = [],
                CarSlug = slug,
                FileName = fileName,
                SetupName = ToDisplayText(slug)
            };
        }

        return new R3eSetupFileName
        {
            Cars = carSlugIndex.CarsFor(carSlug),
            CarSlug = carSlug,
            FileName = fileName,
            SetupName = ToDisplayText(slug[(carSlug.Length + 1)..])
        };
    }

    private static string ToDisplayText(string slug)
    {
        return slug.Replace(R3eCarSlugIndex.Separator, ' ');
    }
}
