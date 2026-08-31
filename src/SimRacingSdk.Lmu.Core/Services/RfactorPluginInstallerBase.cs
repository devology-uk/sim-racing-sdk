using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using SimRacingSdk.Lmu.Core.Abstractions;

namespace SimRacingSdk.Lmu.Core.Services;

// Shared install mechanics for rFactor2-API-family plugins (LMU's own plugin and the standard rFactor2 one) - both
// copy a DLL into the game's Plugins folder and add a section to CustomPluginVariables.JSON, differing only in
// filenames/resource names/default config values, which subclasses supply via the constructor.
public abstract class RfactorPluginInstallerBase : IRfactorPluginInstaller
{
    private readonly string dllResourceName;
    private readonly JsonObject defaultConfig;
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = true
    };
    private readonly string licenseFileName;
    private readonly string licenseResourceName;
    private readonly ILmuPathProvider lmuPathProvider;
    private readonly string productName;
    private readonly IReadOnlyCollection<string> requiredEnabledSettings;

    protected RfactorPluginInstallerBase(ILmuPathProvider lmuPathProvider,
        string pluginFileName,
        string productName,
        string dllResourceName,
        string licenseFileName,
        string licenseResourceName,
        string bundledVersion,
        JsonObject defaultConfig,
        IReadOnlyCollection<string> requiredEnabledSettings)
    {
        this.lmuPathProvider = lmuPathProvider;
        this.PluginFileName = pluginFileName;
        this.productName = productName;
        this.dllResourceName = dllResourceName;
        this.licenseFileName = licenseFileName;
        this.licenseResourceName = licenseResourceName;
        this.BundledVersion = bundledVersion;
        this.defaultConfig = defaultConfig;
        this.requiredEnabledSettings = requiredEnabledSettings;
    }

    public string BundledVersion { get; }

    public bool IsInstalled => this.IsPluginFileInstalled && this.IsPluginConfigured;

    // "Enabled" is deliberately not something we check or write - confirmed (Mike, 2026-08-31) to be a game-level
    // flag surfaced in LMU's own settings screens, defaulting to on for any plugin present in the Plugins folder,
    // not a plugin-declared custom variable. A plugin with no required settings of its own (e.g. the standard
    // rFactor2 plugin, which works without DMA) is therefore considered configured by file presence alone -
    // nothing to verify, and nothing to fight other tools (SimHub, CrewChief) over in CustomPluginVariables.JSON.
    public bool IsPluginConfigured
    {
        get
        {
            if(this.requiredEnabledSettings.Count == 0)
            {
                return true;
            }

            var installedFileName = this.FindInstalledFileName();
            if(installedFileName is null)
            {
                return false;
            }

            var section = this.ReadPluginConfigSection(installedFileName);
            return section is not null &&
                   this.requiredEnabledSettings.All(setting => this.IsSettingEnabled(section, setting));
        }
    }

    public bool IsPluginFileInstalled => this.FindInstalledFileName() is not null;

    // The file actually sitting in the Plugins folder may not be the bundled one, or even be named what we expect
    // - SimHub/CrewChief/etc also ship and self-update this same plugin family, sometimes under a different
    // filename (e.g. a version-suffixed name), and Install() never overwrites a file that's already there.
    public string? InstalledVersion
    {
        get
        {
            var installedFileName = this.FindInstalledFileName();
            if(installedFileName is null)
            {
                return null;
            }

            var pluginPath = Path.Combine(this.lmuPathProvider.PluginsFolder, installedFileName);
            return FileVersionInfo.GetVersionInfo(pluginPath).FileVersion;
        }
    }

    protected string PluginFileName { get; }

    public void Install()
    {
        this.InstallPluginFile();
        this.ConfigurePlugin();
    }

    private void ConfigurePlugin()
    {
        if(this.requiredEnabledSettings.Count == 0)
        {
            return;
        }

        var configFileName = this.FindInstalledFileName() ?? this.PluginFileName;
        var root = this.ReadCustomPluginVariables() ?? new JsonObject();

        if(root.ContainsKey(configFileName))
        {
            return;
        }

        root[configFileName] = this.defaultConfig.DeepClone();

        this.WriteCustomPluginVariables(root);
    }

    private void ExtractEmbeddedResource(string resourceName, string destinationPath)
    {
        using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName) ??
                                    throw new InvalidOperationException(
                                        $"Embedded resource '{resourceName}' was not found.");
        using var fileStream = File.Create(destinationPath);
        resourceStream.CopyTo(fileStream);
    }

    // Matches by the DLL's own embedded ProductName rather than trusting the on-disk filename to be our expected
    // constant - a copy installed by another tool can be renamed (confirmed on Mike's own rig: the LMU plugin was
    // present as "LMU.3.8.SharedMemoryMapPlugin.dll", not "LMU_SharedMemoryMapPlugin64.dll").
    private string? FindInstalledFileName()
    {
        if(!Directory.Exists(this.lmuPathProvider.PluginsFolder))
        {
            return null;
        }

        var canonicalPath = Path.Combine(this.lmuPathProvider.PluginsFolder, this.PluginFileName);
        if(File.Exists(canonicalPath))
        {
            return this.PluginFileName;
        }

        foreach(var dllPath in Directory.EnumerateFiles(this.lmuPathProvider.PluginsFolder, "*.dll"))
        {
            FileVersionInfo versionInfo;
            try
            {
                versionInfo = FileVersionInfo.GetVersionInfo(dllPath);
            }
            catch(Exception)
            {
                continue;
            }

            if(versionInfo.ProductName == this.productName)
            {
                return Path.GetFileName(dllPath);
            }
        }

        return null;
    }

    private void InstallPluginFile()
    {
        if(this.IsPluginFileInstalled)
        {
            return;
        }

        Directory.CreateDirectory(this.lmuPathProvider.PluginsFolder);
        this.ExtractEmbeddedResource(this.dllResourceName,
            Path.Combine(this.lmuPathProvider.PluginsFolder, this.PluginFileName));
        this.ExtractEmbeddedResource(this.licenseResourceName,
            Path.Combine(this.lmuPathProvider.PluginsFolder, this.licenseFileName));
    }

    private bool IsSettingEnabled(JsonObject section, string propertyName)
    {
        return section[propertyName]?.GetValue<int>() == 1;
    }

    private JsonObject? ReadCustomPluginVariables()
    {
        var filePath = this.lmuPathProvider.CustomPluginVariablesFilePath;
        if(!File.Exists(filePath))
        {
            return null;
        }

        var json = File.ReadAllText(filePath, Encoding.UTF8);
        return JsonNode.Parse(json)?.AsObject();
    }

    private JsonObject? ReadPluginConfigSection(string configFileName)
    {
        return this.ReadCustomPluginVariables()?[configFileName]?.AsObject();
    }

    private void WriteCustomPluginVariables(JsonObject root)
    {
        var filePath = this.lmuPathProvider.CustomPluginVariablesFilePath;
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(filePath, root.ToJsonString(this.jsonSerializerOptions), Encoding.UTF8);
    }
}
