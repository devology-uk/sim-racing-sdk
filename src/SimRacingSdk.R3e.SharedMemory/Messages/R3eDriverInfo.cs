#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eDriverInfo
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] Name;
    public int CarNumber;
    public int ClassId;
    public int ModelId;
    public int TeamId;
    public int LiveryId;
    public int ManufacturerId;
    public int UserId;
    public int SlotId;
    public int ClassPerformanceIndex;
    public int EngineType;
    public float CarWidth;
    public float CarLength;
    public float Rating;
    public float Reputation;
    public float Unused1;
    public float Unused2;
}
