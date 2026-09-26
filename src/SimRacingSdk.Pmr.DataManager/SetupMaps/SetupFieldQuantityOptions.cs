namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public static class SetupFieldQuantityOptions
{
    public static IReadOnlyList<SetupFieldQuantity> All { get; } = Enum.GetValues<SetupFieldQuantity>();
}
