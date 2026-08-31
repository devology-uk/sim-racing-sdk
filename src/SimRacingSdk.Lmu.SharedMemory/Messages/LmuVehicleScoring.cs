#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches VehicleScoringInfoV01 (InternalsPlugin.hpp, official LMU SDK header) exactly. One entry per car in
// LmuSharedMemoryScoringData.VehScoringInfo; Id is the same slot-ID space as LmuVehicleTelemetry.Id, though the
// official interface also gives the player's index directly (LmuSharedMemoryTelemetryData.PlayerVehicleIdx), so
// cross-referencing by Id is no longer needed just to find the player's own car.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct LmuVehicleScoring
{
    public int Id;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string DriverName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string VehicleName;
    public short TotalLaps;
    public sbyte Sector;
    public sbyte FinishStatus;
    public double LapDist;
    public double PathLateral;
    public double TrackEdge;

    public double BestSector1;
    public double BestSector2;
    public double BestLapTime;
    public double LastSector1;
    public double LastSector2;
    public double LastLapTime;
    public double CurSector1;
    public double CurSector2;

    public short NumPitstops;
    public short NumPenalties;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsPlayer;

    public sbyte Control;
    [MarshalAs(UnmanagedType.I1)]
    public bool InPits;
    public byte Place;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string VehicleClass;

    // Dash indicators
    public double TimeBehindNext;
    public int LapsBehindNext;
    public double TimeBehindLeader;
    public int LapsBehindLeader;
    public double LapStartEt;

    // Position and derivatives
    public LmuVect3 Pos;
    public LmuVect3 LocalVel;
    public LmuVect3 LocalAccel;

    // Orientation and derivatives
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public LmuVect3[] Ori;
    public LmuVect3 LocalRot;
    public LmuVect3 LocalRotAccel;

    public byte Headlights;
    public byte PitState;
    public byte ServerScored;
    public byte IndividualPhase;

    public int Qualification;

    public double TimeIntoLap;
    public double EstimatedLapTime;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
    public string PitGroup;
    public byte Flag;
    [MarshalAs(UnmanagedType.I1)]
    public bool UnderYellow;
    public byte CountLapFlag;
    [MarshalAs(UnmanagedType.I1)]
    public bool InGarageStall;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] UpgradePack;
    public float PitLapDist;

    public float BestLapSector1;
    public float BestLapSector2;

    public ulong SteamId;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string VehFilename;

    public short AttackMode;

    // 0x00 = 0%, 0xFF = 100% of fuel or battery remaining.
    public byte FuelFraction;

    [MarshalAs(UnmanagedType.I1)]
    public bool DrsState;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] Expansion;

    public override readonly string ToString()
    {
        return $"LmuVehicleScoring {{ Id = {this.Id}, DriverName = {this.DriverName}, "
             + $"TotalLaps = {this.TotalLaps}, Place = {this.Place}, IsPlayer = {this.IsPlayer}, "
             + $"LastLapTime = {this.LastLapTime}, BestLapTime = {this.BestLapTime}, "
             + $"FuelFraction = {this.FuelFraction}, SteamId = {this.SteamId} }}";
    }
}
