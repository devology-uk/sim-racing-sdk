using System.Collections.ObjectModel;
using SimRacingSdk.Acc.Core.Models;

namespace SimRacingSdk.Acc.Core.Abstractions;

public interface IAccCarInfoProvider
{
    AccCarInfo? FindByModelId(int modelId);
    string GetCarDisplayNameWithYear(int modelId);
    IReadOnlyCollection<AccCarInfo> GetCarInfos();
    ReadOnlyCollection<string> GetCarClasses();
    ReadOnlyCollection<AccCarInfo> GetCarInfosForClass(string carClass);
}