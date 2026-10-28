namespace SimRacingSdk.R3e.Core.Abstractions;

public interface IR3eCompatibilityChecker
{
    bool HasDrivenAtLeastOneSession();
    bool HasLaunchedGame();
    bool HasSavedSetup();
    bool IsR3eInstalled();
}
