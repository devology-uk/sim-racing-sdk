namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public record SetupFieldInfo
{
    // Decimal places the game shows, per Units setting - they differ per field, not just per unit
    // (Imperial Ride Height Adjust shows 2, Bump Stop Length 1, both inches). ImperialDecimals is
    // only set where Quantity converts; otherwise the display is the same in both.
    public int? Decimals { get; init; }

    // Exactly what the game's screen shows, in click order. The raw value for entry i is
    // RawMin + i * RawStep (0 and 1 when unset, e.g. Tyre Compound). This also covers a numeric
    // setting whose display isn't a linear scale of its raw value - e.g. Final Drive shows
    // 112 / raw as a ratio, so it's an Enum of "4.308:1", "3.862:1", "3.500:1" over raw 26..32 step 3.
    public List<string>? EnumValues { get; init; }

    // Only meaningful for an Enum field (e.g. Tyre Compound) - the game always shows an extra
    // "Auto" option ahead of the real compounds, displaying the resolved choice in parenthesis
    // (e.g. "Auto (Medium)"), so it isn't just another entry in EnumValues.
    public required bool HasAutoOption { get; init; }

    public int? ImperialDecimals { get; init; }
    public required SetupFieldKind Kind { get; init; }
    public double? Max { get; init; }
    public double? Min { get; init; }
    public required string Name { get; init; }
    public SetupFieldQuantity Quantity { get; init; }

    // The actual .vset parameter name(s) this field reads/writes, found by diffing saved setups
    // (a min/step/max trio per car - see project docs). A template using "{Corner}" (PerCorner,
    // e.g. "{Corner}-spring-rate" -> FL-/FR-/RL-/RR-spring-rate) or "{Axle}" (FrontRear, e.g.
    // "{Axle}-antirollbar" -> F-/R-antirollbar); a Single field's raw key has no placeholder at
    // all (e.g. "brake-bias", "final-drive"). Null until confirmed against a real .vset diff -
    // never guessed from the field's display Name, since raw casing/wording often differs
    // (e.g. display "Fuel Level" is raw "fuelLevel", not "fuel-level").
    public string? RawKey { get; init; }

    // The raw value(s) actually written to the .vset file - independent of Min/Max/Step, which
    // are the display values shown in-game and may be a different unit (e.g. display N/mm is
    // raw N/m, display bar is raw Pa). RawMin == RawMax (with Min == Max too) means the field is
    // present in the car's saved setups but never adjustable - see DefaultSetupFieldApplier's
    // notes on the alternative, a field that's absent from the .vset entirely for a given car.
    public double? RawMax { get; init; }
    public double? RawMin { get; init; }
    public double? RawStep { get; init; }

    public required SetupFieldScope Scope { get; init; }

    // Free text, not a fixed list - e.g. "Wheels"/"Aero" on Tyres & Chassis, "Differential"/
    // "Transmission" on Engine & Drivetrain - grouping within a tab, derived from what's typed
    // per car rather than hardcoded, the same reasoning as Track's Continent field.
    public required string Section { get; init; }

    public double? Step { get; init; }

    // The Metric label; the Imperial one follows from Quantity.
    public string? Unit { get; init; }
}
