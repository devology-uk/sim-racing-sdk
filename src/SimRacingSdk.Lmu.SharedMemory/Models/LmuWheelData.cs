#nullable disable

using SimRacingSdk.Lmu.SharedMemory.Messages;

namespace SimRacingSdk.Lmu.SharedMemory.Models;

public record LmuWheelData
{
    internal LmuWheelData(LmuWheelTelemetry wheel)
    {
        this.SuspensionDeflection = wheel.SuspensionDeflection;
        this.RideHeight = wheel.RideHeight;
        this.SuspForce = wheel.SuspForce;
        this.BrakeTemp = wheel.BrakeTemp;
        this.BrakePressure = wheel.BrakePressure;
        this.Rotation = wheel.Rotation;
        this.LateralPatchVel = wheel.LateralPatchVel;
        this.LongitudinalPatchVel = wheel.LongitudinalPatchVel;
        this.LateralGroundVel = wheel.LateralGroundVel;
        this.LongitudinalGroundVel = wheel.LongitudinalGroundVel;
        this.Camber = wheel.Camber;
        this.LateralForce = wheel.LateralForce;
        this.LongitudinalForce = wheel.LongitudinalForce;
        this.TireLoad = wheel.TireLoad;
        this.GripFract = wheel.GripFract;
        this.Pressure = wheel.Pressure;
        this.Temperature = wheel.Temperature;
        this.Wear = wheel.Wear;
        this.TerrainName = wheel.TerrainName;
        this.SurfaceType = wheel.SurfaceType;
        this.Flat = wheel.Flat;
        this.Detached = wheel.Detached;
        this.StaticUndeflectedRadius = wheel.StaticUndeflectedRadius;
        this.VerticalTireDeflection = wheel.VerticalTireDeflection;
        this.WheelYLocation = wheel.WheelYLocation;
        this.Toe = wheel.Toe;
        this.TireCarcassTemperature = wheel.TireCarcassTemperature;
        this.TireInnerLayerTemperature = wheel.TireInnerLayerTemperature;
        this.OptimalTemp = wheel.OptimalTemp;
        this.CompoundIndex = wheel.CompoundIndex;
        this.CompoundType = wheel.CompoundType;
    }

    public double BrakePressure { get; }
    public double BrakeTemp { get; }
    public double Camber { get; }
    public byte CompoundIndex { get; }
    public byte CompoundType { get; }
    public bool Detached { get; }
    public bool Flat { get; }
    public double GripFract { get; }
    public double LateralForce { get; }
    public double LateralGroundVel { get; }
    public double LateralPatchVel { get; }
    public double LongitudinalForce { get; }
    public double LongitudinalGroundVel { get; }
    public double LongitudinalPatchVel { get; }
    public float OptimalTemp { get; }
    public double Pressure { get; }
    public double RideHeight { get; }
    public double Rotation { get; }
    public byte StaticUndeflectedRadius { get; }
    public double SuspForce { get; }
    public double SuspensionDeflection { get; }
    public byte SurfaceType { get; }
    public string TerrainName { get; }
    public double[] Temperature { get; }
    public double TireCarcassTemperature { get; }
    public double[] TireInnerLayerTemperature { get; }
    public double TireLoad { get; }
    public double Toe { get; }
    public double VerticalTireDeflection { get; }
    public double Wear { get; }
    public double WheelYLocation { get; }
}
