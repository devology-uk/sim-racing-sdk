using System.Collections.ObjectModel;
using SimRacingSdk.Acc.Core.Models;

namespace SimRacingSdk.Acc.Core.Abstractions;

public interface IAccTrackInfoProvider
{
    AccTrackInfo? FindByFullName(string fullName);
    AccTrackInfo? FindByTrackId(string trackId);
    string GetNameByTrackId(string trackId);
    ReadOnlyCollection<string> GetTrackNames();
    ReadOnlyCollection<AccTrackInfo> GetTrackInfos();
}