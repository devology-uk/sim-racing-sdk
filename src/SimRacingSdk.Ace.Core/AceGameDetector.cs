using System.Diagnostics;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Core.Services;

namespace SimRacingSdk.Ace.Core;

// ProcessName is an unverified guess (Unreal Engine's "<Project>-Win64-Shipping" convention,
// following Acc's own "AC2-Win64-Shipping") - Ace = Assetto Corsa Evo is confirmed, but nothing
// has confirmed the real shipping executable name yet. Mike is waiting on his own install to
// check this - see IAceCompatibilityChecker/AcePathProvider for the same category of guess.
public class AceGameDetector : GameDetector, IAceGameDetector
{
    private const string ProcessName = "AceEvo-Win64-Shipping";

    private static AceGameDetector? singletonInstance;

    public static AceGameDetector Instance => singletonInstance ??= new AceGameDetector();

    protected override bool IsGameRunning()
    {
        return Process.GetProcessesByName(ProcessName)
                      .Any();
    }
}
