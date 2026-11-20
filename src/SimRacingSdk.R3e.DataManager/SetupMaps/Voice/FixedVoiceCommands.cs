using SimRacingSdk.R3e.DataManager.SetupMaps.Voice.Commands;

namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

// Whole phrases that are a command on their own.
public static class FixedVoiceCommands
{
    private static readonly Dictionary<string, Func<VoiceCommand>> CommandsByPhrase = new(StringComparer.OrdinalIgnoreCase)
    {
        ["next car"] = () => new NextCarCommand(),
        ["previous car"] = () => new PreviousCarCommand(),
        ["which car"] = () => new WhichCarCommand(),
        ["copy last car"] = () => new CopyPreviousCarCommand(),
        ["save"] = () => new SaveMapCommand(),
        ["save map"] = () => new SaveMapCommand(),
        ["validated"] = () => new SetValidatedCommand(true),
        ["not validated"] = () => new SetValidatedCommand(false),
        ["undo"] = () => new UndoCommand(),
        ["pause listening"] = () => new PauseListeningCommand(),
        ["resume listening"] = () => new ResumeListeningCommand(),
        ["help"] = () => new HelpCommand()
    };

    public static IEnumerable<string> Phrases => CommandsByPhrase.Keys;

    public static VoiceCommand? For(string phrase)
    {
        return CommandsByPhrase.TryGetValue(phrase, out var create) ? create() : null;
    }
}
