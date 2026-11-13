namespace SimRacingSdk.R3e.Setups.Models;

public record R3eSetupControlDefinition
{
    public required R3eSetupControl Control { get; init; }
    public required R3eSetupValueKind Kind { get; init; }

    // Empty for a unitless value such as Downforce or Map #.
    public required string MetricUnit { get; init; }

    // As the game labels the row.
    public required string Name { get; init; }

    public R3eSetupQuantity Quantity { get; init; }

    // The heading the row sits under on its tab, e.g. "Shared", "Wheels", "Third Spring".
    public required string Section { get; init; }

    public required R3eSetupTab Tab { get; init; }

    public string ImperialUnit => this.Quantity switch
    {
        R3eSetupQuantity.SpringRate => "lbs/in",
        R3eSetupQuantity.Pressure => "PSI",
        R3eSetupQuantity.Length => "inches",
        _ => this.MetricUnit
    };
}
