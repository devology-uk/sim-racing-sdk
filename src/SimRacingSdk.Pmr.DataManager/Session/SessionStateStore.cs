using System.IO;
using System.Text.Json;

namespace SimRacingSdk.Pmr.DataManager.Session;

// Kept in LocalAppData rather than the repo's data folder - it's one user's UI state, not catalog
// data. Saved on every change rather than on exit, because stopping the debugger in Visual Studio
// kills the app without ever running OnExit.
public class SessionStateStore : ISessionStateStore
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    private readonly string filePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "SimRacingSdk",
        "Pmr.DataManager",
        "session.json");

    public SessionStateStore()
    {
        this.State = this.Load();
    }

    public SessionState State { get; }

    public void Save()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(this.filePath)!);
            File.WriteAllText(this.filePath, JsonSerializer.Serialize(this.State, SerializerOptions));
        }
        catch(Exception exception) when(exception is IOException or UnauthorizedAccessException)
        {
            // Losing where we were is harmless; crashing the editor over it isn't.
        }
    }

    private SessionState Load()
    {
        try
        {
            return File.Exists(this.filePath)
                ? JsonSerializer.Deserialize<SessionState>(File.ReadAllText(this.filePath)) ?? new SessionState()
                : new SessionState();
        }
        catch(Exception exception) when(exception is IOException or JsonException)
        {
            return new SessionState();
        }
    }
}
