using System.Speech.Recognition;

namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

// Every call builds a fresh builder, because one builder appended in two places can confuse the recogniser.
public static class SpokenNumberGrammar
{
    private const int MostDecimalDigits = 3;

    public static GrammarBuilder Number()
    {
        var number = new GrammarBuilder();
        number.Append(SpokenNumbers.Minus, 0, 1);
        number.Append(WholeNumber());
        number.Append(Decimals(), 0, 1);
        return number;
    }

    public static GrammarBuilder WholeNumber()
    {
        return new Choices(
            new Choices(SpokenNumbers.ZeroWords.ToArray()),
            UnderThousand(),
            Thousands()).ToGrammarBuilder();
    }

    private static GrammarBuilder Decimals()
    {
        var decimals = new GrammarBuilder(SpokenNumbers.Point);
        decimals.Append(new Choices(SpokenNumbers.ZeroWords.Concat(SpokenNumbers.Units).ToArray()), 1, MostDecimalDigits);
        return decimals;
    }

    private static GrammarBuilder Hundreds()
    {
        var hundreds = OneToNinetyNine();
        hundreds.Append(SpokenNumbers.Hundred);
        hundreds.Append(AndOneToNinetyNine(), 0, 1);
        return hundreds;
    }

    private static GrammarBuilder AndOneToNinetyNine()
    {
        var tail = new GrammarBuilder();
        tail.Append(SpokenNumbers.And, 0, 1);
        tail.Append(OneToNinetyNine());
        return tail;
    }

    private static GrammarBuilder OneToNinetyNine()
    {
        var tens = new GrammarBuilder(new Choices(SpokenNumbers.Tens.ToArray()));
        tens.Append(new Choices(SpokenNumbers.Units.ToArray()), 0, 1);

        return new Choices(
            new Choices(SpokenNumbers.Units.ToArray()),
            new Choices(SpokenNumbers.Teens.ToArray()),
            tens).ToGrammarBuilder();
    }

    private static GrammarBuilder Thousands()
    {
        var thousands = OneToNinetyNine();
        thousands.Append(SpokenNumbers.Thousand);

        var tail = new GrammarBuilder();
        tail.Append(SpokenNumbers.And, 0, 1);
        tail.Append(UnderThousand());
        thousands.Append(tail, 0, 1);
        return thousands;
    }

    private static GrammarBuilder UnderThousand()
    {
        return new Choices(OneToNinetyNine(), Hundreds()).ToGrammarBuilder();
    }
}
