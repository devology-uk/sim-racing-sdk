using System.Collections.ObjectModel;
using SimRacingSdk.Ams2.Core.Models;

namespace SimRacingSdk.Ams2.Core.Abstractions;

public interface IAms2CarInfoProvider
{
    Ams2CarInfo? FindByModel(string model);
    ReadOnlyCollection<string> GetCarClasses();
    ReadOnlyCollection<Ams2CarInfo> GetCarInfos();
    ReadOnlyCollection<Ams2CarInfo> GetCarInfosForClass(string carClass);
}