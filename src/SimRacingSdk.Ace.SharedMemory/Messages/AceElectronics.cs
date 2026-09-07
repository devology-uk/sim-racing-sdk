#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Ace.SharedMemory.Messages;

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

    public override readonly string ToString()
    {
        return $"AceElectronics {{ TcLevel = {this.TcLevel}, TcCutLevel = {this.TcCutLevel}, "
             + $"AbsLevel = {this.AbsLevel}, EscLevel = {this.EscLevel}, EbbLevel = {this.EbbLevel}, "
             + $"BrakeBias = {this.BrakeBias}, EngineMapLevel = {this.EngineMapLevel}, TurboLevel = {this.TurboLevel}, "
             + $"ErsDeploymentMap = {this.ErsDeploymentMap}, ErsRechargeMap = {this.ErsRechargeMap}, "
             + $"IsErsHeatChargingOn = {this.IsErsHeatChargingOn}, IsErsOvertakeModeOn = {this.IsErsOvertakeModeOn}, "
             + $"IsDrsOpen = {this.IsDrsOpen}, DiffPowerLevel = {this.DiffPowerLevel}, DiffCoastLevel = {this.DiffCoastLevel}, "
             + $"FrontBumpDamperLevel = {this.FrontBumpDamperLevel}, FrontReboundDamperLevel = {this.FrontReboundDamperLevel}, "
             + $"RearBumpDamperLevel = {this.RearBumpDamperLevel}, RearReboundDamperLevel = {this.RearReboundDamperLevel}, "
             + $"IsIgnitionOn = {this.IsIgnitionOn}, IsPitLimiterOn = {this.IsPitLimiterOn}, "
             + $"ActivePerformanceMode = {this.ActivePerformanceMode} }}";
    }
}
