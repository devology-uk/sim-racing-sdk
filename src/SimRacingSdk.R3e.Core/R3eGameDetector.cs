using System.Diagnostics;
using SimRacingSdk.Core.Services;
using SimRacingSdk.R3e.Core.Abstractions;

namespace SimRacingSdk.R3e.Core;

public class R3eGameDetector : GameDetector, IR3eGameDetector
{
    private static readonly string[] ProcessNames = ["RRRE64", "RRRE"];

    private static R3eGameDetector? singletonInstance;

    public static R3eGameDetector Instance => singletonInstance ??= new R3eGameDetector();

    protected override bool IsGameRunning()
    {
        return ProcessNames.Any(processName => Process.GetProcessesByName(processName)
                                                      .Any());
    }
}
