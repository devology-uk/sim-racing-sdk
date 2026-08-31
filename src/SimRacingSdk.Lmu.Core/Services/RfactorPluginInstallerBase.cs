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
    private readonly IReadOnlyCollection<string> requiredEnabledSettings;

    protected RfactorPluginInstallerBase(ILmuPathProvider lmuPathProvider,
        string pluginFileName,
        string dllResourceName,
        string licenseFileName,
        string licenseResourceName,
        string bundledVersion,
        JsonObject defaultConfig,
        IReadOnlyCollection<string> requiredEnabledSettings)
    {
        this.lmuPathProvider = lmuPathProvider;
        this.PluginFileName = pluginFileName;
        this.dllResourceName = dllResourceName;
        this.licenseFileName = licenseFileName;
        this.licenseResourceName = licenseResourceName;
        this.BundledVersion = bundledVersion;
        this.defaultConfig = defaultConfig;
        this.requiredEnabledSettings = requiredEnabledSettings;
    }

    public string BundledVersion { get; }

    public bool IsInstalled => this.IsPluginFileInstalled && this.IsPluginConfigured;

    public bool IsPluginConfigured
    {
        get
        {
            var section = this.ReadPluginConfigSection();
            return section is not null &&
                   this.requiredEnabledSettings.All(setting => this.IsSettingEnabled(section, setting));
        }
    }

    public bool IsPluginFileInstalled =>
        File.Exists(Path.Combine(this.lmuPathProvider.PluginsFolder, this.PluginFileName));

    // The file actually sitting in the Plugins folder may not be the bundled one - SimHub/CrewChief/etc also ship
    // and self-update this same plugin family, and Install() never overwrites a file that's already there.
    public string? InstalledVersion
    {
        get
        {
            if(!this.IsPluginFileInstalled)
            {
                return null;
            }

            var pluginPath = Path.Combine(this.lmuPathProvider.PluginsFolder, this.PluginFileName);
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
        var root = this.ReadCustomPluginVariables() ?? new JsonObject();

        if(root.ContainsKey(this.PluginFileName))
        {
            return;
        }

        root[this.PluginFileName] = this.defaultConfig.DeepClone();

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

    private JsonObject? ReadPluginConfigSection()
    {
        return this.ReadCustomPluginVariables()?[this.PluginFileName]?.AsObject();
    }

    private void WriteCustomPluginVariables(JsonObject root)
    {
        var filePath = this.lmuPathProvider.CustomPluginVariablesFilePath;
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(filePath, root.ToJsonString(this.jsonSerializerOptions), Encoding.UTF8);
    }
}
