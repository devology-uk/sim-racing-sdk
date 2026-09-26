using System.IO;
using System.Text.RegularExpressions;
using SimRacingSdk.Pmr.Core.Abstractions;
using SimRacingSdk.Pmr.Core.Models;

namespace SimRacingSdk.Pmr.Setups.Models.Files;

// A saved setup's file name is <vehicleGameId>-<setupName>.vset. Game ids can contain "-" themselves
// (e.g. "ID_MAZDA_RX-7_GTO_1989"), so the car is found by its longest matching id prefix, falling
// back to the last "-" for a car not in the catalog.
public record PmrSetupFileName(string FileName, string VehicleGameId, string SetupName, PmrCarInfo? Car)
{
    public const string Extension = ".vset";

    private const char VehicleSeparator = '-';
    private const char VersionSeparator = '_';
    private static readonly Regex TrailingVersionNumber = new(@"_\d+$", RegexOptions.Compiled);

    public string CarDisplayName => this.Car?.DisplayName ?? this.VehicleGameId;

    public static PmrSetupFileName Parse(string fileName, IPmrCarInfoProvider carInfoProvider)
    {
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var car = FindCarByPrefix(nameWithoutExtension, carInfoProvider);
        var vehicleGameId = car is null
                                ? SplitAtLastSeparator(nameWithoutExtension)
                                : nameWithoutExtension[..car.GameId.Length];
        var setupName = nameWithoutExtension.Length > vehicleGameId.Length
                            ? nameWithoutExtension[(vehicleGameId.Length + 1)..]
                            : nameWithoutExtension;

        return new PmrSetupFileName(fileName, vehicleGameId, setupName, car);
    }

    public static string WithVersionNumber(string nameWithoutExtension, int versionNumber)
    {
        var nameWithoutVersion = TrailingVersionNumber.Replace(nameWithoutExtension, string.Empty);
        return $"{nameWithoutVersion}{VersionSeparator}{versionNumber}";
    }

    private static PmrCarInfo? FindCarByPrefix(string nameWithoutExtension, IPmrCarInfoProvider carInfoProvider)
    {
        return carInfoProvider.GetCarInfos()
                              .Where(car => !string.IsNullOrEmpty(car.GameId))
                              .Where(car => nameWithoutExtension.StartsWith(car.GameId + VehicleSeparator, StringComparison.OrdinalIgnoreCase))
                              .MaxBy(car => car.GameId.Length);
    }

    private static string SplitAtLastSeparator(string nameWithoutExtension)
    {
        var separatorIndex = nameWithoutExtension.LastIndexOf(VehicleSeparator);
        return separatorIndex > 0 ? nameWithoutExtension[..separatorIndex] : nameWithoutExtension;
    }
}
