using SimRacingSdk.Pmr.Setups.Models.Maps;
using SimRacingSdk.Pmr.Setups.Models.Presentation;

namespace SimRacingSdk.Pmr.Setups.Services;

// Raw .vset units are Pa, N/m, m and litres; conversions confirmed against the game's own screens in
// both unit systems (docs/pmr.md, "Imperial units"). Imperial fuel is UK gallons, not US.
internal static class PmrUnitConverter
{
    private const double PascalsPerBar = 100000;
    private const double PascalsPerPsi = 6894.757;
    private const double NewtonMetersPerNewtonMillimeter = 1000;
    private const double NewtonMetersPerPoundInch = 175.1268;
    private const double MillimetersPerMeter = 1000;
    private const double MetersPerInch = 0.0254;
    private const double LitresPerUkGallon = 4.54609;

    public static string? ImperialUnit(PmrSetupFieldQuantity quantity)
    {
        return quantity switch
        {
            PmrSetupFieldQuantity.Pressure => "PSI",
            PmrSetupFieldQuantity.SpringRate => "lb/in",
            PmrSetupFieldQuantity.Length => "in",
            PmrSetupFieldQuantity.Volume => "gal",
            _ => null
        };
    }

    public static double ToDisplay(double rawValue, PmrSetupFieldQuantity quantity, PmrUnitSystem unitSystem)
    {
        return rawValue * DisplayUnitsPerRawUnit(quantity, unitSystem);
    }

    public static double ToRaw(double metricDisplayValue, PmrSetupFieldQuantity quantity)
    {
        return metricDisplayValue / DisplayUnitsPerRawUnit(quantity, PmrUnitSystem.Metric);
    }

    private static double DisplayUnitsPerRawUnit(PmrSetupFieldQuantity quantity, PmrUnitSystem unitSystem)
    {
        return unitSystem == PmrUnitSystem.Imperial ? ImperialUnitsPerRawUnit(quantity) : MetricUnitsPerRawUnit(quantity);
    }

    private static double ImperialUnitsPerRawUnit(PmrSetupFieldQuantity quantity)
    {
        return quantity switch
        {
            PmrSetupFieldQuantity.Pressure => 1 / PascalsPerPsi,
            PmrSetupFieldQuantity.SpringRate => 1 / NewtonMetersPerPoundInch,
            PmrSetupFieldQuantity.Length => 1 / MetersPerInch,
            PmrSetupFieldQuantity.Volume => 1 / LitresPerUkGallon,
            _ => 1
        };
    }

    private static double MetricUnitsPerRawUnit(PmrSetupFieldQuantity quantity)
    {
        return quantity switch
        {
            PmrSetupFieldQuantity.Pressure => 1 / PascalsPerBar,
            PmrSetupFieldQuantity.SpringRate => 1 / NewtonMetersPerNewtonMillimeter,
            PmrSetupFieldQuantity.Length => MillimetersPerMeter,
            _ => 1
        };
    }
}
