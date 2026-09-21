namespace SimRacingSdk.Abstractions;

public interface ISteamInfoProvider
{
    string GetGamePath(string gameName);
    string GetSteamPath();
    void Init();
    bool IsGameInstalled(string gameName);
}