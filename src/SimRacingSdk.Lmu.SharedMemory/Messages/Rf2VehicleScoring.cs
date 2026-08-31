#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches rF2VehicleScoring (rF2State.h) / VehicleScoringInfoV01 exactly. One entry per car in
// Rf2ScoringBuffer.Vehicles; Id is keyed against Rf2VehicleTelemetry.Id (same slot ID space).
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct Rf2VehicleScoring
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
    public Rf2Vec3 Pos;
    public Rf2Vec3 LocalVel;
    public Rf2Vec3 LocalAccel;

    // Orientation and derivatives
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public Rf2Vec3[] Ori;
    public Rf2Vec3 LocalRot;
    public Rf2Vec3 LocalRotAccel;

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

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
    public byte[] Expansion;

    public override readonly string ToString()
    {
        return $"Rf2VehicleScoring {{ Id = {this.Id}, DriverName = {this.DriverName}, "
             + $"TotalLaps = {this.TotalLaps}, Place = {this.Place}, IsPlayer = {this.IsPlayer}, "
             + $"LastLapTime = {this.LastLapTime}, BestLapTime = {this.BestLapTime}, "
             + $"NumPitstops = {this.NumPitstops}, NumPenalties = {this.NumPenalties} }}";
    }
}
