using SimRacingSdk.R3e.Core;
using SimRacingSdk.R3e.Core.Abstractions;
using SimRacingSdk.R3e.Setups.Abstractions;
using SimRacingSdk.R3e.Setups.Models.Files;

namespace SimRacingSdk.R3e.Setups.Services;

public class R3eSetupProvider : IR3eSetupProvider
{
    private const int FirstVersionNumber = 2;
    private const string SetupFilePattern = "*" + R3eSetupFileName.Extension;

    private static R3eSetupProvider? singletonInstance;

    private readonly Lazy<R3eCarSlugIndex> carSlugIndex;
    private readonly IR3ePathProvider pathProvider;

    public R3eSetupProvider(IR3ePathProvider pathProvider, IR3eCarInfoProvider carInfoProvider)
    {
        this.pathProvider = pathProvider;
        this.carSlugIndex = new Lazy<R3eCarSlugIndex>(() => new R3eCarSlugIndex(carInfoProvider));
    }

    public static R3eSetupProvider Instance =>
        singletonInstance ??= new R3eSetupProvider(R3ePathProvider.Instance, R3eCarInfoProvider.Instance);

    public void DeleteSetupFile(string relativePath)
    {
        File.Delete(this.PathFor(relativePath));
    }

    public byte[] GetSetupFileBytes(string relativePath)
    {
        return File.ReadAllBytes(this.PathFor(relativePath));
    }

    public IReadOnlyList<R3eSetupFileInfo> GetSetupFiles()
    {
        var rootFolderPath = this.pathProvider.SetupsFolderPath;
        if(!Directory.Exists(rootFolderPath))
        {
            return [];
        }

        return Directory.EnumerateDirectories(rootFolderPath)
                        .SelectMany(Directory.EnumerateDirectories)
                        .SelectMany(layoutFolderPath => Directory.EnumerateFiles(layoutFolderPath, SetupFilePattern))
                        .Select(this.BuildFileInfo)
                        .ToList();
    }

    public string InstallSetupFile(string trackFolderName, string layoutFolderName, string fileName, byte[] fileBytes)
    {
        EnsureFolderName(trackFolderName);
        EnsureFolderName(layoutFolderName);
        EnsureSetupFileName(fileName);

        var layoutFolderPath = Path.Combine(this.pathProvider.SetupsFolderPath, trackFolderName, layoutFolderName);
        Directory.CreateDirectory(layoutFolderPath);

        var installedFileName = FindUnusedFileName(layoutFolderPath, fileName);
        File.WriteAllBytes(Path.Combine(layoutFolderPath, installedFileName), fileBytes);
        return Path.Combine(trackFolderName, layoutFolderName, installedFileName);
    }

    private static void EnsureFolderName(string folderName)
    {
        if(string.IsNullOrWhiteSpace(folderName)
           || folderName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
           || folderName.Trim('.').Length == 0)
        {
            throw new ArgumentException("Not a valid track or layout folder name.", nameof(folderName));
        }
    }

    private static void EnsureSetupFileName(string fileName)
    {
        EnsureFolderName(fileName);
        if(!fileName.EndsWith(R3eSetupFileName.Extension, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("A setup file name must end in .svm.", nameof(fileName));
        }
    }

    private static string FindUnusedFileName(string layoutFolderPath, string fileName)
    {
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var candidate = fileName;
        for(var versionNumber = FirstVersionNumber;
            File.Exists(Path.Combine(layoutFolderPath, candidate));
            versionNumber++)
        {
            candidate = $"{nameWithoutExtension}{R3eCarSlugIndex.Separator}{versionNumber}{R3eSetupFileName.Extension}";
        }

        return candidate;
    }

    private R3eSetupFileInfo BuildFileInfo(string filePath)
    {
        var layoutFolderPath = Path.GetDirectoryName(filePath)!;

        return new R3eSetupFileInfo
        {
            CreatedAt = File.GetCreationTime(filePath),
            LayoutFolderName = Path.GetFileName(layoutFolderPath),
            ModifiedAt = File.GetLastWriteTime(filePath),
            Name = R3eSetupFileName.Parse(Path.GetFileName(filePath), this.carSlugIndex.Value),
            TrackFolderName = Path.GetFileName(Path.GetDirectoryName(layoutFolderPath)!)
        };
    }

    // Relative paths come from callers that may pass on outside input, so they must stay inside the setups folder.
    private string PathFor(string relativePath)
    {
        var rootFolderPath = Path.GetFullPath(this.pathProvider.SetupsFolderPath);
        var fullPath = Path.GetFullPath(Path.Combine(rootFolderPath, relativePath));
        if(!fullPath.StartsWith(rootFolderPath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
           || !fullPath.EndsWith(R3eSetupFileName.Extension, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("A setup path must name a setup file in the CarSetups folder.",
                nameof(relativePath));
        }

        return fullPath;
    }
}
