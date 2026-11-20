using System.Globalization;

namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

// Numbers as they're spoken: "minus five", "eight point eight", "two hundred and twenty", "eighty five hundred".
public static class SpokenNumbers
{
    public const string And = "and";
    public const string Hundred = "hundred";
    public const string Minus = "minus";
    public const string Point = "point";
    public const string Thousand = "thousand";

    public static readonly IReadOnlyList<string> ZeroWords = ["zero", "nought", "oh"];
    public static readonly IReadOnlyList<string> Units = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
    public static readonly IReadOnlyList<string> Teens =
        ["ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"];
    public static readonly IReadOnlyList<string> Tens = ["twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"];

    private static readonly Dictionary<string, int> WordValues = BuildWordValues();

    public static double? Parse(IReadOnlyList<string> words)
    {
        var text = ParseAsText(words);
        return text is null ? null : double.Parse(text, CultureInfo.InvariantCulture);
    }

    // Keeps the decimals exactly as spoken, so "four point oh oh" stays "4.00" rather than becoming 4.
    public static string? ParseAsText(IReadOnlyList<string> words)
    {
        var pointIndex = SpokenWords.IndexOf(words, Point);
        var wholeWords = pointIndex < 0 ? words : words.Take(pointIndex).ToList();
        var decimalWords = pointIndex < 0 ? [] : words.Skip(pointIndex + 1).ToList();

        var isNegative = wholeWords.Count > 0 && SpokenWords.IsWord(wholeWords[0], Minus);
        var whole = ParseWhole(isNegative ? wholeWords.Skip(1).ToList() : wholeWords);
        var decimals = ParseDigits(decimalWords);
        if(whole is null || decimals is null || (pointIndex >= 0 && decimals.Length == 0))
        {
            return null;
        }

        var sign = isNegative ? "-" : string.Empty;
        var fraction = decimals.Length > 0 ? $".{decimals}" : string.Empty;
        return $"{sign}{whole.Value.ToString(CultureInfo.InvariantCulture)}{fraction}";
    }

    public static string Say(int value, bool withAnd = false)
    {
        if(value < 10)
        {
            return value == 0 ? ZeroWords[0] : Units[value - 1];
        }

        if(value < 20)
        {
            return Teens[value - 10];
        }

        if(value < 100)
        {
            return value % 10 == 0 ? Tens[(value / 10) - 2] : $"{Tens[(value / 10) - 2]} {Units[(value % 10) - 1]}";
        }

        if(value < 1000)
        {
            return WithRemainder($"{Say(value / 100)} {Hundred}", value % 100, withAnd);
        }

        return WithRemainder($"{Say(value / 1000, withAnd)} {Thousand}", value % 1000, withAnd && value % 1000 < 100);
    }

    private static Dictionary<string, int> BuildWordValues()
    {
        var values = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach(var zeroWord in ZeroWords)
        {
            values[zeroWord] = 0;
        }

        for(var index = 0; index < Units.Count; index++)
        {
            values[Units[index]] = index + 1;
        }

        for(var index = 0; index < Teens.Count; index++)
        {
            values[Teens[index]] = index + 10;
        }

        for(var index = 0; index < Tens.Count; index++)
        {
            values[Tens[index]] = (index + 2) * 10;
        }

        return values;
    }

    private static string? ParseDigits(IReadOnlyList<string> words)
    {
        var digits = new List<int>();
        foreach(var word in words)
        {
            if(!WordValues.TryGetValue(word, out var value) || value > 9)
            {
                return null;
            }

            digits.Add(value);
        }

        return string.Concat(digits);
    }

    private static long? ParseWhole(IReadOnlyList<string> words)
    {
        if(words.Count == 0)
        {
            return 0;
        }

        long total = 0;
        long current = 0;
        foreach(var word in words)
        {
            if(SpokenWords.IsWord(word, And))
            {
                continue;
            }

            if(SpokenWords.IsWord(word, Hundred))
            {
                current = Math.Max(current, 1) * 100;
                continue;
            }

            if(SpokenWords.IsWord(word, Thousand))
            {
                total += Math.Max(current, 1) * 1000;
                current = 0;
                continue;
            }

            if(!WordValues.TryGetValue(word, out var value))
            {
                return null;
            }

            current += value;
        }

        return total + current;
    }

    private static string WithRemainder(string head, int remainder, bool withAnd)
    {
        if(remainder == 0)
        {
            return head;
        }

        var joiner = withAnd ? $" {And} " : " ";
        return $"{head}{joiner}{Say(remainder)}";
    }
}
