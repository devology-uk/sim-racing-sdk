#nullable disable

namespace SimRacingSdk.Acc.Setups.Models;

public class AccMechanicalBalance
{
    public int ArbFront { get; set; }
    public int ArbRear { get; set; }
    public int[] WheelRate { get; set; } = new int[4];
    public int[] BumpStopRateUp { get; set; } = new int[4];
    public int[] BumpStopRateDn { get; set; } = new int[4];
    public int[] BumpStopWindow { get; set; } = new int[4];
    public int BrakeTorque { get; set; }
    public int BrakeBias { get; set; }
}
