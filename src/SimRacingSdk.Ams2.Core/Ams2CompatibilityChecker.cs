using SimRacingSdk.Ams2.Core.Abstractions;

namespace SimRacingSdk.Ams2.Core;

public class Ams2CompatibilityChecker : IAms2CompatibilityChecker
{
    private static Ams2CompatibilityChecker? singletonInstance;
    private readonly IAms2PathProvider ams2PathProvider;

    public Ams2CompatibilityChecker(IAms2PathProvider ams2PathProvider)
    {
        this.ams2PathProvider = ams2PathProvider;
    }

    public static Ams2CompatibilityChecker Instance =>
        singletonInstance ??= new Ams2CompatibilityChecker(Ams2PathProvider.Instance);

    public bool IsAms2Installed()
    {
        return Directory.Exists(this.ams2PathProvider.DocumentsFolderPath);
    }
}