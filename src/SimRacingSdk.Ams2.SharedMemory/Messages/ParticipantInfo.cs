using System.Numerics;
using System.Runtime.InteropServices;

namespace SimRacingSdk.Ams2.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential)]
public struct ParticipantInfo
{
    [MarshalAs(UnmanagedType.I1)]
    public bool IsActive;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SharedMemoryConstants.MaxStringLength)]
    public string Name;
    public Vector3 WorldPosition;
    public float DistanceIntoCurrentLap;
    public uint RacePosition;
    public uint LapsCompleted;
    public uint CurrentLap;
    public int CurrentSector;
}
