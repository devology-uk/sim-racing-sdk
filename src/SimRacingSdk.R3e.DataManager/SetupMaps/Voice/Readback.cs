using System.Globalization;
using SimRacingSdk.R3e.Setups.Models;

namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

// What the Data Manager says back, worded for listening rather than reading.
public static class Readback
{
    private const double WholeClickTolerance = 0.01;

    public static string Car(SetupMapCarRow car)
    {
        var status = car.IsValidated ? "Validated" : car.HasMap ? "Mapped" : "Not mapped";
        return $"{car.Car.Name}, {car.Car.Class}. {status}.";
    }

    public static string Number(double value)
    {
        return value.ToString("0.###", CultureInfo.InvariantCulture);
    }

    public static string Range(SetupRange range)
    {
        if(range.Min > range.Max)
        {
            return $"{Number(range.Min)} to {Number(range.Max)}. Warning, the min is above the max";
        }

        var clicks = (range.Max - range.Min) / range.Step;
        var clickText = Math.Abs(clicks - Math.Round(clicks)) < WholeClickTolerance
            ? $"{Math.Round(clicks)} clicks"
            : $"warning, {Number(clicks)} clicks, not a whole number";
        return $"{Number(range.Min)} to {Number(range.Max)}, step {Number(range.Step)}, {clickText}";
    }

    public static string Target(VoiceTarget target, SetupMapEditorViewModel editor)
    {
        var rows = target.Controls.Select(editor.RowFor).ToList();
        var states = rows.Select(State).ToList();
        if(states.Distinct().Count() == 1)
        {
            return $"{target.Description}, {states[0]}.";
        }

        return string.Join(". ", rows.Select((row, index) => $"{RowName(row)}, {states[index]}")) + ".";
    }

    private static string Availability(R3eSetupControlAvailability availability)
    {
        return availability switch
        {
            R3eSetupControlAvailability.Adjustable => "adjustable",
            R3eSetupControlAvailability.Fixed => "fixed",
            _ => "not fitted"
        };
    }

    private static string GearState(SetupControlRowViewModel row)
    {
        var gearCount = row.GearCount is null ? "no gear count" : $"{row.GearCount} gears";
        return $"{Availability(row.Availability)}, {gearCount}, {row.Options.Count} tooth pairs";
    }

    private static string OptionState(SetupControlRowViewModel row)
    {
        var options = row.Options.Count == 0 ? "no options" : string.Join(", ", row.Options);
        return $"{Availability(row.Availability)}, {options}";
    }

    private static string RangeState(SetupControlRowViewModel row)
    {
        if(row.Availability != R3eSetupControlAvailability.Adjustable)
        {
            return Availability(row.Availability);
        }

        return row.Range is null ? "adjustable, no range yet" : $"adjustable, {Range(row.Range)}";
    }

    private static string RowName(SetupControlRowViewModel row)
    {
        return row.Definition.Tab is R3eSetupTab.Front or R3eSetupTab.Rear ? $"{row.Tab} {row.Name}" : row.Name;
    }

    private static string State(SetupControlRowViewModel row)
    {
        if(row.Availability == R3eSetupControlAvailability.NotFitted)
        {
            return Availability(row.Availability);
        }

        return row.Definition.Kind switch
        {
            R3eSetupValueKind.GearSet => GearState(row),
            R3eSetupValueKind.Choice => OptionState(row),
            _ => RangeState(row)
        };
    }
}
