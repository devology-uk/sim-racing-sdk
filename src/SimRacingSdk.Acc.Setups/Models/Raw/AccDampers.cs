#nullable disable

namespace SimRacingSdk.Acc.Setups.Models;

public class AccDampers
{
    public int[] BumpSlow { get; set; } = new int[4];
    public int[] BumpFast { get; set; } = new int[4];
    public int[] ReboundSlow { get; set; } = new int[4];
    public int[] ReboundFast { get; set; } = new int[4];
}
