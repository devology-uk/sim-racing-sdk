#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Ace.SharedMemory.Messages;

// SMEvoInstrumentation from the PDF, fixed at 128 bytes. See AceTyreState.cs for why a trailing
// Reserved array is used to hit that documented size - unverified against a live game.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct AceInstrumentation
{
    public sbyte MainLightStage;
    public sbyte SpecialLightStage;
    public sbyte CockpitLightStage;
    public sbyte WiperLevel;
    [MarshalAs(UnmanagedType.I1)]
    public bool RainLights;
    [MarshalAs(UnmanagedType.I1)]
    public bool DirectionLightLeft;
    [MarshalAs(UnmanagedType.I1)]
    public bool DirectionLightRight;
    [MarshalAs(UnmanagedType.I1)]
    public bool FlashingLights;
    [MarshalAs(UnmanagedType.I1)]
    public bool WarningLights;
    public sbyte SelectedDisplayIndex;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public sbyte[] DisplayCurrentPageIndex;
    [MarshalAs(UnmanagedType.I1)]
    public bool AreHeadlightsVisible;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 101)]
    public byte[] Reserved;

    public override readonly string ToString()
    {
        return $"AceInstrumentation {{ MainLightStage = {this.MainLightStage}, SpecialLightStage = {this.SpecialLightStage}, "
             + $"CockpitLightStage = {this.CockpitLightStage}, WiperLevel = {this.WiperLevel}, RainLights = {this.RainLights}, "
             + $"DirectionLightLeft = {this.DirectionLightLeft}, DirectionLightRight = {this.DirectionLightRight}, "
             + $"FlashingLights = {this.FlashingLights}, WarningLights = {this.WarningLights}, "
             + $"SelectedDisplayIndex = {this.SelectedDisplayIndex}, "
             + $"DisplayCurrentPageIndex = {SharedMemoryLogFormatting.FormatArray(this.DisplayCurrentPageIndex)}, "
             + $"AreHeadlightsVisible = {this.AreHeadlightsVisible} }}";
    }
}
