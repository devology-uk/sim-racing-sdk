#nullable disable

namespace SimRacingSdk.Acc.Setups.Models;

public class IndexedIntegerList
{
    public bool IsFixed { get; set; }
    public bool IsSupported { get; set; }
    public List<int> Values { get; set; } = [];

    public int ValueAt(int index)
    {
        return IsSupported && index >= 0 && index < Values.Count ? Values[index] : 0;
    }
}
