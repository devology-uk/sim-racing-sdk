namespace SimRacingSdk.R3e.Core.Models;

// The game creates one user folder per install ("RaceRoom Racing Experience", "... Install 2")
// and records in it which install's Game folder it belongs to.
public record R3eUserFolder
{
    public string ExecutablesFolderPath { get; init; } = string.Empty;
    public DateTime LastLaunchedUtc { get; init; }
    public string RootFolderPath { get; init; } = string.Empty;
}
