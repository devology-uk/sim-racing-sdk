using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.Core.Models;

namespace SimRacingSdk.Lmu.Core.Services;

public class LmuTrackInfoProvider : ILmuTrackInfoProvider
{
    private const string DefaultLayoutName = "Default";
    private const string NotFollowedByLetterOrDigit = @"(?![\p{L}\p{N}])";
    private const string NotPrecededByLetterOrDigit = @"(?<![\p{L}\p{N}])";

    private static LmuTrackInfoProvider? singletonInstance;

    // Shared memory asks on every frame, so each raw name is resolved once.
    private readonly ConcurrentDictionary<string, (LmuTrackInfo Track, LmuTrackLayoutInfo Layout)?> resolvedRawTrackNames =
        new(StringComparer.InvariantCultureIgnoreCase);

    private readonly List<LmuTrackInfo> tracks =
    [
        new() { Country = "Portugal", CountryCode = "PRT", Latitude = 37.231944, Layouts = [new() { Corners = 15, LengthM = 4653, Name = "Default", RawTrackNames = [] }], Longitude = -8.631944, Name = "Algarve International Circuit", ShortName = "Portimao" },
        new() { Country = "Italy", CountryCode = "ITA", Latitude = 44.341111, Layouts = [new() { Corners = 20, LengthM = 4909, Name = "Default", RawTrackNames = [] }], Longitude = 11.713333, Name = "Autodromo Enzo E Dino Ferrari", ShortName = "Imola" },
        new() { Country = "Brazil", CountryCode = "BRA", Latitude = -23.701111, Layouts = [new() { Corners = 15, LengthM = 4309, Name = "Default", RawTrackNames = [] }], Longitude = -46.697222, Name = "Autódromo José Carlos Pace", ShortName = "Interlagos" },
        new() { Country = "Italy", CountryCode = "ITA", Latitude = 45.620556, Layouts = [new() { Corners = 11, LengthM = 5793, Name = "Default", RawTrackNames = [] }, new() { Corners = 10, LengthM = 5793, Name = "Curva Grande Circuit", RawTrackNames = ["Monza Curva Grande Circuit"] }], Longitude = 9.289444, Name = "Autodromo Nazionale Monza", ShortName = "Monza" },
        new() { Country = "Bahrain", CountryCode = "BHR", Latitude = 26.0325, Layouts = [new() { Corners = 15, LengthM = 5412, Name = "Default", RawTrackNames = [] }, new() { Corners = 24, LengthM = 6299, Name = "Endurance Circuit", RawTrackNames = ["Bahrain Endurance Circuit"] }, new() { Corners = 11, LengthM = 3543, Name = "Outer Circuit", RawTrackNames = ["Bahrain Outer Circuit"] }, new() { Corners = 10, LengthM = 3705, Name = "Paddock Circuit", RawTrackNames = [] }], Longitude = 50.510556, Name = "Bahrain International Circuit", ShortName = "Sakhir" },
        new() { Country = "Spain", CountryCode = "ESP", Latitude = 41.57, Layouts = [new() { Corners = 14, LengthM = 4657, Name = "Default", RawTrackNames = [] }], Longitude = 2.2578, Name = "Circuit de Barcelona", ShortName = "Barcelona" },
        new() { Country = "France", CountryCode = "FRA", Latitude = 47.933333, Layouts = [new() { Corners = 38, LengthM = 13626, Name = "Default", RawTrackNames = [] }, new() { Corners = 34, LengthM = 13530, Name = "Mulsanne", RawTrackNames = [] }], Longitude = 0.233333, Name = "Circuit de la Sarthe", ShortName = "Le Mans" },
        new() { Country = "Belgium", CountryCode = "BEL", Latitude = 50.437222, Layouts = [new() { Corners = 20, LengthM = 7004, Name = "Default", RawTrackNames = [] }, new() { Corners = 20, LengthM = 7004, Name = "Endurance Circuit", RawTrackNames = [] }], Longitude = 5.971389, Name = "Circuit de Spa-Francorchamps", ShortName = "Spa" },
        new() { Country = "USA", CountryCode = "USA", Latitude = 30.132778, Layouts = [new() { Corners = 20, LengthM = 5513, Name = "Default", RawTrackNames = [] }, new() { Corners = 17, LengthM = 3702, Name = "National Circuit", RawTrackNames = [] }], Longitude = -97.641111, Name = "Circuit of the Americas", ShortName = "COTA" },
        new() { Country = "USA", CountryCode = "USA", Latitude = 29.18777976891776, Layouts = [new() { Corners = 13, LengthM = 5729, Name = "Default", RawTrackNames = [] }], Longitude = -81.0684744190165, Name = "Daytona International Speedway", ShortName = "Daytona" },
        new() { Country = "Japan", CountryCode = "JPN", Latitude = 35.371667, Layouts = [new() { Corners = 16, LengthM = 4563, Name = "Default", RawTrackNames = [] }, new() { Corners = 14, LengthM = 4526, Name = "Classic Circuit", RawTrackNames = ["Fuji Speedway Classic"] }], Longitude = 138.926667, Name = "Fuji Speedway", ShortName = "Fuji" },
        new() { Country = "USA", CountryCode = "USA", Latitude = 33.765, Layouts = [new() { Corners = 11, LengthM = 3167, Name = "Default", RawTrackNames = [] }], Longitude = -118.189, Name = "Grand Prix of Long Beach", ShortName = "Long Beach" },
        new() { Country = "Qatar", CountryCode = "QAT", Latitude = 25.49, Layouts = [new() { Corners = 16, LengthM = 5400, Name = "Default", RawTrackNames = [] }, new() { Corners = 11, LengthM = 3701, Name = "Short Circuit", RawTrackNames = ["Lusail Short Circuit"] }], Longitude = 51.454167, Name = "Lusail International Circuit", ShortName = "Lusail" },
        new() { Country = "USA", CountryCode = "USA", Latitude = 34.14667, Layouts = [new() { Corners = 12, LengthM = 4088, Name = "Default", RawTrackNames = [] }], Longitude = -83.81778, Name = "Michelin Raceway Road Atlanta", ShortName = "Road Atlanta" },
        new() { Country = "France", CountryCode = "FRA", Latitude = 43.2514641, Layouts = [new() { Corners = 13, LengthM = 5770, Name = "Default", RawTrackNames = ["Paul Ricard - ELMS"] }, new() { Corners = 13, LengthM = 5752, Name = "Layout 1A", RawTrackNames = ["Paul Ricard - 1A"] }, new() { Corners = 15, LengthM = 5841, Name = "Layout 1A-V2", RawTrackNames = ["Paul Ricard - 1A-V2"] }, new() { Corners = 14, LengthM = 5227, Name = "Layout 1A-V2 Short", RawTrackNames = ["Paul Ricard - 1A-V2-Short"] }, new() { Corners = 8, LengthM = 3793, Name = "Layout 3A", RawTrackNames = ["Paul Ricard - 3A"] }], Longitude = 5.7907357, Name = "Paul Ricard Circuit", ShortName = "Paul Ricard" },
        new() { Country = "USA", CountryCode = "USA", Latitude = 27.455, Layouts = [new() { Corners = 17, LengthM = 6019, Name = "Default", RawTrackNames = [] }, new() { Corners = 7, LengthM = 3219, Name = "School Circuit", RawTrackNames = ["Sebring School Circuit"] }], Longitude = -81.35, Name = "Sebring International Raceway", ShortName = "Sebring" },
        new() { Country = "England", CountryCode = "GBR-ENG", Latitude = 52.0680505, Layouts = [new() { Corners = 18, LengthM = 5891, Name = "Default", RawTrackNames = ["Silverstone Grand Prix Circuit - ELMS"] }, new() { Corners = 8, LengthM = 2979, Name = "International Circuit", RawTrackNames = [] }, new() { Corners = 6, LengthM = 2639, Name = "National Circuit", RawTrackNames = [] }], Longitude = -1.0292908, Name = "Silverstone Circuit", ShortName = "Silverstone" },
        new() { Country = "USA", CountryCode = "USA", Latitude = 36.24666666666667, Layouts = [new() { Corners = 11, LengthM = 2602, Name = "Default", RawTrackNames = [] }], Longitude = -121.87166666666667, Name = "WeatherTech Raceway Laguna Seca", ShortName = "Laguna Seca" },
    ];

