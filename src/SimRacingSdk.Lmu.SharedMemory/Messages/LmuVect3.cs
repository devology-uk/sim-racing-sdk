#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches TelemVect3 (InternalsPlugin.hpp) - a union of {x,y,z} and a double[3], which is the same 24-byte layout
// either way.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct LmuVect3
{
    public double X;
    public double Y;
    public double Z;

    public override readonly string ToString()
    {
        return $"LmuVect3 {{ X = {this.X}, Y = {this.Y}, Z = {this.Z} }}";
    }
}
