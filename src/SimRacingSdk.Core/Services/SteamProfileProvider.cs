using SimRacingSdk.Abstractions;

namespace SimRacingSdk.Core.Services;

public class SteamProfileProvider : ISteamProfileProvider
{
    private const string ConfigFolderName = "config";
    private const string LoginUsersFileName = "loginusers.vdf";
    private const string PersonaNameKey = "PersonaName";
    private const string MostRecentKey = "MostRecent";
    private const string MostRecentValue = "1";

    private static SteamProfileProvider? singletonInstance;

    private readonly ISteamInfoProvider steamInfoProvider;

    public SteamProfileProvider(ISteamInfoProvider steamInfoProvider)
    {
        this.steamInfoProvider = steamInfoProvider;
    }

    public static SteamProfileProvider Instance =>
        singletonInstance ??= new SteamProfileProvider(SteamInfoProvider.Instance);

    public string GetPersonaName()
    {
        try
        {
            var loginUsersFilePath = Path.Combine(this.steamInfoProvider.GetSteamPath(),
                ConfigFolderName,
                LoginUsersFileName);
            return File.Exists(loginUsersFilePath)
                ? FindMostRecentPersonaName(File.ReadAllLines(loginUsersFilePath))
                : string.Empty;
        }
        catch(Exception)
        {
            return string.Empty;
        }
    }

    private static string FindMostRecentPersonaName(IEnumerable<string> lines)
    {
        var personaName = string.Empty;
        var isMostRecent = false;
        foreach(var line in lines)
        {
            var trimmedLine = line.Trim();
            if(trimmedLine == "}")
            {
                if(isMostRecent && personaName.Length > 0)
                {
                    return personaName;
                }

                personaName = string.Empty;
                isMostRecent = false;
                continue;
            }

            var parts = trimmedLine.Split('"');
            if(parts.Length < 4)
            {
                continue;
            }

            switch(parts[1])
            {
                case PersonaNameKey:
                    personaName = parts[3];
                    break;
                case MostRecentKey:
                    isMostRecent = parts[3] == MostRecentValue;
                    break;
            }
        }

        return string.Empty;
    }
}
