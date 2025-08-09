using SimRacingSdk.Acc.Core.Models;

namespace SimRacingSdk.Acc.Core.Abstractions;

public interface IAccCarInfoProvider
{
    List<AccCarInfo> Cars { get; }
    AccCarInfo? FindByModelId(int modelId);
    string GetCarDisplayNameWithYear(int modelId);
}