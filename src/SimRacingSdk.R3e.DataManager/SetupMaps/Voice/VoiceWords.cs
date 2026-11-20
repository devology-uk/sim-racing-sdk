namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

// The words that shape a command, shared by the grammar and the parser so the two can't drift apart.
public static class VoiceWords
{
    public const string Add = "add";
    public const string Adjustable = "adjustable";
    public const string Car = "car";
    public const string CarKey = "car";
    public const string Clear = "clear";
    public const string Fixed = "fixed";
    public const string NotFitted = "not fitted";
    public const string Options = "options";
    public const string Step = "step";
    public const string To = "to";

    public static readonly IReadOnlyList<string> Compounds = ["soft", "medium", "hard", "wet"];
}
