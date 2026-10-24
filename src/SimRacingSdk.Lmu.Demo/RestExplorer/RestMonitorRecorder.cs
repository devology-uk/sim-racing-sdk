using System.IO;
using SimRacingSdk.Lmu.Rest.Models;

namespace SimRacingSdk.Lmu.Demo.RestExplorer;

// Saves the monitor's responses without filling the disk: each path is saved the first time it is seen in a
// game state, then again only when its body has changed and enough time has passed. events.txt gets every
// state, window and availability change, timestamped.
public class RestMonitorRecorder
{
    private static readonly TimeSpan MinimumTimeBetweenSaves = TimeSpan.FromSeconds(30);

    private readonly RestCaptureWriter captureWriter;
    private readonly string eventsFilePath;
    private readonly Dictionary<string, SavedResponse> lastSaves = new();
    private readonly object writeLock = new();

    public RestMonitorRecorder(string rootFolderPath)
    {
        this.captureWriter = new RestCaptureWriter(rootFolderPath);
        this.eventsFilePath = Path.Combine(this.captureWriter.CaptureFolderPath, "events.txt");
    }

    public string CaptureFolderPath => this.captureWriter.CaptureFolderPath;

    public void RecordEvent(string message)
    {
        lock(this.writeLock)
        {
            File.AppendAllLines(this.eventsFilePath, [$"{DateTime.Now:HH:mm:ss.fff} {message}"]);
        }
    }

    public void RecordResponse(LmuRestResponse response, string stateLabel)
    {
        lock(this.writeLock)
        {
            if(!this.IsWorthSaving(response, stateLabel))
            {
                return;
            }

            this.captureWriter.Write(response, stateLabel);
            this.lastSaves[response.Path] = new SavedResponse(stateLabel, response.Body, DateTime.UtcNow);
        }
    }

    private bool IsWorthSaving(LmuRestResponse response, string stateLabel)
    {
        if(!this.lastSaves.TryGetValue(response.Path, out var lastSave) || lastSave.StateLabel != stateLabel)
        {
            return true;
        }

        return lastSave.Body != response.Body && DateTime.UtcNow - lastSave.SavedAt >= MinimumTimeBetweenSaves;
    }

    private record SavedResponse(string StateLabel, string Body, DateTime SavedAt);
}
