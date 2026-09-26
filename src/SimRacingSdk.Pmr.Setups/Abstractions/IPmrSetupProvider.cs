using SimRacingSdk.Pmr.Setups.Models.Files;

namespace SimRacingSdk.Pmr.Setups.Abstractions;

// The player's saved setups: flat .vset files in the savegame folder.
public interface IPmrSetupProvider
{
    void DeleteSetupFile(string fileName);
    byte[] GetSetupFileBytes(string fileName);
    IReadOnlyList<PmrSetupFileInfo> GetSetupFiles();

    // Saves a setup obtained elsewhere (e.g. downloaded) without overwriting an existing file: a
    // clashing name gets a "_2", "_3"... suffix. Returns the file name actually written.
    string InstallSetupFile(string fileName, byte[] fileBytes);

    PmrSetupFile ReadSetupFile(string fileName);
    void SaveSetupFile(string fileName, byte[] fileBytes);
}
