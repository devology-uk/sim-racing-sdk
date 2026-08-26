using SimRacingSdk.Ace.Setups.Models;

namespace SimRacingSdk.Ace.Setups.Abstractions;

public interface IAceSetupProvider
{
    IReadOnlyList<AceSetupFileInfo> GetSetupFiles();
    AceSetupFileInfo GetSetupFile(string carFolderName, string trackFolderName, string fileName);
}
