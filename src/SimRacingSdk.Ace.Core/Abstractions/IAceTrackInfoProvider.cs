using System.Collections.ObjectModel;
using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Core.Abstractions;

public interface IAceTrackInfoProvider
{
    AceTrackInfo? FindByTrackAndLayout(string track, string layout);
    ReadOnlyCollection<string> GetTrackNames();
    ReadOnlyCollection<AceTrackInfo> GetLayoutsForTrack(string track);
    ReadOnlyCollection<AceTrackInfo> GetTrackInfos();
}
