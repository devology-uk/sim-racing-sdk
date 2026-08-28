using System.IO;
using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.Core.Services;
using SimRacingSdk.Lmu.Setups.Abstractions;
using SimRacingSdk.Lmu.Setups.Models;

namespace SimRacingSdk.Lmu.Setups.Services;

// Discovers and parses the user's live LMU setup files directly from disk (read-only, no mirrored
// copy or version history). Unlike ACC/ACE, LMU's Settings folder has no per-car subfolder - every
// car's setups for a track sit as loose files, so car identity can only be resolved by opening each
// file and reading its own //VEH= comment.
public class LmuSetupProvider : ILmuSetupProvider
{
    private static LmuSetupProvider? singletonInstance;

    private readonly ILmuCarInfoProvider carInfoProvider;
    private readonly ILmuPathProvider pathProvider;

    public LmuSetupProvider(ILmuPathProvider pathProvider, ILmuCarInfoProvider carInfoProvider)
    {
        this.pathProvider = pathProvider;
        this.carInfoProvider = carInfoProvider;
    }

    public static LmuSetupProvider Instance { get; } =
        singletonInstance ??= new LmuSetupProvider(LmuPathProvider.Instance, LmuCarInfoProvider.Instance);

    public IReadOnlyList<LmuSetupFileInfo> GetSetupFiles()
    {
        var setupsFolderPath = this.pathProvider.SetupsFolderPath;
        var results = new List<LmuSetupFileInfo>();

        if(!Directory.Exists(setupsFolderPath))
        {
            return results;
        }

        foreach(var trackFolderPath in Directory.GetDirectories(setupsFolderPath))
        {
            var trackFolderName = Path.GetFileName(trackFolderPath);

            foreach(var filePath in Directory.GetFiles(trackFolderPath, "*.svm"))
            {
                LmuSetupFileInfo info;
                try
                {
                    info = this.BuildFileInfo(trackFolderName, filePath);
                }
                catch(Exception)
                {
                    // Skip an unreadable/corrupt file rather than failing the whole scan - these are
                    // freely user- and third-party-edited text files, unlike ACC/ACE's own formats.
                    continue;
                }

                results.Add(info);
            }
        }

        return results;
    }

    public LmuSetupFileInfo GetSetupFile(string trackFolderName, string fileName)
    {
        var filePath = Path.Combine(this.pathProvider.SetupsFolderPath, trackFolderName, fileName);
        return this.BuildFileInfo(trackFolderName, filePath);
    }

    public byte[] GetSetupFileBytes(string trackFolderName, string fileName)
    {
        var filePath = Path.Combine(this.pathProvider.SetupsFolderPath, trackFolderName, fileName);
        return File.ReadAllBytes(filePath);
    }

    public void SaveSetupFile(string trackFolderName, string fileName, byte[] fileBytes)
    {
        var folderPath = Path.Combine(this.pathProvider.SetupsFolderPath, trackFolderName);
        Directory.CreateDirectory(folderPath);
        File.WriteAllBytes(Path.Combine(folderPath, fileName), fileBytes);
    }

    public void DeleteSetupFile(string trackFolderName, string fileName)
    {
        var filePath = Path.Combine(this.pathProvider.SetupsFolderPath, trackFolderName, fileName);
        File.Delete(filePath);
    }

    private LmuSetupFileInfo BuildFileInfo(string trackFolderName, string filePath)
    {
        var setup = LmuSetupFile.Parse(filePath);
        var carInfo = setup.VehicleFolderId is not null
                          ? this.carInfoProvider.FindByModelId(setup.VehicleFolderId)
                          : null;
        var carDisplayName =
            carInfo?.DisplayName ?? setup.VehicleFolderId ?? setup.VehicleClassSetting ?? "Unknown Vehicle";

        return new LmuSetupFileInfo
        {
            TrackFolderName = trackFolderName,
            FileName = Path.GetFileName(filePath),
            CarIdentifier = setup.VehicleFolderId ?? carDisplayName,
            CarDisplayName = carDisplayName,
            Setup = setup
        };
    }
}
