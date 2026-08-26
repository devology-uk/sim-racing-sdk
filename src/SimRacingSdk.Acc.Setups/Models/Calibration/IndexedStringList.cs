#nullable disable

namespace SimRacingSdk.Acc.Setups.Models;

public class IndexedStringList
{
    public List<string> Values { get; set; } = [];

    public string ValueAt(int index)
    {
        return index >= 0 && index < Values.Count ? Values[index] : string.Empty;
    }
}
