namespace SimRacingSdk.Ams2.Demo.LogViewer;

public record LogFolderItem(string Name, string Path)
{
    public List<LogFileItem> LogFiles { get; } = [];
}