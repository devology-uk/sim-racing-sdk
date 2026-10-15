namespace SimRacingSdk.DataManager.Shared.Storage;

public interface IDataPathProvider
{
    string GetEntityFolder(string entityName);
    string GetProjectFolder(string projectName);
    string GetRepoRoot();
}
