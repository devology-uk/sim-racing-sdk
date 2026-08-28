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

    // Identity only, deliberately no file content read or decode - CarFolderName/TrackFolderName/
    // FileName all come from the folder structure, and the display names are catalog lookups by
    // folder name, so enumerating for a search list/hierarchy never needs to touch a file's bytes.
    // A corrupt file therefore still appears here even though GetSetupFile would fail to open it -
    // that failure surfaces when the user actually selects it, not silently at listing time.
    public IReadOnlyList<AccSetupFileIdentity> GetSetupFiles()
    {
        var setupsFolderPath = this.pathProvider.SetupsFolderPath;
        var results = new List<AccSetupFileIdentity>();

        if(!Directory.Exists(setupsFolderPath))
        {
            return results;
        }

        foreach(var carFolderPath in Directory.GetDirectories(setupsFolderPath))
        {
            var carFolderName = Path.GetFileName(carFolderPath);
            var carDisplayName = this.carInfoProvider.GetCarInfos()
                                      .FirstOrDefault(c => c.AccName == carFolderName)
                                      ?.DisplayName ?? carFolderName;

            foreach(var trackFolderPath in Directory.GetDirectories(carFolderPath))
            {
                var trackFolderName = Path.GetFileName(trackFolderPath);
                var trackDisplayName = this.trackInfoProvider.FindByTrackId(trackFolderName)?.ShortName
                                        ?? trackFolderName;

                foreach(var filePath in Directory.GetFiles(trackFolderPath, "*.json"))
                {
                    var fileName = Path.GetFileName(filePath);
                    if(fileName.Contains("motec", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    results.Add(new AccSetupFileIdentity
                    {
                        CarFolderName = carFolderName,
                        TrackFolderName = trackFolderName,
                        CarDisplayName = carDisplayName,
                        TrackDisplayName = trackDisplayName,
                        FileName = fileName
                    });
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
