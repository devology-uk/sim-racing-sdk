#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3ePushToPass
{
    public int Available;
    public int Engaged;
    public int AmountLeft;
    public float EngagedTimeLeft;
    public float WaitTimeLeft;
}
