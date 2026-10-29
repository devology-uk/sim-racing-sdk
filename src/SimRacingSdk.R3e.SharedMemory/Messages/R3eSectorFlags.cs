#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eSectorFlags
{
    public int Sector1;
    public int Sector2;
    public int Sector3;
}
