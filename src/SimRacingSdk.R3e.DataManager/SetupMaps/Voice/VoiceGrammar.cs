using System.Globalization;
using System.Speech.Recognition;

namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

// Only these phrases can be heard, which is what makes numbers and control names reliable.
public static class VoiceGrammar
{
    private const int MostOptionsAtOnce = 6;

    public static Grammar Build(CultureInfo culture, IEnumerable<string> carNames)
    {
        var commands = new Choices();
        commands.Add(FixedVoiceCommands.Phrases.ToArray());
        commands.Add(CarCommand(carNames));
        commands.Add(TargetCommand());

        var root = new GrammarBuilder(commands) { Culture = culture };
        return new Grammar(root) { Name = "Setup map voice entry" };
    }

    private static GrammarBuilder Actions()
    {
        return new Choices(
            new GrammarBuilder(VoiceWords.Fixed),
            new GrammarBuilder(VoiceWords.NotFitted),
            new GrammarBuilder(VoiceWords.Adjustable),
            new GrammarBuilder(VoiceWords.Clear),
            Range(),
            Options(),
            Addition(),
            SpokenNumberGrammar.WholeNumber()).ToGrammarBuilder();
    }

    private static GrammarBuilder Addition()
    {
        var addition = new GrammarBuilder(VoiceWords.Add);
        addition.Append(new Choices(SpokenNumberGrammar.Number(), Compounds(), GearPair()));
        return addition;
    }

    private static GrammarBuilder CarCommand(IEnumerable<string> carNames)
    {
        var cars = carNames
            .Distinct()
            .Select(carName => new SemanticResultValue(SpokenCarNames.For(carName), carName).ToGrammarBuilder())
            .ToArray();

        var command = new GrammarBuilder(VoiceWords.Car);
        command.Append(new SemanticResultKey(VoiceWords.CarKey, new Choices(cars)));
        return command;
    }

    private static GrammarBuilder Compounds()
    {
        return new Choices(VoiceWords.Compounds.ToArray()).ToGrammarBuilder();
    }

    private static GrammarBuilder GearPair()
    {
        var pair = SpokenNumberGrammar.WholeNumber();
        pair.Append(VoiceWords.To);
        pair.Append(SpokenNumberGrammar.WholeNumber());
        return pair;
    }

    private static GrammarBuilder Options()
    {
        var options = new GrammarBuilder(VoiceWords.Options);
        options.Append(Compounds(), 1, MostOptionsAtOnce);
        return options;
    }

    private static GrammarBuilder Range()
    {
        var range = SpokenNumberGrammar.Number();
        range.Append(VoiceWords.To);
        range.Append(SpokenNumberGrammar.Number());

        var step = new GrammarBuilder(VoiceWords.Step);
        step.Append(SpokenNumberGrammar.Number());
        range.Append(step, 0, 1);
        return range;
    }

    private static GrammarBuilder TargetCommand()
    {
        var command = new GrammarBuilder(new Choices(VoiceTargets.AllPhrases.ToArray()));
        command.Append(Actions(), 0, 1);
        return command;
    }
}
