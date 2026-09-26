using CommunityToolkit.Mvvm.ComponentModel;

namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

// A mutable, bindable working copy of one SetupFieldInfo - mirrors CarEditorViewModel/
// TrackLayoutEditorViewModel (the record it wraps is immutable with required init-only
// properties, which TwoWay bindings can't target).
public partial class SetupFieldEditorViewModel : ObservableObject
{
    [ObservableProperty]
    private int? decimals;

    [ObservableProperty]
    private SetupFieldDisplayFormat displayFormat;

    [ObservableProperty]
    private SetupFieldDisplaySource displaySource;

    [ObservableProperty]
    private string enumValuesText = string.Empty;

    [ObservableProperty]
    private bool hasAutoOption;

    [ObservableProperty]
    private int? imperialDecimals;

    [ObservableProperty]
    private SetupFieldKind kind = SetupFieldKind.Numeric;

    [ObservableProperty]
    private double? max;

    [ObservableProperty]
    private double? min;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private SetupFieldQuantity quantity;

    [ObservableProperty]
    private string? rawKey;

    [ObservableProperty]
    private double? rawMax;

    [ObservableProperty]
    private double? rawMin;

    [ObservableProperty]
    private double? rawStep;

    [ObservableProperty]
    private SetupFieldScope scope = SetupFieldScope.Single;

    [ObservableProperty]
    private string section = string.Empty;

    [ObservableProperty]
    private double? step;

    [ObservableProperty]
    private string? unit;

    public bool IsEnum => this.Kind == SetupFieldKind.Enum;
    public bool IsNumeric => this.Kind == SetupFieldKind.Numeric;

    public static SetupFieldEditorViewModel From(SetupFieldInfo field)
    {
        return new SetupFieldEditorViewModel
        {
            Decimals = field.Decimals,
            DisplayFormat = field.DisplayFormat,
            DisplaySource = field.DisplaySource,
            EnumValuesText = string.Join(", ", field.EnumValues ?? []),
            HasAutoOption = field.HasAutoOption,
            ImperialDecimals = field.ImperialDecimals,
            Kind = field.Kind,
            Max = field.Max,
            Min = field.Min,
            Name = field.Name,
            Quantity = field.Quantity,
            RawKey = field.RawKey,
            RawMax = field.RawMax,
            RawMin = field.RawMin,
            RawStep = field.RawStep,
            Scope = field.Scope,
            Section = field.Section,
            Step = field.Step,
            Unit = field.Unit
        };
    }

    public SetupFieldInfo ToSetupFieldInfo()
    {
        return new SetupFieldInfo
        {
            Decimals = this.Decimals,
            DisplayFormat = this.DisplayFormat,
            DisplaySource = this.DisplaySource,
            EnumValues = this.Kind == SetupFieldKind.Enum
                ? this.EnumValuesText.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList()
                : null,
            HasAutoOption = this.Kind == SetupFieldKind.Enum && this.HasAutoOption,
            ImperialDecimals = this.Quantity == SetupFieldQuantity.None ? null : this.ImperialDecimals,
            Kind = this.Kind,
            Max = this.Kind == SetupFieldKind.Numeric ? this.Max : null,
            Min = this.Kind == SetupFieldKind.Numeric ? this.Min : null,
            Name = this.Name,
            Quantity = this.Quantity,
            RawKey = this.RawKey,
            RawMax = this.RawMax,
            RawMin = this.RawMin,
            RawStep = this.RawStep,
            Scope = this.Scope,
            Section = this.Section,
            Step = this.Kind == SetupFieldKind.Numeric ? this.Step : null,
            Unit = this.Unit
        };
    }

    partial void OnKindChanged(SetupFieldKind value)
    {
        this.OnPropertyChanged(nameof(this.IsNumeric));
        this.OnPropertyChanged(nameof(this.IsEnum));
    }
}
