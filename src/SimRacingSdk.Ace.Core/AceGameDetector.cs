using System.Diagnostics;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Core.Services;

namespace SimRacingSdk.Ace.Core;

public class AceGameDetector : GameDetector, IAceGameDetector
{
    public const string ProcessName = "AssettoCorsaEVO";

    private static AceGameDetector? singletonInstance;

    public static AceGameDetector Instance => singletonInstance ??= new AceGameDetector();

    protected override bool IsGameRunning()
    {
        return Process.GetProcessesByName(ProcessName)
                      .Any();
    }
}
