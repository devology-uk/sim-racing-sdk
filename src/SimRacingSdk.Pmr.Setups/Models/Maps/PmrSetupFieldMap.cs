namespace SimRacingSdk.Pmr.Setups.Models.Maps;

// One row of a car's setup map, as captured in SimRacingSdk.Pmr.DataManager (see docs/pmr.md).
// Min/Max/Step/Unit/Decimals describe the Metric screen; Raw* describe what the .vset stores.
public record PmrSetupFieldMap
{
    public int? Decimals { get; init; }
    public PmrSetupFieldDisplayFormat DisplayFormat { get; init; }
    public PmrSetupFieldDisplaySource DisplaySource { get; init; }

    // Exactly what the screen shows, in click order; entry i is raw RawMin + i * RawStep.
    public IReadOnlyList<string>? EnumValues { get; init; }

    public bool HasAutoOption { get; init; }
    public int? ImperialDecimals { get; init; }
    public PmrSetupFieldKind Kind { get; init; }
    public double? Max { get; init; }
    public double? Min { get; init; }
    public required string Name { get; init; }
    public PmrSetupFieldQuantity Quantity { get; init; }

    // A .vset parameter name, or a template: "{Corner}" becomes FL/FR/RL/RR, "{Axle}" becomes F/R.
    public string? RawKey { get; init; }

    public double? RawMax { get; init; }
    public double? RawMin { get; init; }
    public double? RawStep { get; init; }
    public PmrSetupFieldScope Scope { get; init; }
    public required string Section { get; init; }
    public double? Step { get; init; }
    public string? Unit { get; init; }

    public bool IsFixed => this.Kind == PmrSetupFieldKind.Numeric && this.Min is not null && this.Min == this.Max;
}
