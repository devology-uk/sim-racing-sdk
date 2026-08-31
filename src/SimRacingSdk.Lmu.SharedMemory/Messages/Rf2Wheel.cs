#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches rF2Wheel (rF2State.h) / TelemWheelV01 exactly - field order and types are load-bearing for marshalling.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct Rf2Wheel
{
    public double SuspensionDeflection;
    public double RideHeight;
    public double SuspForce;
    public double BrakeTemp;
    public double BrakePressure;

    public double Rotation;
    public double LateralPatchVel;
    public double LongitudinalPatchVel;
    public double LateralGroundVel;
    public double LongitudinalGroundVel;
    public double Camber;
    public double LateralForce;
    public double LongitudinalForce;
    public double TireLoad;

    public double GripFract;
    public double Pressure;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public double[] Temperature;
    public double Wear;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string TerrainName;
    public byte SurfaceType;
    [MarshalAs(UnmanagedType.I1)]
    public bool Flat;
    [MarshalAs(UnmanagedType.I1)]
    public bool Detached;
    public byte StaticUndeflectedRadius;

    public double VerticalTireDeflection;
    public double WheelYLocation;
    public double Toe;

    public double TireCarcassTemperature;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public double[] TireInnerLayerTemperature;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
    public byte[] Expansion;

    public override readonly string ToString()
    {
        return $"Rf2Wheel {{ Pressure = {this.Pressure}, Wear = {this.Wear}, "
             + $"BrakeTemp = {this.BrakeTemp}, TireLoad = {this.TireLoad}, Flat = {this.Flat}, "
             + $"Detached = {this.Detached}, SurfaceType = {this.SurfaceType} }}";
    }
}
