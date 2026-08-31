#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches SharedMemoryScoringData (SharedMemoryInterface.hpp), mapped as part of "LMU_Data". Refreshed on
// LmuSharedMemoryEventType.UpdateScoring (~5FPS by the game).
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
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
