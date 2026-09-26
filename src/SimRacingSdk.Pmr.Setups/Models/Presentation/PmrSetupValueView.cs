namespace SimRacingSdk.Pmr.Setups.Models.Presentation;

public record PmrSetupValueView
{
    // How far the setting sits above its minimum, from the saved value - exact even where
    // DisplayText can't be (see PmrSetupValueStatus.ShownInGameOnly). Null for a fixed setting.
    public int? Clicks { get; init; }

    // Exactly as the game shows it in the chosen unit system, unit included (e.g. "21.0 PSI").
    // Null unless Status is Shown.
    public string? DisplayText { get; init; }

    // A player-facing reason to show in place of DisplayText. Null when Status is Shown.
    public string? Explanation { get; init; }

    public required PmrSetupPosition Position { get; init; }
    public double? RawValue { get; init; }
    public required PmrSetupValueStatus Status { get; init; }
}
