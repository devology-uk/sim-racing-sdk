#nullable disable

namespace SimRacingSdk.Acc.Setups.Models;

public class AccSetupAlignment
{
    public int[] Camber { get; set; } = new int[4];
    public int[] Toe { get; set; } = new int[4];
    public double[] StaticCamber { get; set; } = new double[4];
    public double[] ToeOutLinear { get; set; } = new double[4];
    public int CasterLf { get; set; }
    public int CasterRf { get; set; }
    public int SteerRatio { get; set; }
}
