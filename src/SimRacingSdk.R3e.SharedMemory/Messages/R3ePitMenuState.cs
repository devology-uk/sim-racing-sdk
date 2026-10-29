#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3ePitMenuState
{
    public int Preset;
    public int Penalty;
    public int DriverChange;
    public int Fuel;
    public int FrontTyres;
    public int RearTyres;
    public int Body;
    public int FrontWing;
    public int RearWing;
    public int Suspension;
    public int ButtonTop;
    public int ButtonBottom;
}
