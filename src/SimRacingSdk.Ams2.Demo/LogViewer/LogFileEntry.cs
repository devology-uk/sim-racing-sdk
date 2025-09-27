#nullable disable

namespace SimRacingSdk.Ams2.Demo.LogViewer;

public record LogFileEntry(string TimeStamp, string Level, string Content, string ContentType = "Text")
{
}