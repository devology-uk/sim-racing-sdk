#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eVector3Single
{
    public float X;
    public float Y;
    public float Z;
}
