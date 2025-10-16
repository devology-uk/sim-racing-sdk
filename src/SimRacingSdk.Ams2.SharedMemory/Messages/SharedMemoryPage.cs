#nullable disable

using System.IO.MemoryMappedFiles;
using System.Numerics;
using System.Runtime.InteropServices;

namespace SimRacingSdk.Ams2.SharedMemory.Messages;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public class SharedMemoryPage
{
    private const string SharedMemoryMap = "$pcars$";

    private static readonly int size = Marshal.SizeOf<SharedMemoryPage>();
    private static readonly byte[] buffer = new byte[size];

    public uint Version;
    public uint BuildVersionNumber;
    public uint GameState;
    public uint SessionState;
    public uint RaceState;
    public int FocusedParticipantIndex;
    public int ParticipantCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public ParticipantInfo[] Participants;
    public float UnfilteredThrottle;
    public float UnfilteredBrake;
    public float UnfilteredSteering;
    public float UnfilteredClutch;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SharedMemoryConstants.MaxStringLength)]
    public string CarName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SharedMemoryConstants.MaxStringLength)]
    public string CarClassName;
    public uint LapsInEvent;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SharedMemoryConstants.MaxStringLength)]
    public string TrackLocation;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SharedMemoryConstants.MaxStringLength)]
    public string TrackLayout;
    public float TrackLength;
    public int SectorCount;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsLapInvalid;
    public float BestLapTime;
    public float LastLapTime;
    public float CurrentLapTime;
    public float SplitTimeAhead;
    public float SplitTimeBehind;
    public float EventTimeRemaining;
    public float PersonalBestLapTime;
    public float OverallBestLapTime;
    public float CurrentSector1Time;
    public float CurrentSector2Time;
    public float CurrentSector3Time;
    public float BestSector1Time;
    public float BestSector2Time;
    public float BestSector3Time;
    public float OverallBestSector1Time;
    public float OverallBestSector2Time;
    public float OverallBestSector3Time;
    public uint HighestFlagColour;
    public uint HighestFlagReason;
    public uint PitMode;
    public uint PitSchedule;
    public uint CarFlags;
    public float OilTempC;
    public float OilPressureKpa;
    public float WaterTempC;
    public float WaterPressureKpa;
    public float FuelPressureKpa;
    public float FuelLevel;
    public float FuelCapacity;
    public float Speed;
    public float Rpm;
    public float MaxRpm;
    public float Brake;
    public float Throttle;
    public float Clutch;
    public float Steering;
    public int Gear;
    public int GearCount;
    public float OdometerKm;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsAbsActive;
    public int LastOpponentCollisionIndex;
    public float LasOpponentCollisionMagnitude;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsBoostActive;
    public Vector3 Orientation;
    public Vector3 LocalVelocity;
    public Vector3 WorldVelocity;
    public Vector3 AngularVelocity;
    public Vector3 LocalAcceleration;
    public Vector3 WorldAcceleration;
    public Vector3 ExtentsCenter;
    public Wheel<uint> TyreFlags;
    public Wheel<uint> Terrain;
    public Wheel<float> TyreY;
    public Wheel<float> TyreRps;
    public Wheel<float> TyreSlipSpeed;
    public Wheel<float> TyreTemp;
    public Wheel<float> TyreGrip;
    public Wheel<float> TyreHeightAboveGround;
    public Wheel<float> TyreWear;
    public Wheel<float> BrakeDamage;
    public Wheel<float> SuspensionDamage;
    public Wheel<float> BrakeTempC;
    public Wheel<float> TyreTreadTempC;
    public Wheel<float> TyreLayerTempC;
    public Wheel<float> TyreCarcassTempC;
    public Wheel<float> TyreRimTempC;
    public Wheel<float> TyreInternalAirTempC;
    public uint CrashState;
    public float AeroDamage;
    public float EngineDamage;
    public float AmbientTempC;
    public float TrackTempC;
    public float RainDensity;
    public float WindDirectionX;
    public float WindDirectionY;
    public float CloudBrightness;
    public volatile float SequenceNumber;
    public Wheel<float> WheelLocalPositionY;
    public Wheel<float> SuspensionTravel;
    public Wheel<float> SuspensionVelocity;
    public Wheel<float> AirPressure;
    public float EngineSpeed;
    public float EngineTorque;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public float[] Wings;
    public float HandBrake;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public float[] CurrentSector1Times;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public float[] CurrentSector2Times;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public float[] CurrentSector3Times;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public float[] BestSector1Times;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public float[] BestSector2Times;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public float[] BestSector3Times;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public float[] LastLapTimes;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public byte[] IsLapInvalidated;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public uint[] RaceStates;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public uint[] PitModes;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public Vector3[] Orientations;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public float[] Speeds;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public string[] CarNames;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public string[] CarClassNames;
    public int PitStopEnforcedOnLap;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SharedMemoryConstants.MaxStringLength)]
    public string TranslatedTrackLocation;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SharedMemoryConstants.MaxStringLength)]
    public string TranslatedTrackLayout;
    public float BrakeBias;
    public float TurboBoostPressure;
    public Wheel<string> TyreCompound;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public uint[] PitSchedules;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public uint[] HighestFlagColours;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public uint[] HighestFlagReasons;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SharedMemoryConstants.MaxParticipants)]
    public uint[] Nationalities;
    public float SnowDensity;
    public float SessionDuration;
    public int SessionAdditionalLaps;
    public Wheel<float> TyreTempLeftC;
    public Wheel<float> TyreTempCentreC;
    public Wheel<float> TyreTempRightC;
    public uint DrsState;
    public Wheel<float> RideHeight;
    public uint JoyPad0;
    public uint DPad;
    public int AbsSetting;
    public int TCSetting;
    public int ErsDeploymentMode;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsErsAutoModeEnabled;
    public float ClutchTempC;
    public float ClutchWear;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsClutchOverheated;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsClutchSlipping;
    public int YellowFlagState;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsPrivateSession;
    public int LaunchStage;

    public static SharedMemoryPage Read()
    {
        using var mappedFile = MemoryMappedFile.OpenExisting(SharedMemoryMap, MemoryMappedFileRights.Read);
        using var stream = mappedFile.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);

        stream.ReadExactly(buffer, 0, buffer.Length);
        var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        var dataPage = Marshal.PtrToStructure<SharedMemoryPage>(handle.AddrOfPinnedObject());
        handle.Free();
        return dataPage;
    }
}