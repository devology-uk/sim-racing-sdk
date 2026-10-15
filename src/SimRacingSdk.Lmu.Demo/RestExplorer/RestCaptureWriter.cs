using System.IO;
using SimRacingSdk.Lmu.Rest.Models;

namespace SimRacingSdk.Lmu.Demo.RestExplorer;

// One folder per explorer session, one sub-folder per game state, one file per response, numbered in call order,
// plus an index.txt summarising every call so the captures can be brought back and read in order.
public class RestCaptureWriter
{
    private static readonly char[] UnsafeFileNameCharacters = [.. Path.GetInvalidFileNameChars(), '/', '\\', '?', '*'];

    private int sequence;

    public RestCaptureWriter(string rootFolderPath)
    {
        this.CaptureFolderPath = Path.Combine(rootFolderPath, $"{DateTime.Now:yyyy-MM-dd HHmmss}");
        Directory.CreateDirectory(this.CaptureFolderPath);
    }

    public string CaptureFolderPath { get; }

    public void Write(LmuRestResponse response, string stateLabel, string note = "")
    {
        this.sequence++;
        var stateFolderPath = Path.Combine(this.CaptureFolderPath, ToSafeName(stateLabel));
        Directory.CreateDirectory(stateFolderPath);

        var filePath = Path.Combine(stateFolderPath, this.CreateFileName(response, note));
        File.WriteAllText(filePath, CreateFileContent(response));
        this.AppendToIndex(response, stateLabel, note);
    }

    private static string CreateFileContent(LmuRestResponse response)
    {
        if(!response.IsResponding)
        {
            return $"No response: {response.Error}";
        }

        JsonFormatter.TryIndent(response.Body, out var content);
        return content;
    }

    private static string ToSafeName(string text)
    {
        var safeCharacters = text.Select(character => UnsafeFileNameCharacters.Contains(character) ? '-' : character);
        return new string(safeCharacters.ToArray()).Trim('-', ' ');
    }

    private void AppendToIndex(LmuRestResponse response, string stateLabel, string note)
    {
        var line = $"{this.sequence:000} | {DateTime.Now:HH:mm:ss.fff} | {stateLabel} | {ResponseSummary.Describe(response)}"
            + (string.IsNullOrEmpty(note) ? string.Empty : $" | {note}");
        File.AppendAllLines(Path.Combine(this.CaptureFolderPath, "index.txt"), [line]);
    }

    private string CreateFileName(LmuRestResponse response, string note)
    {
        var extension = response.IsResponding && JsonFormatter.TryIndent(response.Body, out _) ? "json" : "txt";
        var noteSuffix = string.IsNullOrEmpty(note) ? string.Empty : $" ({ToSafeName(note)})";
        return $"{this.sequence:000} {response.Method.Method} {ToSafeName(response.Path)}{noteSuffix}.{extension}";
    }
}
