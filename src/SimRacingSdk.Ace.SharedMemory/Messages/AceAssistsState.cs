#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Ace.SharedMemory.Messages;

// SMEvoAssistsState from the PDF, fixed at 64 bytes. See AceTyreState.cs for why a trailing
// Reserved array is used to hit that documented size - unverified against a live game.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct AceAssistsState
{
    public byte AutoGear;
    public byte AutoBlip;
    public byte AutoClutch;
    public byte AutoClutchOnStart;
    public byte ManualIgnitionEStart;
    public byte AutoPitLimiter;
    public byte StandingStartAssist;
    public float AutoSteer;
    public float ArcadeStabilityControl;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
    public byte[] Reserved;

    public override readonly string ToString()
    {
        return $"AceAssistsState {{ AutoGear = {this.AutoGear}, AutoBlip = {this.AutoBlip}, "
             + $"AutoClutch = {this.AutoClutch}, AutoClutchOnStart = {this.AutoClutchOnStart}, "
             + $"ManualIgnitionEStart = {this.ManualIgnitionEStart}, AutoPitLimiter = {this.AutoPitLimiter}, "
             + $"StandingStartAssist = {this.StandingStartAssist}, AutoSteer = {this.AutoSteer}, "
             + $"ArcadeStabilityControl = {this.ArcadeStabilityControl} }}";
    }
}
