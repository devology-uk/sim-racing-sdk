namespace SimRacingSdk.Pmr.Setups.Models.Maps;

// The root of the embedded pmr-setup-maps.json.
public record PmrSetupMapCatalog
{
    public IReadOnlyList<PmrSetupMap> Maps { get; init; } = [];
}
