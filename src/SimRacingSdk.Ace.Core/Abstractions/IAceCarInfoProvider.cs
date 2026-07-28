using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Core.Abstractions;

public interface IAceCarInfoProvider
{
    // Unverified assumption: treats the UDP CarModelType byte as an index into cars.json's
    // array order, since the file has no explicit numeric car ID. See AceCarInfoProvider.cs.
    AceCarInfo? FindByModelId(int modelId);
    AceCarInfo? FindByName(string name);
    IReadOnlyCollection<AceCarInfo> GetCarInfos();
}
