using System.Diagnostics;
using SimRacingSdk.Core.Services;
using SimRacingSdk.Pmr.Core.Abstractions;

namespace SimRacingSdk.Pmr.Core;

public class PmrGameDetector : GameDetector, IPmrGameDetector
{
    private const string ProcessName = "ProjectMotorRacingGame";

    private static PmrGameDetector? singletonInstance;

    public static PmrGameDetector Instance => singletonInstance ??= new PmrGameDetector();

    protected override bool IsGameRunning()
    {
        return Process.GetProcessesByName(ProcessName)
                       .Any();
    }
}
