namespace SimRacingSdk.Pmr.Core.Abstractions;

public interface IPmrPathProvider
{
    string DataFolderPath { get; }

    // Under a specific numbered savegame slot ("savegame1"), not a global preferences file -
    // PMR has no separate profile-agnostic settings location. Only ever confirmed against a
    // single-savegame install; there's currently no way to know which slot is "active" if more
    // than one exists.
    string DriverProfileFilePath { get; }

    string GamePath { get; }

    // Saved setups (.vset) live here, flat, named <vehicleGameId>-<setupName>.vset. A stray
    // savegame2 seen after a reinstall held only careerSavegame.xml, inputBinding.xml and a
    // deviceMapping file, never updated afterwards - the game kept using savegame1 for everything.
    string SavegameFolderPath { get; }

    // See DriverProfileFilePath - same savegame-slot caveat applies.
    string SettingsFilePath { get; }

    string TracksFolderPath { get; }
    string UserDataFolderPath { get; }
    string VehiclesFolderPath { get; }
}
