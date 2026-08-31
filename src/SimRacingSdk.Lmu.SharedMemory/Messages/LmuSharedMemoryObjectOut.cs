#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches SharedMemoryObjectOut (SharedMemoryInterface.hpp) - the full layout mapped as "LMU_Data". SharedMemoryLayout
// in the source is just a single-field wrapper around this, so it's the same size/layout and doesn't need its own
// C# type - this struct is read directly.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct LmuSharedMemoryObjectOut
{
    public LmuSharedMemoryGeneric Generic;
    public LmuSharedMemoryPathData Paths;
    public LmuSharedMemoryScoringData Scoring;
    public LmuSharedMemoryTelemetryData Telemetry;
}
