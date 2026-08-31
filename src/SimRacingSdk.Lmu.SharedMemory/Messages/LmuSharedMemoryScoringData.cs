#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches SharedMemoryScoringData (SharedMemoryInterface.hpp), mapped as part of "LMU_Data". Refreshed on
// LmuSharedMemoryEventType.UpdateScoring (~5FPS by the game).
//
// Pack = 8, not 4: declared outside InternalsPlugin.hpp's pack(4) region - see LmuSharedMemoryObjectOut. This one
// matters in practice: ScoringInfo (548 bytes) isn't itself a multiple of 8, so at natural alignment the compiler
// pads it to 552 bytes before ScoringStreamSize (a size_t) - a real 4-byte offset difference from what Pack = 4
// would produce, which desyncs every field read after it.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Ansi)]
public struct LmuSharedMemoryScoringData
{
    public const int MaxVehicles = 104;
    public const int ScoringStreamCapacity = 65536;

    public LmuScoringInfo ScoringInfo;

    // How many bytes of ScoringStream are actually valid - the buffer itself is always ScoringStreamCapacity bytes.
    public ulong ScoringStreamSize;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxVehicles)]
    public LmuVehicleScoring[] VehScoringInfo;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ScoringStreamCapacity)]
    public string ScoringStream;
}
