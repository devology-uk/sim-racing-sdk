using System.IO;
using System.Linq;
using SimRacingSdk.Ace.Core;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Setups.Abstractions;
using SimRacingSdk.Ace.Setups.Models;

namespace SimRacingSdk.Ace.Setups.Services;

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

    public void SaveSetupFile(string carFolderName, string trackFolderName, string fileName, byte[] fileBytes)
    {
        var folderPath = Path.Combine(this.pathProvider.SetupsFolderPath, carFolderName, trackFolderName);
        Directory.CreateDirectory(folderPath);
        File.WriteAllBytes(Path.Combine(folderPath, fileName), fileBytes);
    }

    public void DeleteSetupFile(string carFolderName, string trackFolderName, string fileName)
    {
        var filePath = Path.Combine(this.pathProvider.SetupsFolderPath, carFolderName, trackFolderName, fileName);
        File.Delete(filePath);
    }

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
