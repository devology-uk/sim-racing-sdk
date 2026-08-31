using System.Text.Json.Nodes;
using SimRacingSdk.Lmu.Core.Abstractions;

namespace SimRacingSdk.Lmu.Core.Services;

// Bundled plugin is GPLv3 (github.com/tembob64/LMU_SharedMemoryMapPlugin, release v4.0.16.7) - an existing file or config section is left untouched rather than overwritten, since the player may already have their own copy/settings.
// " Enabled" (leading space, confirmed via the plugin's own GetCustomVariable/AccessCustomVariable source, both
// LMU_SharedMemoryMap.cpp and rFactor2SharedMemoryMap.cpp - identical, LMU's is copy-pasted from the template) is
// this plugin's own custom variable claiming the game's auto-created enable slot, deliberately defaulting to 0 -
// not a game-managed flag, and not a typo carried over from tooling. Genuinely required, unlike EnableHWControlInput
// etc. below, whose plugin-declared defaults we deliberately override for safety rather than by necessity.
public class LmuSharedMemoryPluginInstaller : RfactorPluginInstallerBase, ILmuSharedMemoryPluginInstaller
{
    public new const string PluginFileName = "LMU_SharedMemoryMapPlugin64.dll";

    private static LmuSharedMemoryPluginInstaller? singletonInstance;

    public LmuSharedMemoryPluginInstaller(ILmuPathProvider lmuPathProvider)
        : base(lmuPathProvider,
            PluginFileName,
            "LMU_SharedMemoryMapPlugin64",
            "SimRacingSdk.Lmu.Core.PluginResources.LMU_SharedMemoryMapPlugin64.dll",
            "LMU_SharedMemoryMapPlugin64-LICENSE.txt",
            "SimRacingSdk.Lmu.Core.PluginResources.LMU_SharedMemoryMapPlugin64-LICENSE.txt",
            "4.0.16.7",
            new JsonObject
            {
                [" Enabled"] = 1,
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
            [" Enabled", "EnableDirectMemoryAccess"])
    {
    }

    public static LmuSharedMemoryPluginInstaller Instance =>
        singletonInstance ??= new LmuSharedMemoryPluginInstaller(LmuPathProvider.Instance);
}
