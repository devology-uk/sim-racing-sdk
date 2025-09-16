#nullable disable

namespace SimRacingSdk.Acc.Demo.LogViewer;

public record LogFileEntry(string TimeStamp, string Level, string Content, string ContentType = "Text")
{
}