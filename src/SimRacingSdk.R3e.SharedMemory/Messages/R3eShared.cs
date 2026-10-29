#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

// Matches r3e_shared (r3e.h, API 3.5) field for field; every struct in the header is #pragma pack(1).
// "Not available" is -1 (-2 for Gear, -1000 for TimeDeltaBestSelf) almost everywhere.
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eShared
{
    public const int MaxCars = 128;

    // Version
    public int VersionMajor;
    public int VersionMinor;
    public int AllDriversOffset;
    public int DriverDataSize;

    // Game state
    public int GameMode;
    public int GamePaused;
    public int GameInMenus;
    public int GameInReplay;
    public int GameUsingVr;
    public int GamePlayerInGarage;

    public R3ePlayerData Player;

    // Event and session
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] TrackName;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] LayoutName;
    public int TrackId;
    public int LayoutId;
    public float LayoutLength;
    public R3eSectors SectorStartFactors;
    public R3eRaceDuration RaceSessionLaps;
    public R3eRaceDuration RaceSessionMinutes;
    public int EventIndex;
    public int SessionType;
    public int SessionIteration;
    public int SessionLengthFormat;
    public float SessionPitSpeedLimit;
    public int SessionPhase;
    public int StartLights;
    public int TireWearActive;
    public int FuelUseActive;
    public int NumberOfLaps;
    public float SessionTimeDuration;
    public float SessionTimeRemaining;
    public int MaxIncidentPoints;
    public float EventUnused1;
    public float EventUnused2;

    // Pit
    public int PitWindowStatus;
    public int PitWindowStart;
    public int PitWindowEnd;
    public int InPitlane;
    public int PitMenuSelection;
    public R3ePitMenuState PitMenuState;
    public int PitState;
    public float PitTotalDuration;
    public float PitElapsedTime;
    public int PitAction;
    public int NumPitstopsPerformed;
    public float PitMinDurationTotal;
    public float PitMinDurationLeft;

    // Scoring and timings
    public R3eFlags Flags;
    public int Position;
    public int PositionClass;
    public int FinishStatus;
    public int CutTrackWarnings;
    public R3eCutTrackPenalties Penalties;
    public int NumPenalties;
    public int CompletedLaps;
    public int CurrentLapValid;
    public int TrackSector;
    public float LapDistance;
    public float LapDistanceFraction;
    public float LapTimeBestLeader;
    public float LapTimeBestLeaderClass;
    public R3eSectors SectorTimesSessionBestLap;
    public float LapTimeBestSelf;
    public R3eSectors SectorTimesBestSelf;
    public float LapTimePreviousSelf;
    public R3eSectors SectorTimesPreviousSelf;
    public float LapTimeCurrentSelf;
    public R3eSectors SectorTimesCurrentSelf;
    public float LapTimeDeltaLeader;
    public float LapTimeDeltaLeaderClass;
    public float TimeDeltaFront;
    public float TimeDeltaBehind;
    public float TimeDeltaBestSelf;
    public R3eSectors BestIndividualSectorTimeSelf;
    public R3eSectors BestIndividualSectorTimeLeader;
    public R3eSectors BestIndividualSectorTimeLeaderClass;
    public int IncidentPoints;
    public int LapValidState;
    public int PrevLapValid;
    public float DischargeRate;
    public float BrakeRegen;
    public float ScoringUnused1;

    // Vehicle information
    public R3eDriverInfo VehicleInfo;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] PlayerName;

    // Vehicle state
    public int ControlType;
    public float CarSpeed;
    public float EngineRps;
    public float MaxEngineRps;
    public float UpshiftRps;
    public int Gear;
    public int NumGears;
    public R3eVector3Single CarCgLocation;
    public R3eOrientation CarOrientation;
    public R3eVector3Single LocalAcceleration;
    public float TotalMass;
    public float FuelLeft;
    public float FuelCapacity;
    public float FuelPerLap;
    public float VirtualEnergyLeft;
    public float VirtualEnergyCapacity;
    public float VirtualEnergyPerLap;
    public float EngineTemp;
    public float EngineOilTemp;
    public float FuelPressure;
    public float EngineOilPressure;
    public float TurboPressure;
    public float Throttle;
    public float ThrottleRaw;
    public float Brake;
    public float BrakeRaw;
    public float Clutch;
    public float ClutchRaw;
    public float SteerInputRaw;
    public int SteerLockDegrees;
    public int SteerWheelRangeDegrees;
    public R3eAidSettings AidSettings;
    public R3eDrs Drs;
    public int PitLimiter;
    public R3ePushToPass PushToPass;
    public float BrakeBias;
    public int DrsNumActivationsTotal;
    public int PtpNumActivationsTotal;
    public float BatterySoC;
    public float WaterLeft;
    public int AbsSetting;
    public int HeadLights;
    public int SteerWheelMaxRotation;

    // Tyres
    public int TireType;
    public R3eWheelsSingle TireRps;
    public R3eWheelsSingle TireSpeed;
    public R3eWheelsSingle TireGrip;
    public R3eWheelsSingle TireWear;
    public R3eWheelsInt TireFlatspot;
    public R3eWheelsSingle TirePressure;
    public R3eWheelsSingle TireDirt;
    public R3eWheelsTyreTemperature TireTemp;
    public int TireTypeFront;
    public int TireTypeRear;
    public int TireSubtypeFront;
    public int TireSubtypeRear;
    public R3eWheelsBrakeTemperature BrakeTemp;
    public R3eWheelsSingle BrakePressure;
    public int TractionControlSetting;
    public int EngineMapSetting;
    public int EngineBrakeSetting;
    public float TractionControlPercent;
    public R3eWheelsInt TireOnMtrl;
    public R3eWheelsSingle TireLoad;

    // Damage
    public R3eCarDamage CarDamage;

    // Driver info, in place order
    public int NumCars;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxCars)]
    public R3eDriverData[] DriverData;
}
