#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches LMU_Extended (LMU_State.h) from the LMU-specific plugin, mapped as $LMU_SMMP_Extended$. Unlike
// Rf2TelemetryBuffer/Rf2ScoringBuffer, this does NOT start with a BytesUpdatedHint field - LMU_Extended derives
// from the plain header (no size hint), not the WithSize variant. Most fields here are sourced by the plugin via
// raw process-memory offsets (DirectMemoryReader) rather than the documented ISI plugin API, so they only populate
// when EnableDirectMemoryAccess is on - see LmuSharedMemoryPluginInstaller.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct LmuExtendedBuffer
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
    public string Version;
    [MarshalAs(UnmanagedType.I1)]
    public bool Is64Bit;

    [MarshalAs(UnmanagedType.I1)]
    public bool InRealtimeFc;

    [MarshalAs(UnmanagedType.I1)]
    public bool SessionStarted;
    public ulong TicksSessionStarted;
    public ulong TicksSessionEnded;

    [MarshalAs(UnmanagedType.I1)]
    public bool DirectMemoryAccessEnabled;

    public int UnsubscribedBuffersMask;

    public int BrakeMigration;
    public int BrakeMigrationMax;
    public int TractionControl;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string MotorMap;
    public int ChangedParamType;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string ChangedParamValue;
    public int FrontAbr;
    public int RearAbr;
    public int PenaltyType;
    public int PenaltyCount;
    public int PenaltyLeftLaps;
    public int PendingPenaltyType1;
    public int PendingPenaltyType2;
    public int PendingPenaltyType3;
    public float Cuts;
    public int CutsPoints;
    public double CurrentBatteryValue;
    public double MaxBatteryValue;
    public double CurrentEnergyValue;
    public double MaxEnergyValue;
    public double CurrentFuelValue;
    public double MaxFuelValue;
    public float EnergyLastLap;
    public float FuelLastLap;

    public override readonly string ToString()
    {
        return $"LmuExtendedBuffer {{ DirectMemoryAccessEnabled = {this.DirectMemoryAccessEnabled}, "
             + $"CurrentFuelValue = {this.CurrentFuelValue}, MaxFuelValue = {this.MaxFuelValue}, "
             + $"CurrentEnergyValue = {this.CurrentEnergyValue}, MaxEnergyValue = {this.MaxEnergyValue}, "
             + $"CurrentBatteryValue = {this.CurrentBatteryValue}, TractionControl = {this.TractionControl}, "
             + $"PenaltyType = {this.PenaltyType}, PenaltyCount = {this.PenaltyCount}, Cuts = {this.Cuts} }}";
    }
}
