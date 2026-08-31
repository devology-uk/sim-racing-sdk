#nullable disable

using System.Runtime.InteropServices;
using SimRacingSdk.Lmu.SharedMemory.Enums;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches TelemInfoV01 (InternalsPlugin.hpp, the official LMU/Studio 397 SDK header - newer and richer than the
// version the community rF2 plugins mirror) exactly - field order and types are load-bearing for marshalling.
// One entry per car in LmuSharedMemoryTelemetryData.TelemInfo.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct LmuVehicleTelemetry
{
    // Time
    public int Id;
    public double DeltaTime;
    public double ElapsedTime;
    public int LapNumber;
    public double LapStartEt;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string VehicleName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string TrackName;

    // Position and derivatives
    public LmuVect3 Pos;
    public LmuVect3 LocalVel;
    public LmuVect3 LocalAccel;

    // Orientation and derivatives
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public LmuVect3[] Ori;
    public LmuVect3 LocalRot;
    public LmuVect3 LocalRotAccel;

    // Vehicle status
    public int Gear;
    public double EngineRpm;
    public double EngineWaterTemp;
    public double EngineOilTemp;
    public double ClutchRpm;

    // Driver input (unfiltered)
    public double UnfilteredThrottle;
    public double UnfilteredBrake;
    public double UnfilteredSteering;
    public double UnfilteredClutch;

    // Driver input (filtered)
    public double FilteredThrottle;
    public double FilteredBrake;
    public double FilteredSteering;
    public double FilteredClutch;

    // Misc
    public double SteeringShaftTorque;
    public double Front3rdDeflection;
    public double Rear3rdDeflection;

    // Aerodynamics
    public double FrontWingHeight;
    public double FrontRideHeight;
    public double RearRideHeight;
    public double Drag;
    public double FrontDownforce;
    public double RearDownforce;

    // State/damage info
    public double Fuel;
    public double EngineMaxRpm;
    public byte ScheduledStops;
    [MarshalAs(UnmanagedType.I1)]
    public bool Overheating;
    [MarshalAs(UnmanagedType.I1)]
    public bool Detached;
    [MarshalAs(UnmanagedType.I1)]
    public bool Headlights;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public byte[] DentSeverity;
    public double LastImpactEt;
    public double LastImpactMagnitude;
    public LmuVect3 LastImpactPos;

    // Expanded
    public double EngineTorque;
    public int CurrentSector;
    public byte SpeedLimiter;
    public byte MaxGears;
    public byte FrontTireCompoundIndex;
    public byte RearTireCompoundIndex;
    public double FuelCapacity;
    public byte FrontFlapActivated;
    public byte RearFlapActivated;
    public byte RearFlapLegalStatus;
    public byte IgnitionStarter;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
    public string FrontTireCompoundName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
    public string RearTireCompoundName;

    public byte SpeedLimiterAvailable;
    public byte AntiStallActivated;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public byte[] Unused;
    public float VisualSteeringWheelRange;

    public double RearBrakeBias;
    public double TurboBoostPressure;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] PhysicsToGraphicsOffset;
    public float PhysicalSteeringWheelRange;

    public double DeltaBest;

    public double BatteryChargeFraction;

    // Electric boost motor
    public double ElectricBoostMotorTorque;
    public double ElectricBoostMotorRpm;
    public double ElectricBoostMotorTemperature;
    public double ElectricBoostWaterTemperature;
    public byte ElectricBoostMotorState;
    [MarshalAs(UnmanagedType.I1)]
    public bool LapInvalidated;
    [MarshalAs(UnmanagedType.I1)]
    public bool AbsActive;
    [MarshalAs(UnmanagedType.I1)]
    public bool TcActive;
    [MarshalAs(UnmanagedType.I1)]
    public bool SpeedLimiterActive;
    public byte WiperState;
    public byte Tc;
    public byte TcMax;
    public byte TcSlip;
    public byte TcSlipMax;
    public byte TcCut;
    public byte TcCutMax;
    public byte Abs;
    public byte AbsMax;
    public byte MotorMap;
    public byte MotorMapMax;
    public byte Migration;
    public byte MigrationMax;
    public byte FrontAntiSway;
    public byte FrontAntiSwayMax;
    public byte RearAntiSway;
    public byte RearAntiSwayMax;
    public byte LiftAndCoastProgress;
    public byte TrackLimitsSteps;
    public float Regen;
    public float SoC;
    public float VirtualEnergy;
    public float TimeGapCarAhead;
    public float TimeGapCarBehind;
    public float TimeGapPlaceAhead;
    public float TimeGapPlaceBehind;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
    public string VehicleModel;
    public LmuVehicleClass VehicleClass;
    public LmuVehicleChampionship VehicleChampionship;

    // Future use
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public byte[] Expansion;

    // Keeping this at the end of the structure, matching the source layout.
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public LmuWheelTelemetry[] Wheel;

    public override readonly string ToString()
    {
        return $"LmuVehicleTelemetry {{ Id = {this.Id}, LapNumber = {this.LapNumber}, "
             + $"ElapsedTime = {this.ElapsedTime}, Pos = {this.Pos}, Gear = {this.Gear}, "
             + $"EngineRpm = {this.EngineRpm}, UnfilteredThrottle = {this.UnfilteredThrottle}, "
             + $"UnfilteredBrake = {this.UnfilteredBrake}, Fuel = {this.Fuel}, SoC = {this.SoC}, "
             + $"VirtualEnergy = {this.VirtualEnergy}, DeltaBest = {this.DeltaBest} }}";
    }
}
