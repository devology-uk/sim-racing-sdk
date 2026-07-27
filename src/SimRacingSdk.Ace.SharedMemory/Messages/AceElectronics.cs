#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Ace.SharedMemory.Messages;

// SMEvoElectronics from the PDF, fixed at 128 bytes. See AceTyreState.cs for why a trailing
// Reserved array is used to hit that documented size - unverified against a live game.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct AceElectronics
{
    public sbyte TcLevel;
    public sbyte TcCutLevel;
    public sbyte AbsLevel;
    public sbyte EscLevel;
    public sbyte EbbLevel;
    public float BrakeBias;
    public sbyte EngineMapLevel;
    public float TurboLevel;
    public sbyte ErsDeploymentMap;
    public float ErsRechargeMap;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsErsHeatChargingOn;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsErsOvertakeModeOn;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsDrsOpen;
    public sbyte DiffPowerLevel;
    public sbyte DiffCoastLevel;
    public sbyte FrontBumpDamperLevel;
    public sbyte FrontReboundDamperLevel;
    public sbyte RearBumpDamperLevel;
    public sbyte RearReboundDamperLevel;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsIgnitionOn;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsPitLimiterOn;
    public sbyte ActivePerformanceMode;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 88)]
    public byte[] Reserved;
}
