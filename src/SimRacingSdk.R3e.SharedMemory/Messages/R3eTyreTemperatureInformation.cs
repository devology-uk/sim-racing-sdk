#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eTyreTemperatureInformation
{
    public R3eTyreTemperature CurrentTemp;
    public float OptimalTemp;
    public float ColdTemp;
    public float HotTemp;
}
