#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches rF2Telemetry (rF2State.h), mapped as $rFactor2SMMP_Telemetry$. Refreshed at ~50FPS by the game.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Rf2TelemetryBuffer
{
    public const int MaxMappedVehicles = 128;

    public int BytesUpdatedHint;
    public int NumVehicles;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxMappedVehicles)]
    public Rf2VehicleTelemetry[] Vehicles;
}