    public static LmuTrackInfoProvider Instance => singletonInstance ??= new LmuTrackInfoProvider();

    public LmuTrackInfo? FindTrackByShortName(string shortName)
    {
        return this.tracks.FirstOrDefault(t => t.ShortName == shortName);
    }

    public LmuTrackLayoutInfo? FindLayoutByRawTrackName(string rawTrackName)
    {
        return this.FindTrackAndLayoutByRawTrackName(rawTrackName)?.Layout;
    }

    public (LmuTrackInfo Track, LmuTrackLayoutInfo Layout)? FindTrackAndLayoutByRawTrackName(string rawTrackName)
    {
        return string.IsNullOrWhiteSpace(rawTrackName)
            ? null
            : this.resolvedRawTrackNames.GetOrAdd(rawTrackName.Trim(), this.ResolveRawTrackName);
    }

    public LmuTrackInfo? GeTrackInfoByVenue(string venue)
    {
        return this.tracks.FirstOrDefault(t => t.Name.Equals(venue,
                                              StringComparison.InvariantCultureIgnoreCase));
    }

    public IReadOnlyCollection<LmuTrackInfo> GetTrackInfos()
    {
        return this.tracks.AsReadOnly();
    }

    public ReadOnlyCollection<string> GetTrackNames()
    {
        return this.tracks.Select(t => t.ShortName)
                   .ToList()
                   .AsReadOnly();
    }

