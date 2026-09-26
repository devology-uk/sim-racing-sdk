using System.IO;
using SimRacingSdk.Pmr.Core;
using SimRacingSdk.Pmr.Core.Abstractions;
using SimRacingSdk.Pmr.Setups.Abstractions;
using SimRacingSdk.Pmr.Setups.Models.Files;

namespace SimRacingSdk.Pmr.Setups.Services;

public class PmrSetupProvider : IPmrSetupProvider
{
    private const string SetupFilePattern = "*" + PmrSetupFileName.Extension;
    private const int FirstVersionNumber = 2;

    private static PmrSetupProvider? singletonInstance;

    private readonly IPmrCarInfoProvider carInfoProvider;
    private readonly IPmrPathProvider pathProvider;

    public PmrSetupProvider(IPmrPathProvider pathProvider, IPmrCarInfoProvider carInfoProvider)
    {
        this.pathProvider = pathProvider;
        this.carInfoProvider = carInfoProvider;
    }

    public static PmrSetupProvider Instance =>
        singletonInstance ??= new PmrSetupProvider(PmrPathProvider.Instance, PmrCarInfoProvider.Instance);

    public void DeleteSetupFile(string fileName)
    {
        File.Delete(this.PathFor(fileName));
    }

    public byte[] GetSetupFileBytes(string fileName)
    {
        return File.ReadAllBytes(this.PathFor(fileName));
    }

    public IReadOnlyList<PmrSetupFileInfo> GetSetupFiles()
    {
        var folderPath = this.pathProvider.SavegameFolderPath;
        if(!Directory.Exists(folderPath))
        {
            return [];
        }

        return Directory.EnumerateFiles(folderPath, SetupFilePattern)
                        .Select(this.BuildFileInfo)
                        .ToList();
    }

    public string InstallSetupFile(string fileName, byte[] fileBytes)
    {
        Directory.CreateDirectory(this.pathProvider.SavegameFolderPath);
        var installedFileName = this.FindUnusedFileName(fileName);
        File.WriteAllBytes(this.PathFor(installedFileName), fileBytes);
        return installedFileName;
    }

    public PmrSetupFile ReadSetupFile(string fileName)
    {
        return PmrSetupFile.Parse(this.ParseName(fileName), File.ReadAllText(this.PathFor(fileName)));
    }

    public void SaveSetupFile(string fileName, byte[] fileBytes)
    {
        Directory.CreateDirectory(this.pathProvider.SavegameFolderPath);
        File.WriteAllBytes(this.PathFor(fileName), fileBytes);
    }

    private PmrSetupFileInfo BuildFileInfo(string filePath)
    {
        return new PmrSetupFileInfo
        {
            CreatedAt = File.GetCreationTime(filePath),
            ModifiedAt = File.GetLastWriteTime(filePath),
            Name = this.ParseName(Path.GetFileName(filePath))
        };
    }

    private string FindUnusedFileName(string fileName)
    {
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var candidate = fileName;
        for(var versionNumber = FirstVersionNumber; File.Exists(this.PathFor(candidate)); versionNumber++)
        {
            candidate = PmrSetupFileName.WithVersionNumber(nameWithoutExtension, versionNumber) + PmrSetupFileName.Extension;
        }

        return candidate;
    }

    private PmrSetupFileName ParseName(string fileName)
    {
        return PmrSetupFileName.Parse(fileName, this.carInfoProvider);
    }

    private string PathFor(string fileName)
    {
        if(Path.GetFileName(fileName) != fileName)
        {
            throw new ArgumentException("A setup file name must name a file in the savegame folder.", nameof(fileName));
        }

        return Path.Combine(this.pathProvider.SavegameFolderPath, fileName);
    }
}
