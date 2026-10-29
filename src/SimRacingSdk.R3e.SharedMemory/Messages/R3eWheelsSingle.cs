#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eWheelsSingle
{
    public float FrontLeft;
    public float FrontRight;
    public float RearLeft;
    public float RearRight;
}
