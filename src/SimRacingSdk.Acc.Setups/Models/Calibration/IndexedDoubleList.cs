#nullable disable

namespace SimRacingSdk.Acc.Setups.Models;

public class IndexedDoubleList
{
    public bool IsFixed { get; set; }
    public bool IsSupported { get; set; }
    public List<double> Values { get; set; } = [];

    public double ValueAt(int index)
    {
        return IsSupported && index >= 0 && index < Values.Count ? Values[index] : 0;
    }
}
