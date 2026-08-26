#nullable disable

using System.IO;
using System.Text.Json;
using SimRacingSdk.Acc.Setups.Models;

namespace SimRacingSdk.Acc.Setups.Services;

public static class AccSetupMapProvider
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly string SetupMapsFolderPath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SetupMaps");

    public static AccSetupMap GetSetupMapForCar(string carAccName)
    {
        var carFilePath = Path.Combine(SetupMapsFolderPath, $"{carAccName}.json");
        var filePath = File.Exists(carFilePath)
            ? carFilePath
            : Path.Combine(SetupMapsFolderPath, "_default.json");

        if(!File.Exists(filePath))
        {
            return new AccSetupMap();
        }

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<AccSetupMap>(json, JsonOptions) ?? new AccSetupMap();
    }
}
