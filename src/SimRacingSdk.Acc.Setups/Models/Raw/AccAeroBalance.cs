#nullable disable

namespace SimRacingSdk.Acc.Setups.Models;

public class AccAeroBalance
{
    public int[] RideHeight { get; set; } = new int[4];
    public double[] RodLength { get; set; } = new double[4];
    public int Splitter { get; set; }
    public int RearWing { get; set; }
    public int[] BrakeDuct { get; set; } = new int[2];
}
