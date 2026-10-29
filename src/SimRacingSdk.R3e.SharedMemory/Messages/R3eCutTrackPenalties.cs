#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eCutTrackPenalties
{
    public float DriveThrough;
    public float StopAndGo;
    public float PitStop;
    public float TimeDeduction;
    public float SlowDown;
}
