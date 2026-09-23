namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public static class SetupFieldScopeOptions
{
    public static IReadOnlyList<SetupFieldScope> All { get; } = Enum.GetValues<SetupFieldScope>();
}
