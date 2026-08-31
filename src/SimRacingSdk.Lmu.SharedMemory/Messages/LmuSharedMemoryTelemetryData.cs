#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches SharedMemoryTelemetryData (SharedMemoryInterface.hpp), mapped as part of "LMU_Data". Refreshed on
// LmuSharedMemoryEventType.UpdateTelemetry (~50FPS by the game). PlayerVehicleIdx/PlayerHasVehicle mean the
// player's own car never needs to be found by cross-referencing IsPlayer from the Scoring data.
//
// Pack = 8, not 4: declared outside InternalsPlugin.hpp's pack(4) region - see LmuSharedMemoryObjectOut.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct LmuSharedMemoryTelemetryData
{
    public byte ActiveVehicles;
    public byte PlayerVehicleIdx;
    [MarshalAs(UnmanagedType.I1)]
    public bool PlayerHasVehicle;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = LmuSharedMemoryScoringData.MaxVehicles)]
    public LmuVehicleTelemetry[] TelemInfo;
}
