using SimRacingSdk.Pmr.Setups.Models.Files;
using SimRacingSdk.Pmr.Setups.Models.Maps;
using SimRacingSdk.Pmr.Setups.Models.Presentation;

namespace SimRacingSdk.Pmr.Setups.Services;

// Turns one mapped field at one position into what the game shows - or, where that can't be known
// exactly, says why instead of approximating it.
internal static class PmrSetupValueReader
{
    private const string AutoOption = "Auto";

    public static PmrSetupValueView Read(PmrSetupFieldMap field, PmrSetupPosition position, PmrSetupFile setup, PmrUnitSystem unitSystem)
    {
        var rawValue = field.RawKey is null ? null : setup.FindValue(PmrSetupPositions.ExpandRawKey(field.RawKey, position));
        if(field.RawKey is not null && rawValue is null)
        {
            return Unavailable(position, PmrSetupValueStatus.NotInSetupFile, PmrSetupExplanations.NotInSetupFile, null, null);
        }

        var clicks = rawValue is null ? null : ClicksFor(field, rawValue.Value);
        if(field.DisplaySource == PmrSetupFieldDisplaySource.GameSimulation)
        {
            return Unavailable(position, PmrSetupValueStatus.ShownInGameOnly, PmrSetupExplanations.ShownInGameOnly, rawValue, clicks);
        }

        var displayText = DisplayTextFor(field, rawValue, unitSystem);
        return displayText is null
                   ? Unavailable(position, PmrSetupValueStatus.NotMapped, PmrSetupExplanations.NotMapped, rawValue, clicks)
                   : Shown(position, displayText, rawValue, clicks);
    }

    private static int? ClicksFor(PmrSetupFieldMap field, double rawValue)
    {
        var (rawMin, rawStep) = RawStepping(field);
        return rawMin is null || rawStep is null or <= 0 ? null : (int)Math.Round((rawValue - rawMin.Value) / rawStep.Value);
    }

    // An Enum's entries are 0, 1, 2... unless its map says otherwise.
    private static (double? RawMin, double? RawStep) RawStepping(PmrSetupFieldMap field)
    {
        return field.Kind == PmrSetupFieldKind.Enum ? (field.RawMin ?? 0, field.RawStep ?? 1) : (field.RawMin, field.RawStep);
    }

    private static string? DisplayTextFor(PmrSetupFieldMap field, double? rawValue, PmrUnitSystem unitSystem)
    {
        if(field.Kind == PmrSetupFieldKind.Enum)
        {
            return rawValue is null ? null : EnumText(field, rawValue.Value);
        }

        var displayValue = NumericDisplayValue(field, rawValue, unitSystem);
        return displayValue is null ? null : PmrSetupValueFormatter.Format(displayValue.Value, field, unitSystem);
    }

    // A value below the first entry is the game's live-resolving "Auto" option.
    private static string? EnumText(PmrSetupFieldMap field, double rawValue)
    {
        var index = ClicksFor(field, rawValue) ?? -1;
        if(index < 0)
        {
            return field.HasAutoOption ? AutoOption : null;
        }

        return field.EnumValues is not null && index < field.EnumValues.Count
                   ? PmrSetupValueFormatter.FormatEnumValue(field.EnumValues[index], field)
                   : null;
    }

    private static double? NumericDisplayValue(PmrSetupFieldMap field, double? rawValue, PmrUnitSystem unitSystem)
    {
        if(rawValue is null)
        {
            return field.IsFixed ? FixedDisplayValue(field, unitSystem) : null;
        }

        if(field.Quantity != PmrSetupFieldQuantity.None)
        {
            return PmrUnitConverter.ToDisplay(rawValue.Value, field.Quantity, unitSystem);
        }

        return field.IsFixed ? field.Min : ScaledDisplayValue(field, rawValue.Value);
    }

    // A fixed value is captured as the Metric screen shows it, so it's converted back to raw first.
    private static double? FixedDisplayValue(PmrSetupFieldMap field, PmrUnitSystem unitSystem)
    {
        if(field.Quantity == PmrSetupFieldQuantity.None || unitSystem == PmrUnitSystem.Metric)
        {
            return field.Min;
        }

        return PmrUnitConverter.ToDisplay(PmrUnitConverter.ToRaw(field.Min!.Value, field.Quantity), field.Quantity, unitSystem);
    }

    // Maps the raw range onto the displayed range, e.g. raw 1e7..1e8 onto Steering Stiffness 1..10.
    private static double? ScaledDisplayValue(PmrSetupFieldMap field, double rawValue)
    {
        if(field.RawMin is not { } rawMin || field.RawMax is not { } rawMax || field.Min is not { } min || field.Max is not { } max
           || rawMax == rawMin)
        {
            return null;
        }

        return min + ((rawValue - rawMin) * (max - min) / (rawMax - rawMin));
    }

    private static PmrSetupValueView Shown(PmrSetupPosition position, string displayText, double? rawValue, int? clicks)
    {
        return new PmrSetupValueView
        {
            Clicks = clicks,
            DisplayText = displayText,
            Position = position,
            RawValue = rawValue,
            Status = PmrSetupValueStatus.Shown
        };
    }

    private static PmrSetupValueView Unavailable(PmrSetupPosition position, PmrSetupValueStatus status, string explanation, double? rawValue, int? clicks)
    {
        return new PmrSetupValueView
        {
            Clicks = clicks,
            Explanation = explanation,
            Position = position,
            RawValue = rawValue,
            Status = status
        };
    }
}
