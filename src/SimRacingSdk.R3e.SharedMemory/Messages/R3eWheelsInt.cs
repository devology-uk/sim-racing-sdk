#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eWheelsInt
{
    public int FrontLeft;
    public int FrontRight;
    public int RearLeft;
    public int RearRight;
}
