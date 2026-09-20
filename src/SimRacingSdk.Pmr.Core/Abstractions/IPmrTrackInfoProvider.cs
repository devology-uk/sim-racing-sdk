using System.Collections.ObjectModel;
using SimRacingSdk.Pmr.Core.Models;

namespace SimRacingSdk.Pmr.Core.Abstractions;

public interface IPmrTrackInfoProvider
{
    PmrTrackInfo? FindByTrackAndLayout(string trackName, string layoutId);
    ReadOnlyCollection<string> GetContinents();

    // Keyed by TrackName rather than TrackFolder - a handful of venues (e.g. Nurburg_full,
    // which holds both the short Nordschleife and the 24h combined layout) share one install
    // folder across genuinely distinct venues with their own TrackName, so TrackFolder can't
    // uniquely identify one for UI drill-down.
    ReadOnlyCollection<PmrTrackInfo> GetLayoutsForTrack(string trackName);
    ReadOnlyCollection<PmrTrackInfo> GetLayoutsForGameId(string gameId);
    ReadOnlyCollection<PmrTrackInfo> GetTrackInfos();
    ReadOnlyCollection<string> GetTrackNames();
    ReadOnlyCollection<string> GetTrackNamesForContinent(string continent);
}
