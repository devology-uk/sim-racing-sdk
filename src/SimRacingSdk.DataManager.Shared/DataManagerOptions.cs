namespace SimRacingSdk.DataManager.Shared;

public record DataManagerOptions
{
    // The short code used in project names, e.g. "Lmu" for SimRacingSdk.Lmu.Core.
    public required string GameCode { get; init; }

    public required string GameName { get; init; }

    public string DataFolderName => this.GameCode.ToLowerInvariant();
    public string Title => $"Sim Racing SDK Data Manager for {this.GameName}";

    public string CoreProjectName => $"SimRacingSdk.{this.GameCode}.Core";
}
