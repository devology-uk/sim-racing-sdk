using System.Collections.ObjectModel;
using SimRacingSdk.Ace.Core.Models;

namespace SimRacingSdk.Ace.Core.Abstractions;

public interface IAceTrackInfoProvider
{
    // Name is the track name ACE's shared memory reports, which is also its setup folder name.
    AceTrackInfo? FindByName(string name);
    AceTrackInfo? FindByShortName(string shortName);
    ReadOnlyCollection<string> GetContinents();
    ReadOnlyCollection<AceTrackInfo> GetTrackInfos();
    ReadOnlyCollection<AceTrackInfo> GetTrackInfosForContinent(string continent);
}

