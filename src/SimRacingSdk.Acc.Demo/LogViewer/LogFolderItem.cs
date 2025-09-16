namespace SimRacingSdk.Acc.Demo.LogViewer;

public record LogFolderItem(string Name, string Path)
{
    public List<LogFileItem> LogFiles { get; } = [];
}