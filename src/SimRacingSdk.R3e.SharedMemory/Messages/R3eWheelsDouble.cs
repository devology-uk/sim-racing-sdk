#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eWheelsDouble
{
    public double FrontLeft;
    public double FrontRight;
    public double RearLeft;
    public double RearRight;
}
