namespace SimRacingSdk.Lmu.Setups.Models;

// A single "Key=Index//Comment" line from a .svm file. Comment is whatever LMU itself displays for
// that click index (e.g. "-1.9 deg", "B39", "49.8:50.2") - format varies per field, not yet typed.
public readonly record struct LmuSetupValue(int Index, string Comment)
{
    // "Standard" is deliberately excluded - it's a genuine selectable value for some fields (e.g.
    // RatioSetSetting), not a not-applicable marker.
    private static readonly HashSet<string> InapplicableSentinels =
        new(StringComparer.OrdinalIgnoreCase) { "N/A", "Non-adjustable", "Fixed", "Detached" };

    public bool IsApplicable => !InapplicableSentinels.Contains(this.Comment);
}
