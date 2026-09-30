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

    // Why IsActive is false, in the player's terms. Null while active.
    public string? InactiveReason { get; init; }

    // False where the game greys the setting out because another setting disables it (e.g. Regen
    // Limit unless Regen Mode is Full). The saved value is still shown, as the game does.
    public bool IsActive { get; init; } = true;

    public required PmrSetupPosition Position { get; init; }
    public double? RawValue { get; init; }
    public required PmrSetupValueStatus Status { get; init; }
}
