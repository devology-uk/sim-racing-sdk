#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eCarDamage
{
    public float Engine;
    public float Transmission;
    public float Aerodynamics;
    public float Suspension;
    public float Unused1;
    public float Unused2;
}
