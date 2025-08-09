using System.Text;
using System.Text.Json;
using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Acc.Core.Exceptions;
using SimRacingSdk.Acc.Core.Models.Customs;
using SimRacingSdk.Acc.Core.Models.RaceResult;

namespace SimRacingSdk.Acc.Core;

public class AccDataProvider
{
    private static AccDataProvider? singletonInstance;
    private readonly IAccPathProvider accPathProvider;
    private readonly IList<string> FilePrefixes = new List<string>
    {
        "Race",
        "Hotstint",
        "Qualifying",
        "Practice"
    };

    public AccDataProvider(IAccPathProvider accPathProvider)
    {
        this.accPathProvider = accPathProvider;
    }

    public static AccDataProvider Instance =>
        singletonInstance ??= new AccDataProvider(AccPathProvider.Instance);

    public IEnumerable<CustomCar> GetCustomCars()
    {
        if(!Directory.Exists(this.accPathProvider.CustomCarsFolderPath))
        {
            return [];
        }

        var filePaths = Directory.GetFiles(this.accPathProvider.CustomCarsFolderPath, "*.json");
        var result = new List<CustomCar>();
        foreach(var filePath in filePaths)
        {
            try
            {
                var content = File.ReadAllText(filePath, Encoding.Unicode);
                if(!content.Contains("displayName") && !content.Contains("carModelType"))
                {
                    continue;
                }

                var customCar = JsonSerializer.Deserialize<CustomCar>(this.CleanJson(content));
                customCar.FilePath = filePath;
                result.Add(customCar);
            }
            catch(Exception exception)
            {
                throw new InvalidCustomCarException(filePath, exception);
            }
        }

        return result;
    }

    public IEnumerable<CustomSkin> GetCustomSkins()
    {
        if(!Directory.Exists(this.accPathProvider.CustomLiveriesFolderPath))
        {
            return [];
        }

        var folderPaths = Directory.GetDirectories(this.accPathProvider.CustomLiveriesFolderPath);

        var result = new List<CustomSkin>();

        foreach(var folderPath in folderPaths)
        {
            var folderName = Path.GetRelativePath(this.accPathProvider.CustomLiveriesFolderPath, folderPath);
            result.Add(new CustomSkin
            {
                Name = folderName,
                FolderPath = folderPath
            });
        }

        return result;
    }

    public IEnumerable<string> GetRecentSessionFilePaths()
    {
        if(!Directory.Exists(this.accPathProvider.ResultFolderPath))
        {
            return [];
        }

        return Directory.GetFiles(this.accPathProvider.ResultFolderPath, "*.json")
                        .Where(this.IsLocalSessionFile)
                        .ToList();
    }

    public IEnumerable<RaceSession> GetRecentSessions()
    {
        var result = new List<RaceSession>();

        var sessionFilePaths = this.GetRecentSessionFilePaths();
        foreach(var sessionFilePath in sessionFilePaths)
        {
            var raceSession = this.LoadRaceSession(sessionFilePath);
            result.Add(raceSession);
        }

        return result;
    }

    public bool IsLocalSessionFile(string sessionFilePath)
    {
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sessionFilePath);
        return this.FilePrefixes.Any(filePrefix => fileNameWithoutExtension.StartsWith(filePrefix));
    }

    public RaceSession LoadRaceSession(string filePath)
    {
        try
        {
            var fileContent = File.ReadAllText(filePath);
            var json = this.CleanJson(fileContent);
            return JsonSerializer.Deserialize<RaceSession>(json)!;
        }
        catch(Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }

    private string CleanJson(string json)
    {
        return json.Replace("\0", "")
                   .Replace("\n", "");
    }
}