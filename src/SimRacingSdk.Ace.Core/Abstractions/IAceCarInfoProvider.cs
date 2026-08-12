using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Core.Abstractions;

public interface IAceCarInfoProvider
{
    // ModelId is the preset id string from the ACE Dedicated Server's cars.json (e.g.
    // "preset_m4gt3_mech_1") - see AceCarInfoProvider.cs.
    AceCarInfo? FindByModelId(string modelId);
    AceCarInfo? FindByName(string name);

    // Matches against AceCarInfo.AceName, the Manufacturer+Name form Ace's shared memory
    // actually reports as CarModel (e.g. "BMW M4 GT3 Evo") - use this, not FindByName, when
    // matching a shared-memory CarModel string.
    AceCarInfo? FindByAceName(string aceName);
    IReadOnlyCollection<AceCarInfo> GetCarInfos();
    IReadOnlyCollection<AceCarInfo> GetCarInfosForManufacturer(string manufacturer);
    IReadOnlyCollection<string> GetManufacturers();
}
