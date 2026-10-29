#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eWheelsTyreTemperature
{
    public R3eTyreTemperatureInformation FrontLeft;
    public R3eTyreTemperatureInformation FrontRight;
    public R3eTyreTemperatureInformation RearLeft;
    public R3eTyreTemperatureInformation RearRight;
}
