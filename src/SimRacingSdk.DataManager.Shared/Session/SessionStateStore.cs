using System.IO;
using System.Text.Json;

namespace SimRacingSdk.DataManager.Shared.Session;

// Kept in LocalAppData rather than the repo's data folder - it's one user's UI state, not catalog
// data. Saved on every change rather than on exit, because stopping the debugger in Visual Studio
// kills the app without ever running OnExit.
public class SessionStateStore<TState> : ISessionStateStore<TState> where TState : SessionState, new()
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    private readonly string filePath;

    public SessionStateStore(DataManagerOptions options)
    {
        this.filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SimRacingSdk",
            $"{options.GameCode}.DataManager",
            "session.json");
        this.State = this.Load();
    }

    public TState State { get; }

    SessionState ISessionStateStore.State => this.State;

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

    private TState Load()
    {
        try
        {
            return File.Exists(this.filePath)
                ? JsonSerializer.Deserialize<TState>(File.ReadAllText(this.filePath)) ?? new TState()
                : new TState();
        }
        catch(Exception exception) when(exception is IOException or JsonException)
        {
            return new TState();
        }
    }
}