    // LMU names a layout inconsistently ("Sebring School Circuit", "Bahrain Outer Circuit",
    // "Fuji Speedway Classic"), so the exact names it reports are captured per layout and checked
    // first. Anything else is resolved track first, then layout within that track, matching whole
    // words with the longest match winning - so "Silverstone International Circuit" can never
    // resolve to a "National Circuit" layout, at Silverstone or at another track.
    private (LmuTrackInfo Track, LmuTrackLayoutInfo Layout)? ResolveRawTrackName(string rawTrackName)
    {
        return this.FindByKnownRawTrackName(rawTrackName) ?? this.FindByTrackThenLayout(rawTrackName);
    }

    private (LmuTrackInfo Track, LmuTrackLayoutInfo Layout)? FindByKnownRawTrackName(string rawTrackName)
    {
        foreach(var track in this.tracks)
        {
            var layout = track.Layouts.FirstOrDefault(
                l => l.RawTrackNames.Contains(rawTrackName, StringComparer.InvariantCultureIgnoreCase));
            if(layout != null)
            {
                return (track, layout);
            }
        }

        return null;
    }

    private (LmuTrackInfo Track, LmuTrackLayoutInfo Layout)? FindByTrackThenLayout(string rawTrackName)
    {
        var venueTrack = this.GeTrackInfoByVenue(rawTrackName);
        if(venueTrack != null)
        {
            return WithLayout(venueTrack, DefaultLayout(venueTrack));
        }

        var track = this.FindTrackNamedIn(rawTrackName);
        return track == null
            ? null
            : WithLayout(track, FindLayoutNamedIn(track, rawTrackName) ?? DefaultLayout(track));
    }

    private LmuTrackInfo? FindTrackNamedIn(string rawTrackName)
    {
        return this.tracks.Select(t => (Track: t, MatchLength: LongestNameIn(rawTrackName, t.Name, t.ShortName)))
                   .Where(match => match.MatchLength > 0)
                   .OrderByDescending(match => match.MatchLength)
                   .Select(match => match.Track)
                   .FirstOrDefault();
    }

    private static LmuTrackLayoutInfo? FindLayoutNamedIn(LmuTrackInfo track, string rawTrackName)
    {
        return track.Layouts.Where(l => l.Name != DefaultLayoutName && ContainsWholeWords(rawTrackName, l.Name))
                    .OrderByDescending(l => l.Name.Length)
                    .FirstOrDefault();
    }

    private static LmuTrackLayoutInfo? DefaultLayout(LmuTrackInfo track)
    {
        return track.Layouts.FirstOrDefault(l => l.Name == DefaultLayoutName) ?? track.Layouts.FirstOrDefault();
    }

    private static (LmuTrackInfo Track, LmuTrackLayoutInfo Layout)? WithLayout(LmuTrackInfo track, LmuTrackLayoutInfo? layout)
    {
        return layout == null ? null : (track, layout);
    }

    private static int LongestNameIn(string rawTrackName, params string[] names)
    {
        return names.Where(name => !string.IsNullOrWhiteSpace(name) && ContainsWholeWords(rawTrackName, name))
                    .Select(name => name.Length)
                    .DefaultIfEmpty(0)
                    .Max();
    }

    private static bool ContainsWholeWords(string text, string phrase)
    {
        var pattern = NotPrecededByLetterOrDigit + Regex.Escape(phrase) + NotFollowedByLetterOrDigit;
        return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }
}
