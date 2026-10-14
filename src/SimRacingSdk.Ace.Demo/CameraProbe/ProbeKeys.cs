using System.Windows.Input;

namespace SimRacingSdk.Ace.Demo.CameraProbe;

public static class ProbeKeys
{
    public static IReadOnlyList<Key> All { get; } = CreateKeyList();

    private static List<Key> CreateKeyList()
    {
        var keys = new List<Key>();
        keys.AddRange(Range(Key.F1, Key.F12));
        keys.AddRange(Range(Key.A, Key.Z));
        keys.AddRange(Range(Key.D0, Key.D9));
        keys.AddRange(Range(Key.NumPad0, Key.NumPad9));
        keys.AddRange([
            Key.Left, Key.Right, Key.Up, Key.Down,
            Key.PageUp, Key.PageDown, Key.Home, Key.End, Key.Insert, Key.Delete,
            Key.Space, Key.Tab, Key.Enter, Key.Escape, Key.Back,
            Key.Add, Key.Subtract, Key.Multiply, Key.Divide, Key.Decimal,
            Key.OemComma, Key.OemPeriod, Key.OemMinus, Key.OemPlus,
            Key.OemOpenBrackets, Key.OemCloseBrackets, Key.OemSemicolon, Key.OemQuotes, Key.OemQuestion
        ]);
        return keys;
    }

    private static IEnumerable<Key> Range(Key first, Key last)
    {
        for(var key = first; key <= last; key++)
        {
            yield return key;
        }
    }
}
