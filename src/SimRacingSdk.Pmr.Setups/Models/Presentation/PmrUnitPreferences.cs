using SimRacingSdk.Pmr.Setups.Models.Maps;

namespace SimRacingSdk.Pmr.Setups.Models.Presentation;

// The unit system for each quantity a setup screen converts - the game sets these separately.
public record PmrUnitPreferences
{
    public static PmrUnitPreferences Imperial { get; } = AllIn(PmrUnitSystem.Imperial);
    public static PmrUnitPreferences Metric { get; } = AllIn(PmrUnitSystem.Metric);

    public PmrUnitSystem Length { get; init; }
    public PmrUnitSystem Pressure { get; init; }
    public PmrUnitSystem SpringRate { get; init; }
    public PmrUnitSystem Volume { get; init; }

    public static PmrUnitPreferences AllIn(PmrUnitSystem unitSystem)
    {
        return new PmrUnitPreferences
        {
            Length = unitSystem,
            Pressure = unitSystem,
            SpringRate = unitSystem,
            Volume = unitSystem
        };
    }

    public PmrUnitSystem For(PmrSetupFieldQuantity quantity)
    {
        return quantity switch
        {
            PmrSetupFieldQuantity.Length => this.Length,
            PmrSetupFieldQuantity.Pressure => this.Pressure,
            PmrSetupFieldQuantity.SpringRate => this.SpringRate,
            PmrSetupFieldQuantity.Volume => this.Volume,
            _ => PmrUnitSystem.Metric
        };
    }
}
