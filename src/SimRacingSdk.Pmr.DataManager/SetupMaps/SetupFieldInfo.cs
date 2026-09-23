namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public record SetupFieldInfo
{
    public List<string>? EnumValues { get; init; }

    // Only meaningful for an Enum field (e.g. Tyre Compound) - the game always shows an extra
    // "Auto" option ahead of the real compounds, displaying the resolved choice in parenthesis
    // (e.g. "Auto (Medium)"), so it isn't just another entry in EnumValues.
    public required bool HasAutoOption { get; init; }

    public required SetupFieldKind Kind { get; init; }
    public double? Max { get; init; }
    public double? Min { get; init; }
    public required string Name { get; init; }
    public required SetupFieldScope Scope { get; init; }

    // Free text, not a fixed list - e.g. "Wheels"/"Aero" on Tyres & Chassis, "Differential"/
    // "Transmission" on Engine & Drivetrain - grouping within a tab, derived from what's typed
    // per car rather than hardcoded, the same reasoning as Track's Continent field.
    public required string Section { get; init; }

    public double? Step { get; init; }
    public string? Unit { get; init; }
}
