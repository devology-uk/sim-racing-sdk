using System.Globalization;
using SimRacingSdk.Pmr.Setups.Models.Maps;
using SimRacingSdk.Pmr.Setups.Models.Presentation;

namespace SimRacingSdk.Pmr.Setups.Services;

// Writes a display value the way the game's screen does: "75°", "100%", "220 N/mm", "21.0 PSI",
// "56.0F / 44.0R", "2.385 : 1".
internal static class PmrSetupValueFormatter
{
    private const string DegreesUnit = "deg";
    private const string PercentUnit = "%";
    private const double WholeBrakeBias = 100;

    public static string Format(double displayValue, PmrSetupFieldMap field, PmrUnitSystem unitSystem)
    {
        var decimals = DecimalsFor(field, unitSystem);
        return field.DisplayFormat switch
        {
            PmrSetupFieldDisplayFormat.FrontRearSplit =>
                $"{Number(displayValue, decimals)}F / {Number(WholeBrakeBias - displayValue, decimals)}R",
            PmrSetupFieldDisplayFormat.Ratio => AsRatio(Number(displayValue, decimals)),
            _ => WithUnit(Number(displayValue, decimals), UnitFor(field, unitSystem))
        };
    }

    public static string FormatEnumValue(string enumValue, PmrSetupFieldMap field)
    {
        return field.DisplayFormat == PmrSetupFieldDisplayFormat.Ratio ? AsRatio(enumValue) : enumValue;
    }

    private static string AsRatio(string number)
    {
        return $"{number} : 1";
    }

    private static int? DecimalsFor(PmrSetupFieldMap field, PmrUnitSystem unitSystem)
    {
        return IsConverted(field, unitSystem) ? field.ImperialDecimals ?? field.Decimals : field.Decimals;
    }

    private static bool IsConverted(PmrSetupFieldMap field, PmrUnitSystem unitSystem)
    {
        return unitSystem == PmrUnitSystem.Imperial && field.Quantity != PmrSetupFieldQuantity.None;
    }

    private static string Number(double value, int? decimals)
    {
        var format = decimals is null ? "0.###" : "F" + decimals;
        return value.ToString(format, CultureInfo.InvariantCulture);
    }

    private static string? UnitFor(PmrSetupFieldMap field, PmrUnitSystem unitSystem)
    {
        return IsConverted(field, unitSystem) ? PmrUnitConverter.ImperialUnit(field.Quantity) : field.Unit;
    }

    private static string WithUnit(string number, string? unit)
    {
        return unit switch
        {
            null or "" => number,
            DegreesUnit => number + "°",
            PercentUnit => number + PercentUnit,
            _ => $"{number} {unit}"
        };
    }
}
