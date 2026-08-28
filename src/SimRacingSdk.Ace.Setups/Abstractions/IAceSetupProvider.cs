using SimRacingSdk.Ace.Setups.Models;

namespace SimRacingSdk.Ace.Setups.Abstractions;

public interface IAceSetupProvider
{
    IReadOnlyList<AceSetupFileInfo> GetSetupFiles();
    AceSetupFileInfo GetSetupFile(string carFolderName, string trackFolderName, string fileName);
    byte[] GetSetupFileBytes(string carFolderName, string trackFolderName, string fileName);
    void SaveSetupFile(string carFolderName, string trackFolderName, string fileName, byte[] fileBytes);
    void DeleteSetupFile(string carFolderName, string trackFolderName, string fileName);
}
