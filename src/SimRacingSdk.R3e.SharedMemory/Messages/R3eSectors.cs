#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eSectors
{
    public float Sector1;
    public float Sector2;
    public float Sector3;
}
