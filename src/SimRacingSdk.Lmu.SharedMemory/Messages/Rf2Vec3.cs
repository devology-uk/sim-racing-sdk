#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Rf2Vec3
{
    public double X;
    public double Y;
    public double Z;

    public override readonly string ToString()
    {
        return $"Rf2Vec3 {{ X = {this.X}, Y = {this.Y}, Z = {this.Z} }}";
    }
}
