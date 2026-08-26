using System.IO;
using System.Linq;
using System.Text.Json;
using SimRacingSdk.Acc.Core;
using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Acc.Setups.Abstractions;
using SimRacingSdk.Acc.Setups.Models;

namespace SimRacingSdk.Acc.Setups.Services;

// Discovers, parses, and decodes the user's live ACC setup files directly from disk (read-only, no
// mirrored copy or version history). Every raw click index is translated into real units via the
// car's AccSetupMap before it leaves this provider - SRT never sees clicks or the calibration data.
public class AccSetupProvider : IAccSetupProvider
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static AccSetupProvider? singletonInstance;

    private readonly IAccCarInfoProvider carInfoProvider;
    private readonly IAccPathProvider pathProvider;
    private readonly IAccTrackInfoProvider trackInfoProvider;

    public AccSetupProvider(IAccPathProvider pathProvider, IAccCarInfoProvider carInfoProvider,
        IAccTrackInfoProvider trackInfoProvider)
    {
        this.pathProvider = pathProvider;
        this.carInfoProvider = carInfoProvider;
        this.trackInfoProvider = trackInfoProvider;
    }

    public static AccSetupProvider Instance { get; } = singletonInstance ??= new AccSetupProvider(
        AccPathProvider.Instance,
        AccCarInfoProvider.Instance,
        AccTrackInfoProvider.Instance);

    public IReadOnlyList<AccSetupFileInfo> GetSetupFiles()
    {
        var setupsFolderPath = this.pathProvider.SetupsFolderPath;
        var results = new List<AccSetupFileInfo>();

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

                foreach(var filePath in Directory.GetFiles(trackFolderPath, "*.json"))
                {
                    var fileName = Path.GetFileName(filePath);
                    if(fileName.Contains("motec", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    AccSetupFileInfo info;
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

    public AccSetupFileInfo GetSetupFile(string carFolderName, string trackFolderName, string fileName)
    {
        var filePath = Path.Combine(this.pathProvider.SetupsFolderPath, carFolderName, trackFolderName, fileName);
        return this.BuildFileInfo(carFolderName, trackFolderName, filePath);
    }

    public void DeleteSetupFile(string carFolderName, string trackFolderName, string fileName)
    {
        var filePath = Path.Combine(this.pathProvider.SetupsFolderPath, carFolderName, trackFolderName, fileName);
        File.Delete(filePath);
    }

    private AccSetupFileInfo BuildFileInfo(string carFolderName, string trackFolderName, string filePath)
    {
        var json = File.ReadAllText(filePath);
        var setupFile = JsonSerializer.Deserialize<AccSetupFile>(json, JsonOptions) ?? new AccSetupFile();
        var setupMap = AccSetupMapProvider.GetSetupMapForCar(carFolderName);

        var carInfo = this.carInfoProvider.GetCarInfos().FirstOrDefault(c => c.AccName == carFolderName);
        var trackInfo = this.trackInfoProvider.FindByTrackId(trackFolderName);

        return new AccSetupFileInfo
        {
            CarFolderName = carFolderName,
            TrackFolderName = trackFolderName,
            CarDisplayName = carInfo?.DisplayName ?? carFolderName,
            TrackDisplayName = trackInfo?.ShortName ?? trackFolderName,
            FileName = Path.GetFileName(filePath),
            DecodedSetup = AccSetupDecoder.Decode(setupFile, setupMap)
        };
    }
}
