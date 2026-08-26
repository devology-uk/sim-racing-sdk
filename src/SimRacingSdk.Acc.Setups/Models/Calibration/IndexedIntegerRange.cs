#nullable disable

namespace SimRacingSdk.Acc.Setups.Models;

public class IndexedIntegerRange
{
    public bool IsFixed { get; set; }
    public bool IsSupported { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
    public int Step { get; set; }

    public int ValueAt(int index)
    {
        return IsSupported ? Min + (index * Step) : 0;
    }
}
