#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches SharedMemoryGeneric (SharedMemoryInterface.hpp). Events is indexed by LmuSharedMemoryEventType; a
// non-zero slot means that event type was signalled since the last read.
//
// Pack = 8, not 4: declared outside InternalsPlugin.hpp's pack(4) region - see LmuSharedMemoryObjectOut.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct LmuSharedMemoryGeneric
{
    public const int EventCount = 16;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = EventCount)]
    public uint[] Events;
    public int GameVersion;
    public float FfbTorque;
    public LmuApplicationState AppInfo;
}
