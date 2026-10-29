#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eDrs
{
    public int Equipped;
    public int Available;
    public int NumActivationsLeft;
    public int Engaged;
}
