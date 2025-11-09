#nullable disable

namespace SimRacingSdk.LogViewer;

public record LogFileEntry(string TimeStamp, string Level, string Content, string ContentType = "Text")
{
}