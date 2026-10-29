#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eWheelsBrakeTemperature
{
    public R3eBrakeTemperature FrontLeft;
    public R3eBrakeTemperature FrontRight;
    public R3eBrakeTemperature RearLeft;
    public R3eBrakeTemperature RearRight;
}
