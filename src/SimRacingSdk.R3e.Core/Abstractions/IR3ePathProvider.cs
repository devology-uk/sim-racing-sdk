namespace SimRacingSdk.R3e.Core.Abstractions;

public interface IR3ePathProvider
{
    // The game's own id-to-name catalog (r3e-data.json), shipped with and updated by the game.
    string CatalogFilePath { get; }

    // The Game subfolder, where RRRE.exe and RRRE64.exe live.
    string ExecutablesFolderPath { get; }

    string GameOptionsFilePath { get; }
    string GamePath { get; }

    // One JSON file per session, named <yyyy_MM_dd_HH_mm_ss>_<SessionType>.txt.
    string ResultsFolderPath { get; }

    // Encrypted .svm files under <Track>\<Layout>, named <car name slug>_<setup name slug>.svm.
    string SetupsFolderPath { get; }

    string TimeAttackFolderPath { get; }
    string UserDataFolderPath { get; }
}
