using System.Text.Json.Nodes;
using SimRacingSdk.Lmu.Core.Abstractions;

namespace SimRacingSdk.Lmu.Core.Services;

// Bundled plugin is GPLv3 (github.com/TheIronWolfModding/rF2SharedMemoryMapPlugin, release v3.7.15.1) - standard
// rFactor2-API telemetry/scoring/graphics buffers, which LMU's own plugin deliberately doesn't duplicate (see
// LmuSharedMemoryPluginInstaller). EnableDirectMemoryAccess defaults off here (unlike the LMU plugin, where DMA is
// the only source of any of its fields) since this plugin's core buffers work without it.
public class Rfactor2SharedMemoryPluginInstaller : RfactorPluginInstallerBase, IRfactor2SharedMemoryPluginInstaller
{
    public new const string PluginFileName = "rFactor2SharedMemoryMapPlugin64.dll";

    private static Rfactor2SharedMemoryPluginInstaller? singletonInstance;

    public Rfactor2SharedMemoryPluginInstaller(ILmuPathProvider lmuPathProvider)
        : base(lmuPathProvider,
            PluginFileName,
            "rF2SharedMemoryMapPlugin",
            "SimRacingSdk.Lmu.Core.PluginResources.rFactor2SharedMemoryMapPlugin64.dll",
            "rFactor2SharedMemoryMapPlugin64-LICENSE.txt",
            "SimRacingSdk.Lmu.Core.PluginResources.rFactor2SharedMemoryMapPlugin64-LICENSE.txt",
            "3.7.15.1",
            new JsonObject
            {
                ["Enabled"] = 1,
                ["DebugISIInternals"] = 1,
                ["DebugOutputLevel"] = 0,
                ["DebugOutputSource"] = 0,
                ["DedicatedServerMapGlobally"] = 0,
                ["EnableDirectMemoryAccess"] = 0,
                ["EnableHWControlInput"] = 0,
                ["EnableRulesControlInput"] = 0,
                ["EnableWeatherControlInput"] = 0,
                ["UnsubscribedBuffersMask"] = 160
            },
            ["Enabled"])
    {
    }

    public static Rfactor2SharedMemoryPluginInstaller Instance =>
        singletonInstance ??= new Rfactor2SharedMemoryPluginInstaller(LmuPathProvider.Instance);
}
