#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

// Left, Center and Right as the game names them; whether Left is the car's left or the tyre's inside isn't
// confirmed yet.
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eTyreTemperature
{
    public float Left;
    public float Center;
    public float Right;
}
