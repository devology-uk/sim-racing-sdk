#nullable disable

using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using SimRacingSdk.Ace.SharedMemory.Enums;
using SimRacingSdk.Ace.SharedMemory.Models;

namespace SimRacingSdk.Ace.SharedMemory.Messages;

// Field layout transcribed from ACE_SharedFileOut_Documentation_v1.pdf (SPageFileGraphicEvo),
// as of the 2026-04-28 revision (includes CarIds). Embedded sub-structs (AceTyreState etc.)
// carry an unverified Reserved padding block - see AceTyreState.cs.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public class AceGraphicsPage
{
    private const string GraphicsMap = "Local\\acevo_pmf_graphics";

    private static readonly int size = Marshal.SizeOf<AceGraphicsPage>();
    private static readonly byte[] buffer = new byte[size];

    public int PacketId;
    public AceStatus Status;
    public ulong FocusedCarIdA;
    public ulong FocusedCarIdB;
    public ulong PlayerCarIdA;
    public ulong PlayerCarIdB;
    public ushort Rpm;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsRpmLimiterOn;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsChangeUpRpm;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsChangeDownRpm;
    [MarshalAs(UnmanagedType.I1)]
    public bool TcActive;
    [MarshalAs(UnmanagedType.I1)]
    public bool AbsActive;
    [MarshalAs(UnmanagedType.I1)]
    public bool EscActive;
    [MarshalAs(UnmanagedType.I1)]
    public bool LaunchActive;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsIgnitionOn;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsEngineRunning;
    [MarshalAs(UnmanagedType.I1)]
    public bool KersIsCharging;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsWrongWay;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsDrsAvailable;
    [MarshalAs(UnmanagedType.I1)]
    public bool BatteryIsCharging;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsMaxKjPerLapReached;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsMaxChargeKjPerLapReached;
    public short DisplaySpeedKmh;
    public short DisplaySpeedMph;
    public short DisplaySpeedMs;
    public float PitSpeedingDelta;
    public short GearInt;
    public float RpmPercent;
    public float GasPercent;
    public float BrakePercent;
    public float HandbrakePercent;
    public float ClutchPercent;
    public float SteeringPercent;
    public float FfbStrength;
    public float CarFfbMultiplier;
    public float WaterTemperaturePercent;
    public float WaterPressureBar;
    public float FuelPressureBar;
    public sbyte WaterTemperatureC;
    public sbyte AirTemperatureC;
    public float OilTemperatureC;
    public float OilPressureBar;
    public float ExhaustTemperatureC;
    public float GForceX;
    public float GForceY;
    public float GForceZ;
    public float TurboBoost;
    public float TurboBoostLevel;
    public float TurboBoostPercent;
    public int SteerDegrees;
    public float CurrentKm;
    public uint TotalKm;
    public uint TotalDrivingTimeSeconds;
    public int TimeOfDayHours;
    public int TimeOfDayMinutes;
    public int TimeOfDaySeconds;
    public int DeltaTimeMs;
    public int CurrentLapTimeMs;
    public int PredictedLapTimeMs;
    public float FuelLiterCurrentQuantity;
    public float FuelLiterCurrentQuantityPercent;
    public float FuelLiterPerKm;
    public float KmPerFuelLiter;
    public float CurrentTorque;
    public int CurrentBhp;
    public AceTyreState TyreLf;
    public AceTyreState TyreRf;
    public AceTyreState TyreLr;
    public AceTyreState TyreRr;
    public float NormalizedPosition;
    public float KersChargePercent;
    public float KersCurrentPercent;
    public float ControlLockTime;
    public AceDamageState CarDamage;
    public AceCarLocation CarLocation;
    public AcePitInfo PitInfo;
    public float FuelLiterUsed;
    public float FuelLiterPerLap;
    public float LapsPossibleWithFuel;
    public float BatteryTemperature;
    public float BatteryVoltage;
    public float InstantaneousFuelLiterPerKm;
    public float InstantaneousKmPerFuelLiter;
    public float GearRpmWindow;
    public AceInstrumentation Instrumentation;
    public AceInstrumentation InstrumentationMinLimit;
    public AceInstrumentation InstrumentationMaxLimit;
    public AceElectronics Electronics;
    public AceElectronics ElectronicsMinLimit;
    public AceElectronics ElectronicsMaxLimit;
    public AceElectronics ElectronicsIsModifiable;
    public int TotalLapCount;
    public uint CurrentPosition;
    public uint TotalDrivers;
    public int LastLapTimeMs;
    public int BestLapTimeMs;
    public AceFlagType Flag;
    public AceFlagType GlobalFlag;
    public uint MaxGears;
    public AceEngineType EngineType;
    [MarshalAs(UnmanagedType.I1)]
    public bool HasKers;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsLastLap;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string PerformanceModeName;
    public float DiffCoastRawValue;
    public float DiffPowerRawValue;
    public int RaceCutGainedTimeMs;
    public int DistanceToDeadline;
    public float RaceCutCurrentDelta;
    public AceSessionState SessionState;
    public AceTimingState TimingState;
    public int PlayerPing;
    public int PlayerLatency;
    public int PlayerCpuUsage;
    public int PlayerCpuUsageAverage;
    public int PlayerQos;
    public int PlayerQosAverage;
    public int PlayerFps;
    public int PlayerFpsAverage;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string DriverName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string DriverSurname;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string CarModel;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsInPitBox;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsInPitLane;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsValidLap;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
    public AceCoordinate3d[] CarCoordinates;
    public float GapAhead;
    public float GapBehind;
    public byte ActiveCars;
    public float FuelPerLap;
    public float FuelEstimatedLaps;
    public AceAssistsState AssistsState;
    public float MaxFuel;
    public float MaxTurboBoost;
    [MarshalAs(UnmanagedType.I1)]
    public bool UseSingleCompound;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
    public AceCarIdPair[] CarIds;

    public static AceGraphicsPage Read()
    {
        using var mappedFile = MemoryMappedFile.OpenExisting(GraphicsMap, MemoryMappedFileRights.Read);
        using var stream = mappedFile.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);

        stream.ReadExactly(buffer, 0, buffer.Length);
        var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        var graphicsPage = Marshal.PtrToStructure<AceGraphicsPage>(handle.AddrOfPinnedObject());
        handle.Free();
        return graphicsPage;
    }
}
