#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches rF2Scoring (rF2State.h), mapped as $rFactor2SMMP_Scoring$. Refreshed at ~5FPS by the game.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Rf2ScoringBuffer
{
    public int BytesUpdatedHint;

    public Rf2ScoringInfo ScoringInfo;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = Rf2TelemetryBuffer.MaxMappedVehicles)]
    public Rf2VehicleScoring[] Vehicles;
}
