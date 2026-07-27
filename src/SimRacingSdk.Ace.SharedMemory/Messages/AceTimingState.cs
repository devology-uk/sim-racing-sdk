#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Ace.SharedMemory.Messages;

// SMEvoTimingState from the PDF, fixed at 256 bytes. See AceTyreState.cs for why a trailing
// Reserved array is used to hit that documented size - unverified against a live game.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct AceTimingState
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string CurrentLapTime;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string DeltaCurrent;
    public int DeltaCurrentP;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string LastLapTime;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string DeltaLast;
    public int DeltaLastP;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string BestLapTime;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string IdealLapTime;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string TotalTime;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsInvalid;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 136)]
    public byte[] Reserved;
}
