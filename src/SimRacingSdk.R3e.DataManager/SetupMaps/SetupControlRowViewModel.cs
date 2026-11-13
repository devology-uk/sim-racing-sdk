using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.R3e.Setups.Models;

namespace SimRacingSdk.R3e.DataManager.SetupMaps;

// Min, Step and Max are held as text so the cells can update on every keystroke: a double binding would
// reformat "0." or "-" as they're typed.
public partial class SetupControlRowViewModel : ObservableObject
{
    private static readonly char[] OptionSeparators = [','];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanHaveRange), nameof(CanHaveOptions))]
    private R3eSetupControlAvailability availability;

    [ObservableProperty]
    private string maxText = string.Empty;

    [ObservableProperty]
    private string minText = string.Empty;

    [ObservableProperty]
    private string optionsText = string.Empty;

    [ObservableProperty]
    private string stepText = string.Empty;

    public SetupControlRowViewModel(R3eSetupControlDefinition definition)
    {
        this.Definition = definition;
    }

    // A fixed compound still has its one option recorded, so the map says which compound the car runs.
    public bool CanHaveOptions => this.Definition.Kind == R3eSetupValueKind.Choice
                                  && this.Availability != R3eSetupControlAvailability.NotFitted;

    public bool CanHaveRange => this.Availability == R3eSetupControlAvailability.Adjustable
                                && this.Definition.Kind is R3eSetupValueKind.Number or R3eSetupValueKind.LevelTable;

    public R3eSetupControlDefinition Definition { get; }
    public string Name => this.Definition.Name;
    public string Section => this.Definition.Section;
    public string Tab => this.Definition.Tab.ToString();
    public string Unit => this.Definition.MetricUnit;

    public void LoadFrom(R3eSetupControlSetting setting)
    {
        this.Availability = setting.Availability;
        this.MinText = FormatNumber(setting.Min);
        this.StepText = FormatNumber(setting.Step);
        this.MaxText = FormatNumber(setting.Max);
        this.OptionsText = string.Join(", ", setting.Options ?? []);
    }

    public string? Problem()
    {
        if(!this.CanHaveRange)
        {
            return null;
        }

        var notANumber = this.NotANumberProblem("min", this.MinText)
                         ?? this.NotANumberProblem("step", this.StepText)
                         ?? this.NotANumberProblem("max", this.MaxText);
        if(notANumber is not null)
        {
            return notANumber;
        }

        if(ParseNumber(this.StepText) is <= 0)
        {
            return $"{this.Tab} {this.Name}'s step must be more than 0";
        }

        return ParseNumber(this.MinText) > ParseNumber(this.MaxText) ? $"{this.Tab} {this.Name}'s min is above its max" : null;
    }

    public R3eSetupControlSetting ToSetting()
    {
        return new R3eSetupControlSetting
        {
            Control = this.Definition.Control,
            Availability = this.Availability,
            Min = this.CanHaveRange ? ParseNumber(this.MinText) : null,
            Step = this.CanHaveRange ? ParseNumber(this.StepText) : null,
            Max = this.CanHaveRange ? ParseNumber(this.MaxText) : null,
            Options = this.CanHaveOptions ? this.ParseOptions() : null
        };
    }

    private static string FormatNumber(double? value)
    {
        return value?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
    }

    private static bool IsBlankOrNumber(string text)
    {
        return string.IsNullOrWhiteSpace(text) || ParseNumber(text) is not null;
    }

    private static double? ParseNumber(string text)
    {
        return double.TryParse(text.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var value) ? value : null;
    }

    private string? NotANumberProblem(string fieldName, string text)
    {
        return IsBlankOrNumber(text) ? null : $"{this.Tab} {this.Name}'s {fieldName} '{text}' isn't a number";
    }

    private List<string>? ParseOptions()
    {
        var options = this.OptionsText
            .Split(OptionSeparators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
        return options.Count > 0 ? options : null;
    }
}
