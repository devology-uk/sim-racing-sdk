#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eDriverData
{
    public R3eDriverInfo DriverInfo;
    public int FinishStatus;
    public int Place;
    public int PlaceClass;
    public float LapDistance;
    public float LapDistanceFraction;
    public R3eVector3Single Position;
    public int TrackSector;
    public int CompletedLaps;
    public int CurrentLapValid;
    public float LapTimeCurrentSelf;
    public R3eSectors SectorTimeCurrentSelf;
    public R3eSectors SectorTimePreviousSelf;
    public R3eSectors SectorTimeBestSelf;
    public float TimeDeltaFront;
    public float TimeDeltaBehind;
    public int PitStopStatus;
    public int InPitlane;
    public int NumPitstops;
    public R3eCutTrackPenalties Penalties;
    public float CarSpeed;
    public int TyreTypeFront;
    public int TyreTypeRear;
    public int TyreSubtypeFront;
    public int TyreSubtypeRear;
    public float BasePenaltyWeight;
    public float AidPenaltyWeight;
    public int DrsState;
    public int PtpState;
    public float VirtualEnergy;
    public int PenaltyType;
    public int PenaltyReason;
    public int EngineState;
    public R3eVector3Single Orientation;
    public float Unused1;
    public float Unused2;
    public float Unused3;
}
