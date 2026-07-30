#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Ace.SharedMemory.Messages;

// SMEvoTyreState from the PDF. Only the documented fields are known; the PDF fixes this
// struct's total size at 256 bytes (2026-03-31 changelog: "inner structures have fixed size"),
// which is roughly double what the documented fields alone pack to. The trailing Reserved
// array exists purely to keep AceGraphicsPage's later fields at the correct byte offset -
// its contents are unmapped/unknown. Unverified against a live game; revisit if real data
// looks garbled once this can be tested against a running AC Evo instance.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct AceTyreState
{
    public float Slip;
    [MarshalAs(UnmanagedType.I1)]
    public bool Lock;
    public float TyrePressure;
    public float TyreTemperatureC;
    public float BrakeTemperatureC;
    public float BrakePressure;
    public float TyreTemperatureLeft;
    public float TyreTemperatureCenter;
    public float TyreTemperatureRight;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string TyreCompoundFront;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string TyreCompoundRear;
    public float TyreNormalizedPressure;
    public float TyreNormalizedTemperatureLeft;
    public float TyreNormalizedTemperatureCenter;
    public float TyreNormalizedTemperatureRight;
    public float BrakeNormalizedTemperature;
    public float TyreNormalizedTemperatureCore;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] Reserved;

    public override readonly string ToString()
    {
        return $"AceTyreState {{ Slip = {this.Slip}, Lock = {this.Lock}, TyrePressure = {this.TyrePressure}, "
             + $"TyreTemperatureC = {this.TyreTemperatureC}, BrakeTemperatureC = {this.BrakeTemperatureC}, "
             + $"BrakePressure = {this.BrakePressure}, TyreTemperatureLeft = {this.TyreTemperatureLeft}, "
             + $"TyreTemperatureCenter = {this.TyreTemperatureCenter}, TyreTemperatureRight = {this.TyreTemperatureRight}, "
             + $"TyreCompoundFront = {this.TyreCompoundFront}, TyreCompoundRear = {this.TyreCompoundRear}, "
             + $"TyreNormalizedPressure = {this.TyreNormalizedPressure}, "
             + $"TyreNormalizedTemperatureLeft = {this.TyreNormalizedTemperatureLeft}, "
             + $"TyreNormalizedTemperatureCenter = {this.TyreNormalizedTemperatureCenter}, "
             + $"TyreNormalizedTemperatureRight = {this.TyreNormalizedTemperatureRight}, "
             + $"BrakeNormalizedTemperature = {this.BrakeNormalizedTemperature}, "
             + $"TyreNormalizedTemperatureCore = {this.TyreNormalizedTemperatureCore} }}";
    }
}
