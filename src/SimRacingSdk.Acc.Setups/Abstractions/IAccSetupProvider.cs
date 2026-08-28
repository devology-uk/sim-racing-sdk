using SimRacingSdk.Acc.Setups.Models;

namespace SimRacingSdk.Acc.Setups.Abstractions;

public interface IAccSetupProvider
{
    IReadOnlyList<AccSetupFileIdentity> GetSetupFiles();
    AccSetupFileInfo GetSetupFile(string carFolderName, string trackFolderName, string fileName);
    byte[] GetSetupFileBytes(string carFolderName, string trackFolderName, string fileName);
    void DeleteSetupFile(string carFolderName, string trackFolderName, string fileName);
}
