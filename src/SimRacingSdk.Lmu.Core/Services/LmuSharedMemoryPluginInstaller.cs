using System.Text.Json.Nodes;
using SimRacingSdk.Lmu.Core.Abstractions;

namespace SimRacingSdk.Lmu.Core.Services;

// Bundled plugin is GPLv3 (github.com/tembob64/LMU_SharedMemoryMapPlugin, release v4.0.16.7) - an existing file or config section is left untouched rather than overwritten, since the player may already have their own copy/settings.
public class LmuSharedMemoryPluginInstaller : RfactorPluginInstallerBase, ILmuSharedMemoryPluginInstaller
{
    public const string PluginFileName = "LMU_SharedMemoryMapPlugin64.dll";

    private static LmuSharedMemoryPluginInstaller? singletonInstance;

    public LmuSharedMemoryPluginInstaller(ILmuPathProvider lmuPathProvider)
        : base(lmuPathProvider,
            PluginFileName,
            "SimRacingSdk.Lmu.Core.PluginResources.LMU_SharedMemoryMapPlugin64.dll",
            "LMU_SharedMemoryMapPlugin64-LICENSE.txt",
            "SimRacingSdk.Lmu.Core.PluginResources.LMU_SharedMemoryMapPlugin64-LICENSE.txt",
            new JsonObject
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
            },
            ["Enabled", "EnableDirectMemoryAccess"])
    {
    }

    public static LmuSharedMemoryPluginInstaller Instance =>
        singletonInstance ??= new LmuSharedMemoryPluginInstaller(LmuPathProvider.Instance);
}
