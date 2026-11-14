using SimRacingSdk.R3e.Setups.Models.Files;

namespace SimRacingSdk.R3e.Setups.Abstractions;

// The player's saved setups. The files are encrypted by the game, so they can be listed, copied and installed but
// not read. Paths are relative to the CarSetups folder.
public interface IR3eSetupProvider
{
    void DeleteSetupFile(string relativePath);
    byte[] GetSetupFileBytes(string relativePath);
    IReadOnlyList<R3eSetupFileInfo> GetSetupFiles();

    // Saves a setup obtained elsewhere (e.g. downloaded) without overwriting an existing file: a clashing name
    // gets a "_2", "_3"... suffix. Returns the relative path actually written.
    string InstallSetupFile(string trackFolderName, string layoutFolderName, string fileName, byte[] fileBytes);
}
