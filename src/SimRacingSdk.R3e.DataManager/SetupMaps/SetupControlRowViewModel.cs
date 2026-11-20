using System.Globalization;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using SimRacingSdk.R3e.Setups.Models;

namespace SimRacingSdk.R3e.DataManager.SetupMaps;

// Min, Step and Max are held as text so the cells can update on every keystroke: a double binding would
// reformat "0." or "-" as they're typed.
public partial class SetupControlRowViewModel : ObservableObject
{
    private static readonly Regex GearPairPattern = new(@"^\d+:\d+$");
    private static readonly char[] OptionSeparators = [','];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanHaveRange), nameof(CanHaveOptions), nameof(CanHaveGearCount))]
    private R3eSetupControlAvailability availability;

    [ObservableProperty]
    private string gearCountText = string.Empty;

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

    public bool CanHaveGearCount => this.Definition.Kind == R3eSetupValueKind.GearSet
                                    && this.Availability != R3eSetupControlAvailability.NotFitted;

    // A fixed compound still has its one option recorded, so the map says which compound the car runs.
    public bool CanHaveOptions => this.Definition.Kind is R3eSetupValueKind.Choice or R3eSetupValueKind.GearSet
                                  && this.Availability != R3eSetupControlAvailability.NotFitted;

    public bool CanHaveRange => this.Availability == R3eSetupControlAvailability.Adjustable && this.TakesRange;

    public R3eSetupControlDefinition Definition { get; }
    public int? GearCount => ParseWholeNumber(this.GearCountText);
    public string Name => this.Definition.Name;
    public IReadOnlyList<string> Options => this.ParseOptions() ?? [];
    public SetupRange? Range => ParseNumber(this.MinText) is { } min
                                && ParseNumber(this.StepText) is { } step
                                && ParseNumber(this.MaxText) is { } max
        ? new SetupRange(min, step, max)
        : null;

    public string Section => this.Definition.Section;
    public string Tab => this.Definition.Tab.ToString();
    public bool TakesGearPairs => this.Definition.Kind == R3eSetupValueKind.GearSet;
    public bool TakesOptions => this.Definition.Kind == R3eSetupValueKind.Choice;
    public bool TakesRange => this.Definition.Kind is R3eSetupValueKind.Number or R3eSetupValueKind.LevelTable;
    public string Unit => this.Definition.MetricUnit;

    public void LoadFrom(R3eSetupControlSetting setting)
    {
        this.Availability = setting.Availability;
        this.GearCountText = setting.GearCount?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        this.MinText = FormatNumber(setting.Min);
        this.StepText = FormatNumber(setting.Step);
        this.MaxText = FormatNumber(setting.Max);
        this.OptionsText = string.Join(", ", setting.Options ?? []);
    }

    public void FillCommonRangeIfBlank()
    {
        var commonRange = CommonRanges.For(this.Definition.Control);
        if(commonRange is null || !this.CanHaveRange || !this.IsRangeBlank())
        {
            return;
        }

        this.MinText = FormatNumber(commonRange.Min);
        this.StepText = FormatNumber(commonRange.Step);
        this.MaxText = FormatNumber(commonRange.Max);
    }

    public void SetOptions(IEnumerable<string> options)
    {
        this.OptionsText = string.Join(", ", options);
    }

    public void SetRange(SetupRange range)
    {
        this.Availability = R3eSetupControlAvailability.Adjustable;
        this.MinText = FormatNumber(range.Min);
        this.StepText = FormatNumber(range.Step);
        this.MaxText = FormatNumber(range.Max);
    }

    public string? Problem()
    {
        if(this.Definition.Kind == R3eSetupValueKind.GearSet)
        {
            return this.GearSetProblem();
        }

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
            GearCount = this.CanHaveGearCount ? ParseWholeNumber(this.GearCountText) : null,
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

    private static int? ParseWholeNumber(string text)
    {
        return int.TryParse(text.Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out var value) ? value : null;
    }

    private string? GearSetProblem()
    {
        if(!this.CanHaveGearCount)
        {
            return null;
        }

        var gearCount = ParseWholeNumber(this.GearCountText);
        if(!string.IsNullOrWhiteSpace(this.GearCountText) && gearCount is not > 0)
        {
            return $"{this.Name}'s gear count '{this.GearCountText}' isn't a whole number above 0";
        }

        var pairs = this.ParseOptions() ?? [];
        var badPair = pairs.FirstOrDefault(pair => !GearPairPattern.IsMatch(pair));
        if(badPair is not null)
        {
            return $"{this.Name}'s '{badPair}' isn't a tooth pair like 18:34";
        }

        var repeatedPair = pairs.GroupBy(pair => pair).FirstOrDefault(group => group.Count() > 1)?.Key;
        if(repeatedPair is not null)
        {
            return $"{this.Name} lists '{repeatedPair}' more than once";
        }

        return pairs.Count > 0 && pairs.Count < gearCount
            ? $"{this.Name} lists {pairs.Count} tooth pairs for {gearCount} gears"
            : null;
    }

    private bool IsRangeBlank()
    {
        return string.IsNullOrWhiteSpace(this.MinText)
               && string.IsNullOrWhiteSpace(this.StepText)
               && string.IsNullOrWhiteSpace(this.MaxText);
    }

    private string? NotANumberProblem(string fieldName, string text)
    {
        return IsBlankOrNumber(text) ? null : $"{this.Tab} {this.Name}'s {fieldName} '{text}' isn't a number";
    }

    // Values a row can no longer hold are cleared rather than left greyed out; saving drops them anyway.
    partial void OnAvailabilityChanged(R3eSetupControlAvailability value)
    {
        this.ClearValuesItCanNoLongerHave();
        this.FillCommonRangeIfBlank();
    }

    private void ClearValuesItCanNoLongerHave()
    {
        if(!this.CanHaveRange)
        {
            this.MinText = string.Empty;
            this.StepText = string.Empty;
            this.MaxText = string.Empty;
        }

        if(!this.CanHaveOptions)
        {
            this.OptionsText = string.Empty;
        }

        if(!this.CanHaveGearCount)
        {
            this.GearCountText = string.Empty;
        }
    }

    private List<string>? ParseOptions()
    {
        var options = this.OptionsText
            .Split(OptionSeparators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
        return options.Count > 0 ? options : null;
    }
}
