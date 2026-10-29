#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

// Each aid: -1 not available, 0 off, 1 on (ESP 1-3 low to high), 5 currently active.
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eAidSettings
{
    public int Abs;
    public int Tc;
    public int Esp;
    public int Countersteer;
    public int Cornering;
}
