#nullable disable

using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using SimRacingSdk.Acc.SharedMemory.Enums;
using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.SharedMemory.Messages;

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
public class GraphicsPage
{
    private const string GraphicsMap = "Local\\acpmf_graphics";

    private static readonly int size = Marshal.SizeOf<GraphicsPage>();
    private static readonly byte[] buffer = new byte[size];
    
    public int PacketId;
    public AccAppStatus Status;
    public AccSessionType SessionType;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string CurrentTime;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string LastTime;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string BestTime;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string Split;
    public int CompletedLaps;
    public int Position;
    public int CurrentTimeMs;
    public int LastTimeMs;
    public int BestTimeMs;
    public float SessionTimeLeft;
    public float DistanceTraveled;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsInPits;
    public int CurrentSectorIndex;
    public int LastSectorTime;
    public int NumberOfLaps;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string TyreCompound;
    public float ReplayTimeMultiplier;
    public float NormalizedCarPosition;
    public int ActiveCars;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
    public AccCoordinate3d[] CarCoordinates;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
    public int[] CarIds;
    public int PlayerCarID;
    public float PenaltyTime;
    public AccFlagType Flag;
    public AccPenaltyType PenaltyType;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsIdealLineOn;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsInPitLane;
    public float SurfaceGrip;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsMandatoryPitStopComplete;
    public float WindSpeed;
    public float WindDirection;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsSetupMenuVisible;
    public int MainDisplayIndex;
    public int SecondaryDisplayIndex;
    public int TC;
    public int TCCut;
    public int EngineMap;
    public int ABS;
    public float AverageFuelPerLap;
    public int RainLights;
    public int FlashingLights;
    public int LightsStage;
    public float ExhaustTemperature;
    public int WiperStage;
    public int DriverStintTotalTimeLeft;
    public int DriverStintTimeLeft;
    public int RainTyres;
    public int SessionIndex;
    public float FuelUsedSinceRefuel;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string DeltaLapTime;
    public int DeltaLapTimeMillis;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string EstimatedLapTime;
    public int EstimatedLapTimeMs;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsDeltaPositive;
    public int SplitTimeMillis;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsValidLap;
    public float FuelEstimatedLaps;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string TrackStatus;
    public int MandatoryPitStopsLeft;
    public float ClockTimeOfDaySeconds;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsLeftIndicatorOn;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsRightIndicatorOn;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsYellowFlagActive;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsYellowFlagActiveInSector1;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsYellowFlagActiveInSector2;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsYellowActiveInSector3;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsWhiteFlagActive;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsGreenFlagActive;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsChequeredFlagActive;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsRedFlagActive;
    public int MfdTyreSet;
    public float MfdFuelToAdd;
    public float MfdTyrePressureLF;
    public float MfdTyrePressureRF;
    public float MfdTyrePressureLR;
    public float MfdTyrePressureRR;
    public AccTrackGripStatus TrackGripStatus;
    public AccRainIntensity RainIntensity;
    public AccRainIntensity RainIntensityIn10Minutes;
    public AccRainIntensity RainIntensityIn30Minutes;
    public int CurrentTyreSet;
    public int StrategyTyreSet;
    public int GapAheadMs;
    public int GapBehindMs;

    public static bool TryRead(out GraphicsPage graphicsPage)
    {
        try
        {
            using var mappedFile = MemoryMappedFile.OpenExisting(GraphicsMap, MemoryMappedFileRights.Read);
            using var stream = mappedFile.CreateViewStream(0,0, MemoryMappedFileAccess.Read);

            stream.ReadExactly(buffer, 0, buffer.Length);
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            graphicsPage = Marshal.PtrToStructure<GraphicsPage>(handle.AddrOfPinnedObject());
            handle.Free();
            return true;

        }
        catch(FileNotFoundException)
        {
            graphicsPage = null;
            return false;
        }
    }
}