using System.IO;
using System.Linq;
using SimRacingSdk.Ace.Core;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Setups.Abstractions;
using SimRacingSdk.Ace.Setups.Models;

namespace SimRacingSdk.Ace.Setups.Services;

// Discovers and parses the user's live ACE setup files directly from disk (read-only, no mirrored
// copy or version history). Every raw value is already a real physical unit - ACE needs no
// click-index-to-real-unit calibration layer, unlike ACC.
public class AceSetupProvider : IAceSetupProvider
{
    private static AceSetupProvider? singletonInstance;

    private readonly IAceCarInfoProvider carInfoProvider;
    private readonly IAcePathProvider pathProvider;
    private readonly IAceTrackInfoProvider trackInfoProvider;

    public AceSetupProvider(IAcePathProvider pathProvider, IAceCarInfoProvider carInfoProvider,
        IAceTrackInfoProvider trackInfoProvider)
    {
        this.pathProvider = pathProvider;
        this.carInfoProvider = carInfoProvider;
        this.trackInfoProvider = trackInfoProvider;
    }

    public static AceSetupProvider Instance { get; } = singletonInstance ??= new AceSetupProvider(
        AcePathProvider.Instance,
        AceCarInfoProvider.Instance,
        AceTrackInfoProvider.Instance);

    public IReadOnlyList<AceSetupFileInfo> GetSetupFiles()
    {
        var setupsFolderPath = this.pathProvider.SetupsFolderPath;
        var results = new List<AceSetupFileInfo>();

        if(!Directory.Exists(setupsFolderPath))
        {
            return results;
        }

        foreach(var carFolderPath in Directory.GetDirectories(setupsFolderPath))
        {
            var carFolderName = Path.GetFileName(carFolderPath);

            foreach(var trackFolderPath in Directory.GetDirectories(carFolderPath))
            {
                var trackFolderName = Path.GetFileName(trackFolderPath);

                foreach(var filePath in Directory.GetFiles(trackFolderPath, "*.carsetup"))
                {
                    AceSetupFileInfo info;
                    try
                    {
                        info = this.BuildFileInfo(carFolderName, trackFolderName, filePath);
                    }
                    catch(Exception)
                    {
                        // Skip an unreadable/corrupt file rather than failing the whole scan.
                        continue;
                    }

                    results.Add(info);
                }
            }
        }

        return results;
    }

    public AceSetupFileInfo GetSetupFile(string carFolderName, string trackFolderName, string fileName)
    {
        var filePath = Path.Combine(this.pathProvider.SetupsFolderPath, carFolderName, trackFolderName, fileName);
        return this.BuildFileInfo(carFolderName, trackFolderName, filePath);
    }

    public byte[] GetSetupFileBytes(string carFolderName, string trackFolderName, string fileName)
    {
        var filePath = Path.Combine(this.pathProvider.SetupsFolderPath, carFolderName, trackFolderName, fileName);
        return File.ReadAllBytes(filePath);
    }

    public void DeleteSetupFile(string carFolderName, string trackFolderName, string fileName)
    {
        var filePath = Path.Combine(this.pathProvider.SetupsFolderPath, carFolderName, trackFolderName, fileName);
        File.Delete(filePath);
    }

    // ACE's setup folders are named after the car's display name, not a stable internal id like
    // ACC's - and that name doesn't always match the catalog cleanly (confirmed drift multiple
    // times: "SF25" vs the catalog's "SF-25", a since-renamed EV, etc.). So car identity is
    // resolved from each file's own embedded preset string instead of trusting the folder name -
    // every ModelId in the catalog is known to appear verbatim as a substring of it.
    private AceSetupFileInfo BuildFileInfo(string carFolderName, string trackFolderName, string filePath)
    {
        var raw = AceRawCarSetup.Parser.ParseFrom(File.ReadAllBytes(filePath));
        var carInfo = this.carInfoProvider.GetCarInfos().FirstOrDefault(c => raw.CarPresetId.Contains(c.ModelId));
        var trackDisplayName = this.trackInfoProvider.GetLayoutsForTrack(trackFolderName)
                                    .FirstOrDefault()
                                    ?.ShortName ?? trackFolderName;

        return new AceSetupFileInfo
        {
            CarFolderName = carFolderName,
            TrackFolderName = trackFolderName,
            TrackDisplayName = trackDisplayName,
            FileName = Path.GetFileName(filePath),
            CarDisplayName = carInfo?.DisplayName ?? carFolderName,
            CarInfo = carInfo,
            RawSetup = raw
        };
    }
}
