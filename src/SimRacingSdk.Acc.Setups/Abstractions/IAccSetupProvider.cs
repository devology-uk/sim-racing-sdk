using SimRacingSdk.Acc.Setups.Models;

namespace SimRacingSdk.Acc.Setups.Abstractions;

public interface IAccSetupProvider
{
    IReadOnlyList<AccSetupFileInfo> GetSetupFiles();
    AccSetupFileInfo GetSetupFile(string carFolderName, string trackFolderName, string fileName);
    void DeleteSetupFile(string carFolderName, string trackFolderName, string fileName);
}
