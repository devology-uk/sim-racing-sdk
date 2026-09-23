using System.Collections.ObjectModel;
using SimRacingSdk.Pmr.Core.Models;

namespace SimRacingSdk.Pmr.Core.Abstractions;

public interface IPmrTrackInfoProvider
{
    PmrTrackInfo? FindByGameId(string gameId);
    PmrTrackInfo? FindByName(string name);
    ReadOnlyCollection<string> GetContinents();
    ReadOnlyCollection<PmrTrackInfo> GetTrackInfos();
    ReadOnlyCollection<PmrTrackInfo> GetTrackInfosForContinent(string continent);
}
