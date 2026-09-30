namespace SimRacingSdk.Pmr.Setups.Models.Maps;

// The game greys a setting out unless another setting holds one of these raw values - e.g. Regen
// Limit only applies while Regen Mode is Full. The saved setup keeps the greyed-out value anyway.
// RawKey may use the same {Corner}/{Axle} placeholders as a field's own RawKey.
public record PmrSetupFieldCondition
{
    public required string RawKey { get; init; }
    public IReadOnlyList<double> RawValues { get; init; } = [];
}
