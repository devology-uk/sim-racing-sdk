namespace SimRacingSdk.R3e.DataManager.SetupMaps.Voice;

// The recogniser's casing follows the grammar text ("A B S"), so words are always compared ignoring case.
public static class SpokenWords
{
    public static int IndexOf(IReadOnlyList<string> words, string word)
    {
        for(var index = 0; index < words.Count; index++)
        {
            if(IsWord(words[index], word))
            {
                return index;
            }
        }

        return -1;
    }

    public static bool IsWord(string spoken, string word)
    {
        return string.Equals(spoken, word, StringComparison.OrdinalIgnoreCase);
    }
}
