#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches rF2VehicleTelemetry (rF2State.h) / TelemInfoV01 exactly - field order and types are load-bearing for
// marshalling. One entry per car in Rf2TelemetryBuffer.Vehicles; keyed by Id, which is cross-referenced against
// Rf2VehicleScoring.Id (same slot ID space) to attribute a frame to the right car/lap.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct Rf2VehicleTelemetry
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
    public Rf2Vec3 Pos;
    public Rf2Vec3 LocalVel;
    public Rf2Vec3 LocalAccel;

    // Orientation and derivatives
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public Rf2Vec3[] Ori;
    public Rf2Vec3 LocalRot;
    public Rf2Vec3 LocalRotAccel;

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
    public Rf2Vec3 LastImpactPos;

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

    public double BatteryChargeFraction;

    // Electric boost motor
    public double ElectricBoostMotorTorque;
    public double ElectricBoostMotorRpm;
    public double ElectricBoostMotorTemperature;
    public double ElectricBoostWaterTemperature;
    public byte ElectricBoostMotorState;

    // Future use
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 111)]
    public byte[] Expansion;

    // Keeping this at the end of the structure, matching the source layout.
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public Rf2Wheel[] Wheels;

    public override readonly string ToString()
    {
        return $"Rf2VehicleTelemetry {{ Id = {this.Id}, LapNumber = {this.LapNumber}, "
             + $"ElapsedTime = {this.ElapsedTime}, Pos = {this.Pos}, Gear = {this.Gear}, "
             + $"EngineRpm = {this.EngineRpm}, UnfilteredThrottle = {this.UnfilteredThrottle}, "
             + $"UnfilteredBrake = {this.UnfilteredBrake}, UnfilteredSteering = {this.UnfilteredSteering}, "
             + $"Fuel = {this.Fuel}, CurrentSector = {this.CurrentSector} }}";
    }
}
