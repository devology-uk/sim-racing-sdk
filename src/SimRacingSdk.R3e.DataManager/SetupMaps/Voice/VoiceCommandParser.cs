using System.Globalization;
using SimRacingSdk.R3e.DataManager.SetupMaps.Voice.Commands;
using SimRacingSdk.R3e.Setups.Models;

namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

// Turns the words of a phrase the grammar allowed into a command; null when they don't make one.
public static class VoiceCommandParser
{
    public static VoiceCommand? Parse(HeardPhrase heard)
    {
        var words = heard.Words;
        var fixedCommand = FixedVoiceCommands.For(string.Join(' ', words));
        if(fixedCommand is not null)
        {
            return fixedCommand;
        }

        if(words.Count > 0 && SpokenWords.IsWord(words[0], VoiceWords.Car))
        {
            return heard.CarName is null ? null : new SelectCarCommand(heard.CarName);
        }

        var match = VoiceTargets.MatchStartOf(words);
        return match is null ? null : ParseAction(match.Value.Target, words.Skip(match.Value.WordCount).ToList());
    }

    private static bool IsCompound(string word)
    {
        return VoiceWords.Compounds.Contains(word, StringComparer.OrdinalIgnoreCase);
    }

    private static VoiceCommand? ParseAction(VoiceTarget target, IReadOnlyList<string> words)
    {
        if(words.Count == 0)
        {
            return new ReadTargetCommand(target);
        }

        var phrase = string.Join(' ', words);
        var availability = ParseAvailability(phrase);
        if(availability is not null)
        {
            return new SetAvailabilityCommand(target, availability.Value);
        }

        if(SpokenWords.IsWord(phrase, VoiceWords.Clear))
        {
            return new ClearOptionsCommand(target);
        }

        if(SpokenWords.IsWord(words[0], VoiceWords.Options))
        {
            return ParseOptions(target, words.Skip(1).ToList());
        }

        if(SpokenWords.IsWord(words[0], VoiceWords.Add))
        {
            return ParseAddition(target, words.Skip(1).ToList());
        }

        return SpokenWords.IndexOf(words, VoiceWords.To) >= 0 ? ParseRange(target, words) : ParseGearCount(target, words);
    }

    private static VoiceCommand? ParseAddition(VoiceTarget target, IReadOnlyList<string> words)
    {
        if(words.Count == 1 && IsCompound(words[0]))
        {
            return new AddOptionCommand(target, TitleCase(words[0]));
        }

        var toIndex = SpokenWords.IndexOf(words, VoiceWords.To);
        if(toIndex >= 0)
        {
            var driving = ParseWholeNumber(words.Take(toIndex).ToList());
            var driven = ParseWholeNumber(words.Skip(toIndex + 1).ToList());
            return driving is > 0 && driven is > 0 ? new AddGearPairCommand(target, driving.Value, driven.Value) : null;
        }

        var option = SpokenNumbers.ParseAsText(words);
        return option is null ? null : new AddOptionCommand(target, option);
    }

    private static R3eSetupControlAvailability? ParseAvailability(string phrase)
    {
        if(SpokenWords.IsWord(phrase, VoiceWords.Fixed))
        {
            return R3eSetupControlAvailability.Fixed;
        }

        if(SpokenWords.IsWord(phrase, VoiceWords.NotFitted))
        {
            return R3eSetupControlAvailability.NotFitted;
        }

        return SpokenWords.IsWord(phrase, VoiceWords.Adjustable) ? R3eSetupControlAvailability.Adjustable : null;
    }

    private static VoiceCommand? ParseGearCount(VoiceTarget target, IReadOnlyList<string> words)
    {
        var gearCount = ParseWholeNumber(words);
        return gearCount is > 0 ? new SetGearCountCommand(target, gearCount.Value) : null;
    }

    private static VoiceCommand? ParseOptions(VoiceTarget target, IReadOnlyList<string> words)
    {
        return words.Count > 0 && words.All(IsCompound) ? new SetOptionsCommand(target, words.Select(TitleCase).ToList()) : null;
    }

    private static VoiceCommand? ParseRange(VoiceTarget target, IReadOnlyList<string> words)
    {
        var toIndex = SpokenWords.IndexOf(words, VoiceWords.To);
        var afterTo = words.Skip(toIndex + 1).ToList();
        var stepIndex = SpokenWords.IndexOf(afterTo, VoiceWords.Step);

        var min = SpokenNumbers.Parse(words.Take(toIndex).ToList());
        var max = SpokenNumbers.Parse(stepIndex < 0 ? afterTo : afterTo.Take(stepIndex).ToList());
        var step = stepIndex < 0 ? null : SpokenNumbers.Parse(afterTo.Skip(stepIndex + 1).ToList());
        if(min is null || max is null || (stepIndex >= 0 && step is not > 0))
        {
            return null;
        }

        return new SetRangeCommand(target, min.Value, max.Value, step);
    }

    private static int? ParseWholeNumber(IReadOnlyList<string> words)
    {
        var value = words.Count == 0 ? null : SpokenNumbers.Parse(words);
        return value is not null && value == Math.Floor(value.Value) ? (int)value.Value : null;
    }

    private static string TitleCase(string word)
    {
        return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(word.ToLowerInvariant());
    }
}
