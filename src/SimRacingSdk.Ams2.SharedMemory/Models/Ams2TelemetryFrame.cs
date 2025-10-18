#nullable disable

using System.Numerics;
using SimRacingSdk.Ams2.SharedMemory.Enums;

namespace SimRacingSdk.Ams2.SharedMemory.Models;

public record Ams2TelemetryFrame
{
    public int AbsSetting { get; set; }
    public float AeroDamage { get; init; }
    public Ams2WheelMetric<float> AirPressure { get; init; }
    public Vector3 AngularVelocity { get; init; }
    public float Brake { get; init; }
    public float BrakeBias { get; init; }
    public Ams2WheelMetric<float> BrakeDamage { get; init; }
    public Ams2WheelMetric<float> BrakeTempC { get; init; }
    public Ams2CarFlags CarFlags { get; init; }
    public string Class { get; set; }
    public float Clutch { get; init; }
    public float ClutchTempK { get; init; }
    public float ClutchWear { get; init; }
    public Ams2CrashDamageState CrashDamageState { get; init; }
    public float EngineDamage { get; init; }
    public float EngineSpeed { get; init; }
    public float EngineTorque { get; init; }
    public Ams2ErsDeploymentMode ErsDeploymentMode { get; init; }
    public Vector3 ExtentsCenter { get; init; }
    public float FrontWing { get; init; }
    public float FuelCapacity { get; init; }
    public float FuelLevel { get; init; }
    public float FuelPressureKpa { get; init; }
    public int Gear { get; init; }
    public int GearCount { get; init; }
    public float HandBrake { get; init; }
    public bool IsAbsActive { get; init; }
    public bool IsBoostActive { get; init; }
    public bool IsClutchOverheated { get; init; }
    public bool IsClutchSlipping { get; init; }
    public bool IsErsAutoModeEnabled { get; init; }
    public Ams2LaunchStage LaunchStage { get; init; }
    public Vector3 LocalAcceleration { get; init; }
    public Vector3 LocalVelocity { get; init; }
    public float MaxRpm { get; init; }
    public string Name { get; set; }
    public float OdometerKm { get; init; }
    public float OilPressureKpa { get; init; }
    public float OilTempC { get; init; }
    public Vector3 Orientation { get; init; }
    public float RearWing { get; init; }
    public Ams2WheelMetric<float> RideHeight { get; init; }
    public float Rpm { get; init; }
    public float Speed { get; init; }
    public float Steering { get; init; }
    public int TCSetting { get; init; }
    public float Throttle { get; init; }
    public Ams2WheelMetric<float> TyreCarcassTempK { get; init; }
    public Ams2WheelMetric<string> TyreCompound { get; init; }
    public Ams2WheelMetric<Ams2TyreFlag> TyreFlags { get; init; }
    public Ams2WheelMetric<float> TyreGrip { get; init; }
    public Ams2WheelMetric<float> TyreHeightAboveGround { get; init; }
    public Ams2WheelMetric<float> TyreInternalAirTempK { get; init; }
    public Ams2WheelMetric<float> TyreLayerTempK { get; init; }
    public Ams2WheelMetric<float> TyreRimTempK { get; init; }
    public Ams2WheelMetric<float> TyreRps { get; init; }
    public Ams2WheelMetric<float> TyreSlipSpeed { get; init; }
    public Ams2WheelMetric<float> TyreTempC { get; init; }
    public Ams2WheelMetric<float> TyreTempCenterC { get; init; }
    public Ams2WheelMetric<float> TyreTempLeftC { get; init; }
    public Ams2WheelMetric<float> TyreTempRightC { get; init; }
    public Ams2WheelMetric<float> TyreTreadTempK { get; init; }
    public Ams2WheelMetric<float> TyreWear { get; init; }
    public float UnfilteredBrake { get; init; }
    public float UnfilteredClutch { get; init; }
    public float UnfilteredSteering { get; init; }
    public float UnfilteredThrottle { get; init; }
    public float WaterPressureKpa { get; init; }
    public float WaterTempC { get; init; }
    public Ams2WheelMetric<float> WheelLocalPositionY { get; init; }
    public Vector3 WorldAcceleration { get; init; }
    public Vector3 WorldVelocity { get; init; }
}