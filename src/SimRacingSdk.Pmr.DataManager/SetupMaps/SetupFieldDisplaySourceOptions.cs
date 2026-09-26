namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public static class SetupFieldDisplaySourceOptions
{
    public static IReadOnlyList<SetupFieldDisplaySource> All { get; } = Enum.GetValues<SetupFieldDisplaySource>();
}
