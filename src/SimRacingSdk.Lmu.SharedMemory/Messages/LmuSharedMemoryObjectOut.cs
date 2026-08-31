#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches SharedMemoryObjectOut (SharedMemoryInterface.hpp) - the full layout mapped as "LMU_Data". SharedMemoryLayout
// in the source is just a single-field wrapper around this, so it's the same size/layout and doesn't need its own
// C# type - this struct is read directly.
//
// Pack = 8, not 4: SharedMemoryInterface.hpp's structs are declared after InternalsPlugin.hpp's
// "#pragma pack(push, 4)...pop" region has already closed, so they use the compiler's natural x64 alignment - only
// the InternalsPlugin.hpp types embedded inside them (LmuScoringInfo, LmuVehicleScoring, LmuVehicleTelemetry,
// LmuApplicationState) are genuinely pack(4).
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct LmuSharedMemoryObjectOut
{
    public LmuSharedMemoryGeneric Generic;
    public LmuSharedMemoryPathData Paths;
    public LmuSharedMemoryScoringData Scoring;
    public LmuSharedMemoryTelemetryData Telemetry;
}
