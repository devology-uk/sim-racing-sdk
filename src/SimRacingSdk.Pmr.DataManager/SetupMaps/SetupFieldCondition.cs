namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

// The game greys a field out unless another setting holds one of these raw values - e.g. Regen
// Limit only applies while Regen Mode is Full. The .vset keeps the greyed-out value regardless.
// RawKey may use the same {Corner}/{Axle} placeholders as the field's own RawKey.
public record SetupFieldCondition
{
    public required string RawKey { get; init; }
    public required List<double> RawValues { get; init; }
}
