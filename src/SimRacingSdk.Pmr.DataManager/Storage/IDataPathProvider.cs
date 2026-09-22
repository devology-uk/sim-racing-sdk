namespace SimRacingSdk.Pmr.DataManager.Storage;

public interface IDataPathProvider
{
    string GetEntityFolder(string entityName);
    string GetRepoRoot();
}
