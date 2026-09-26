namespace SimRacingSdk.Pmr.Setups.Models.Presentation;

// Player-facing wording, owned by the SDK so every consumer explains these cases the same way.
public static class PmrSetupExplanations
{
    public const string NoMap =
        "Setup details aren't available for this car yet. The setup file itself is unaffected.";

    public const string NotInSetupFile =
        "This setup file doesn't contain this setting. It may have been saved by an older version of the game.";

    public const string NotMapped =
        "This setting hasn't been mapped for this car yet, so it can't be shown here. Check it in-game.";

    public const string ShownInGameOnly =
        "Shown in-game only. Project Motor Racing calculates this from the car's settled suspension "
        + "(ride height, bump stops, fuel and driver weight) and doesn't save it with the setup, so it can't be "
        + "shown here. Changes between setups are still shown exactly, in clicks.";
}
