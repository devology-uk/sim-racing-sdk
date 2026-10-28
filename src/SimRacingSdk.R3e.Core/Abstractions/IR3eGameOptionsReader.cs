namespace SimRacingSdk.R3e.Core.Abstractions;

public interface IR3eGameOptionsReader
{
    IReadOnlyDictionary<string, object> Read(string filePath);
}
