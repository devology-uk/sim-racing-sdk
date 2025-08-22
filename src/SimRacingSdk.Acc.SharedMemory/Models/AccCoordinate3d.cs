namespace SimRacingSdk.Acc.SharedMemory.Models;

public struct AccCoordinate3d
{
    public float X;
    public float Y;
    public float Z;

    public override string ToString() => $"X: {this.X}, Y: {this.Y}, Z: {this.Z}";
}