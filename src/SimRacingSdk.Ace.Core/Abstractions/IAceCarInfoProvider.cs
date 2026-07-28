using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Core.Abstractions;

public interface IAceCarInfoProvider
{
    // ModelId is null on every seeded car until Mike finds a real numeric car ID source for
    // Evo - see AceCarInfoProvider.cs. FindByModelId will not resolve anything until then.
    AceCarInfo? FindByModelId(int modelId);
    AceCarInfo? FindByName(string name);
    IReadOnlyCollection<AceCarInfo> GetCarInfos();
    IReadOnlyCollection<AceCarInfo> GetCarInfosForManufacturer(string manufacturer);
    IReadOnlyCollection<string> GetManufacturers();
}
