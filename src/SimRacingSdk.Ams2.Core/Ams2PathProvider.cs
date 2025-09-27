using SimRacingSdk.Ams2.Core.Abstractions;

namespace SimRacingSdk.Ams2.Core;

public class Ams2PathProvider : IAms2PathProvider
{
    private const string DocumentsFolderName = "Automobilista 2";
    private static Ams2PathProvider? singletonInstance;

    public Ams2PathProvider()
    {
        var myDocumentsFolderPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                DocumentsFolderName);
        this.DocumentsFolderPath = myDocumentsFolderPath;
    }

    public string DocumentsFolderPath { get; }

    public static Ams2PathProvider Instance => singletonInstance ??= new Ams2PathProvider();
}