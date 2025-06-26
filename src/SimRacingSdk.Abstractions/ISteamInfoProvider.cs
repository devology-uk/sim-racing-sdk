namespace SimRacingSdk.Abstractions;

public interface ISteamInfoProvider
{
    string GetGamePath(string gameName);
    void Init();
    bool IsGameInstalled(string gameName);
}