#nullable disable

namespace SimRacingSdk.Acc.Setups.Models;

public class IndexedDoubleRange
{
    public bool IsFixed { get; set; }
    public bool IsSupported { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
    public double Step { get; set; }

    public double ValueAt(int index)
    {
        return IsSupported ? Math.Round(Min + (index * Step), 2) : 0;
    }
}
