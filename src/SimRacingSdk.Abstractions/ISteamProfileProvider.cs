namespace SimRacingSdk.Abstractions;

public interface ISteamProfileProvider
{
    // The display name of the Steam account most recently signed in on this machine, or an
    // empty string when it can't be determined. Read fresh on every call because the user can
    // rename their Steam profile at any time.
    string GetPersonaName();
}
