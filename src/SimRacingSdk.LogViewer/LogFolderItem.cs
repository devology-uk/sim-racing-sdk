namespace SimRacingSdk.LogViewer;

public record LogFolderItem(string Name, string Path)
{
    public List<LogFileItem> LogFiles { get; } = [];
}