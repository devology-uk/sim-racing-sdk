using System.IO;

namespace SimRacingSdk.DataManager.Shared.Generation;

public static class GeneratedSource
{
    // A generator's template text carries whatever line endings its own source file was checked out
    // with, while joined entry lines use Environment.NewLine - normalised here so the file is consistent.
    public static void Write(string path, string source)
    {
        File.WriteAllText(path, source.ReplaceLineEndings());
    }
}
