using Microsoft.Win32;
using SimRacingSdk.Abstractions;

namespace SimRacingSdk.Core.Services;

public class SteamInfoProvider : ISteamInfoProvider
{
    private const string SteamRegistrySubKey = @"Software\Valve\Steam";
    private const string DefaultSteamPath = @"C:\Program Files (x86)\Steam";
    private const string SteamAppsFolderName = "steamapps";
    private const string SteamPathValueName = "SteamPath";
    private const string SteamLibraryDataFileName = "libraryfolders.vdf";
    private const string SteamInstallGamesRootFolderName = "common";

    private static SteamInfoProvider? singletonInstance;

    private readonly Dictionary<string, string> installedGamePaths = new();
    private string steamPath = null!;

    public static SteamInfoProvider Instance
    {
        get
        {
            // ReSharper disable once InvertIf
            if(singletonInstance == null)
            {
                singletonInstance = new SteamInfoProvider();
                singletonInstance.Init();
            }

            return singletonInstance;
        }
    }

    public string GetGamePath(string gameName)
    {
        if(string.IsNullOrWhiteSpace(this.steamPath))
        {
            this.Init();
        }

        return this.installedGamePaths.TryGetValue(gameName, out var gamePath)? gamePath: string.Empty;
    }

    public void Init()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(SteamRegistrySubKey);
            if(key != null)
            {
                var registrySteamPath = key.GetValue(SteamPathValueName) as string;
                if(!string.IsNullOrEmpty(registrySteamPath) && Directory.Exists(registrySteamPath))
                {
                    this.steamPath = registrySteamPath;
                }
            }
        }
        catch(Exception)
        {
            // ignore error so that default path is used;
        }

        if(string.IsNullOrWhiteSpace(this.steamPath))
        {
            this.steamPath = DefaultSteamPath;
        }

        this.LoadInstalledGamePaths();
    }

    public bool IsGameInstalled(string gameName)
    {
        if(string.IsNullOrWhiteSpace(this.steamPath))
        {
            this.Init();
        }

        return this.installedGamePaths.ContainsKey(gameName);
    }

    private void LoadInstalledGamePaths()
    {
        var libraryDataFilePath =
            Path.Combine(this.steamPath, SteamAppsFolderName, SteamLibraryDataFileName);
        if(!File.Exists(libraryDataFilePath))
        {
            return;
        }

        try
        {
            var lines = File.ReadAllLines(libraryDataFilePath);
            foreach(var line in lines)
            {
                if(!line.Trim()
                        .Contains("path"))
                {
                    continue;
                }

                var parts = line.Split('"');
                if(parts.Length <= 3)
                {
                    continue;
                }

                var libraryPath = parts[3]
                    .Replace(@"\\", @"\");
                if(!Directory.Exists(libraryPath))
                {
                    continue;
                }

                var gameFolders = Directory.GetDirectories(Path.Combine(libraryPath,
                    SteamAppsFolderName,
                    SteamInstallGamesRootFolderName));
                foreach(var folder in gameFolders)
                {
                    var gameName = new DirectoryInfo(folder).Name;
                    this.installedGamePaths.TryAdd(gameName, folder);
                }
            }
        }
        catch(Exception)
        {
            // ignore errors while reading the file
        }
    }
}