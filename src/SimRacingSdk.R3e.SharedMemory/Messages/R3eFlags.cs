#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.R3e.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct R3eFlags
{
    public int Yellow;
    public int YellowCausedIt;
    public int YellowOvertake;
    public int YellowPositionsGained;
    public R3eSectorFlags SectorYellow;
    public float ClosestYellowDistanceIntoTrack;
    public int Blue;
    public int Black;
    public int Green;
    public int Checkered;
    public int White;
    public int BlackAndWhite;
}
