using System.Globalization;
using System.Speech.Recognition;
using System.Text;
using System.Text.RegularExpressions;

namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

// Car names are full of model codes ("BMW M4 GT3 EVO DTM", "Porsche 911 GT3 R (992.2)"), so each part of a name
// is offered the ways people say it: "B M W", "nine eleven" or "nine one one", "nineteen ninety five".
public static class SpokenCarNames
{
    private const int LongestSpelledWord = 4;

    private static readonly Regex DecimalPoint = new(@"(\d)\.(\d)");
    private static readonly Regex NotLetterOrDigit = new(@"[^A-Za-z0-9]+");
    private static readonly Regex LetterOrDigitRun = new(@"[A-Za-z]+|\d+");
    private static readonly Regex Vowel = new("[AEIOU]");

    public static GrammarBuilder For(string carName)
    {
        var builder = new GrammarBuilder();
        foreach(var alternatives in WordAlternatives(carName))
        {
            builder.Append(new Choices(alternatives.ToArray()));
        }

        return builder;
    }

    public static IReadOnlyList<IReadOnlyList<string>> WordAlternatives(string carName)
    {
        var spokenName = DecimalPoint.Replace(WithoutAccents(carName), $"$1 {SpokenNumbers.Point} $2")
            .Replace("&", $" {SpokenNumbers.And} ")
            .Replace("+", " plus ");

        return NotLetterOrDigit.Split(spokenName)
            .SelectMany(word => LetterOrDigitRun.Matches(word).Select(run => run.Value))
            .Select(Alternatives)
            .ToList();
    }

    private static IReadOnlyList<string> Alternatives(string run)
    {
        return char.IsDigit(run[0]) ? DigitAlternatives(run) : LetterAlternatives(run);
    }

    private static IReadOnlyList<string> DigitAlternatives(string digits)
    {
        var value = int.Parse(digits, CultureInfo.InvariantCulture);
        var alternatives = new List<string>();

        if(digits.Length > 1 && digits[0] == '0')
        {
            alternatives.Add($"{SpokenNumbers.ZeroWords[2]} {SpokenNumbers.Say(value)}");
        }

        alternatives.Add(SpokenNumbers.Say(value));
        alternatives.Add(SpokenNumbers.Say(value, true));

        if(digits.Length >= 3)
        {
            alternatives.Add(string.Join(' ', digits.Select(digit => SpokenNumbers.Say(digit - '0'))));
            alternatives.Add(InPairs(digits));
        }

        return alternatives.Distinct().ToList();
    }

    // "720" as "seven twenty", "1995" as "nineteen ninety five", "2002" as "twenty oh two".
    private static string InPairs(string digits)
    {
        var headLength = digits.Length % 2 == 1 ? 1 : 2;
        var parts = new List<string> { SpokenNumbers.Say(int.Parse(digits[..headLength], CultureInfo.InvariantCulture)) };

        for(var index = headLength; index < digits.Length; index += 2)
        {
            parts.Add(SayPair(digits.Substring(index, 2)));
        }

        return string.Join(' ', parts);
    }

    private static string SayPair(string pair)
    {
        if(pair == "00")
        {
            return SpokenNumbers.Hundred;
        }

        var value = int.Parse(pair, CultureInfo.InvariantCulture);
        return pair[0] == '0' ? $"{SpokenNumbers.ZeroWords[2]} {SpokenNumbers.Say(value)}" : SpokenNumbers.Say(value);
    }

    private static IReadOnlyList<string> LetterAlternatives(string letters)
    {
        var isCode = letters.Length == 1 || (letters.Length <= LongestSpelledWord && letters.All(char.IsUpper));
        if(!isCode)
        {
            return [letters];
        }

        var spelled = string.Join(' ', letters.ToUpperInvariant().ToCharArray());
        return letters.Length > 2 && Vowel.IsMatch(letters) ? [spelled, letters.ToLowerInvariant()] : [spelled];
    }

    private static string WithoutAccents(string text)
    {
        var withoutMarks = text.Normalize(NormalizationForm.FormD)
            .Where(character => CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark);
        return string.Concat(withoutMarks).Normalize(NormalizationForm.FormC);
    }
}
