using Microsoft.Win32;
using SimRacingSdk.Abstractions;

namespace SimRacingSdk.Core.Services;

public class SteamInfoProvider : ISteamInfoProvider
{
    private const string AppManifestFilePattern = "appmanifest_*.acf";
    private const string InstallDirectoryKey = "installdir";
    private const string SteamRegistrySubKey = @"Software\Valve\Steam";
    private const string DefaultSteamPath = @"C:\Program Files (x86)\Steam";
    private const string SteamAppsFolderName = "steamapps";
    private const string SteamPathValueName = "SteamPath";
    private const string SteamLibraryDataFileName = "libraryfolders.vdf";
    private const string SteamInstallGamesRootFolderName = "common";

    private static SteamInfoProvider? singletonInstance;

    // Steam folder names don't always match a game's usual capitalisation (ACE installs to "Assetto Corsa EVO").
    private readonly Dictionary<string, string> installedGamePaths = new(StringComparer.OrdinalIgnoreCase);
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

    public string GetSteamPath()
    {
        if(string.IsNullOrWhiteSpace(this.steamPath))
        {
            this.Init();
        }

        return this.steamPath;
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

                this.LoadInstalledGamePathsFromLibrary(libraryPath);
            }
        }
        catch(Exception)
        {
        }
    }

    // Only folders named by an app manifest count: uninstalling a game, or moving it to another
    // library, can leave its folder behind, and a leftover in an earlier library would win.
    private void LoadInstalledGamePathsFromLibrary(string libraryPath)
    {
        var steamAppsFolderPath = Path.Combine(libraryPath, SteamAppsFolderName);
        foreach(var appManifestFilePath in Directory.EnumerateFiles(steamAppsFolderPath, AppManifestFilePattern))
        {
            var installFolderName = this.ReadInstallFolderName(appManifestFilePath);
            if(string.IsNullOrWhiteSpace(installFolderName))
            {
                continue;
            }

            var gamePath = Path.Combine(steamAppsFolderPath, SteamInstallGamesRootFolderName, installFolderName);
            if(Directory.Exists(gamePath))
            {
                this.installedGamePaths.TryAdd(installFolderName, gamePath);
            }
        }
    }

    private string? ReadInstallFolderName(string appManifestFilePath)
    {
        return File.ReadLines(appManifestFilePath)
                   .Select(line => line.Split('"'))
                   .Where(parts => parts.Length > 3 && parts[1]
                                       .Equals(InstallDirectoryKey, StringComparison.OrdinalIgnoreCase))
                   .Select(parts => parts[3])
                   .FirstOrDefault();
    }
}