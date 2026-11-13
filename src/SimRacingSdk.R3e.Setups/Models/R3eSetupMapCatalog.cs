namespace SimRacingSdk.R3e.Setups.Models;

// The root of the embedded r3e-setup-maps.json.
public record R3eSetupMapCatalog
{
    public IReadOnlyList<R3eSetupMap> Maps { get; init; } = [];
}
