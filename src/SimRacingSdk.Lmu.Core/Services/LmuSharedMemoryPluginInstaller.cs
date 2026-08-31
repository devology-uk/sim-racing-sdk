using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using SimRacingSdk.Lmu.Core.Abstractions;

namespace SimRacingSdk.Lmu.Core.Services;

// Bundled plugin is GPLv3 (github.com/tembob64/LMU_SharedMemoryMapPlugin, release v4.0.16.7) - an existing file or config section is left untouched rather than overwritten, since the player may already have their own copy/settings.
public class LmuSharedMemoryPluginInstaller : ILmuSharedMemoryPluginInstaller
{
    public const string PluginFileName = "LMU_SharedMemoryMapPlugin64.dll";

    private const string LicenseFileName = "LMU_SharedMemoryMapPlugin64-LICENSE.txt";
    private const string PluginDllResourceName =
        "SimRacingSdk.Lmu.Core.PluginResources.LMU_SharedMemoryMapPlugin64.dll";
    private const string PluginLicenseResourceName =
        "SimRacingSdk.Lmu.Core.PluginResources.LMU_SharedMemoryMapPlugin64-LICENSE.txt";

    private static LmuSharedMemoryPluginInstaller? singletonInstance;

    private readonly ILmuPathProvider lmuPathProvider;
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    public LmuSharedMemoryPluginInstaller(ILmuPathProvider lmuPathProvider)
    {
        this.lmuPathProvider = lmuPathProvider;
    }

    public static LmuSharedMemoryPluginInstaller Instance =>
        singletonInstance ??= new LmuSharedMemoryPluginInstaller(LmuPathProvider.Instance);

    public bool IsPluginConfigured
    {
        get
        {
            var section = this.ReadPluginConfigSection();
            return section is not null &&
                   this.IsSettingEnabled(section, "Enabled") &&
                   this.IsSettingEnabled(section, "EnableDirectMemoryAccess");
        }
    }

    public bool IsPluginFileInstalled =>
        File.Exists(Path.Combine(this.lmuPathProvider.PluginsFolder, PluginFileName));

    public bool IsInstalled => this.IsPluginFileInstalled && this.IsPluginConfigured;

    public void Install()
    {
        this.InstallPluginFile();
        this.ConfigurePlugin();
    }

    private void ConfigurePlugin()
    {
        var root = this.ReadCustomPluginVariables() ?? new JsonObject();

        if(root.ContainsKey(PluginFileName))
        {
            return;
        }

        root[PluginFileName] = new JsonObject
        {
            ["Enabled"] = 1,
            ["DebugISIInternals"] = 0,
            ["DebugOutputLevel"] = 0,
            ["DebugOutputSource"] = 0,
            ["DedicatedServerMapGlobally"] = 0,
            ["EnableDirectMemoryAccess"] = 1,
            ["EnableHWControlInput"] = 0,
            ["EnableRulesControlInput"] = 0,
            ["EnableWeatherControlInput"] = 0,
            ["UnsubscribedBuffersMask"] = 160
        };

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
        this.ExtractEmbeddedResource(PluginDllResourceName,
            Path.Combine(this.lmuPathProvider.PluginsFolder, PluginFileName));
        this.ExtractEmbeddedResource(PluginLicenseResourceName,
            Path.Combine(this.lmuPathProvider.PluginsFolder, LicenseFileName));
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
        return this.ReadCustomPluginVariables()?[PluginFileName]?.AsObject();
    }

    private void WriteCustomPluginVariables(JsonObject root)
    {
        var filePath = this.lmuPathProvider.CustomPluginVariablesFilePath;
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(filePath, root.ToJsonString(this.jsonSerializerOptions), Encoding.UTF8);
    }
}
