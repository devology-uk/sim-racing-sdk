using SimRacingSdk.Lmu.Setups.Models;

namespace SimRacingSdk.Lmu.Setups.Abstractions;

public interface ILmuSetupProvider
{
    IReadOnlyList<LmuSetupFileInfo> GetSetupFiles();
    LmuSetupFileInfo GetSetupFile(string trackFolderName, string fileName);
}
