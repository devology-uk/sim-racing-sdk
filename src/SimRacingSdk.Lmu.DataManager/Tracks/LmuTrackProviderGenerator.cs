using System.IO;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Storage;
using static SimRacingSdk.DataManager.Shared.Generation.CSharpLiteral;

namespace SimRacingSdk.Lmu.DataManager.Tracks;

// Regenerates SimRacingSdk.Lmu.Core's whole track-catalog surface - both models, the interface and
// the provider - from the tracks captured here.
public class LmuTrackProviderGenerator : ICatalogGenerator<TrackInfo>
{
    private readonly IDataPathProvider dataPathProvider;
    private readonly DataManagerOptions options;

    public LmuTrackProviderGenerator(IDataPathProvider dataPathProvider, DataManagerOptions options)
    {
        this.dataPathProvider = dataPathProvider;
        this.options = options;
    }

    public string Generate(IReadOnlyCollection<TrackInfo> entities)
    {
        var coreRoot = this.dataPathProvider.GetProjectFolder(this.options.CoreProjectName);

        File.WriteAllText(Path.Combine(coreRoot, "Models", "LmuTrackInfo.cs"), BuildModelSource());
        File.WriteAllText(Path.Combine(coreRoot, "Models", "LmuTrackLayoutInfo.cs"), BuildLayoutModelSource());
        File.WriteAllText(Path.Combine(coreRoot, "Abstractions", "ILmuTrackInfoProvider.cs"), BuildInterfaceSource());

        var providerPath = Path.Combine(coreRoot, "Services", "LmuTrackInfoProvider.cs");
        File.WriteAllText(providerPath, BuildProviderSource(entities));
        return providerPath;
    }

    private static string BuildEntryLine(TrackInfo track)
    {
        var layoutEntries = track.Layouts.Select(layout => "new() { "
                                                             + $"Corners = {layout.Corners}, "
                                                             + $"LengthM = {layout.LengthM}, "
                                                             + $"Name = {Text(layout.Name)}, "
                                                             + $"RawTrackNames = {TextArray(layout.RawTrackNames)} "
                                                             + "}");

        return "        new() { "
               + $"Country = {Text(track.Country)}, "
               + $"CountryCode = {Text(track.CountryCode)}, "
               + $"Latitude = {Number(track.Latitude)}, "
               + $"Layouts = [{string.Join(", ", layoutEntries)}], "
               + $"Longitude = {Number(track.Longitude)}, "
               + $"Name = {Text(track.Name)}, "
               + $"ShortName = {Text(track.ShortName)} "
               + "},";
    }

    private static string BuildInterfaceSource()
    {
        return """
               using System.Collections.ObjectModel;
               using SimRacingSdk.Lmu.Core.Models;

               namespace SimRacingSdk.Lmu.Core.Abstractions;

               public interface ILmuTrackInfoProvider
               {
                   LmuTrackInfo? FindTrackByShortName(string shortName);
                   LmuTrackLayoutInfo? FindLayoutByRawTrackName(string rawTrackName);
                   (LmuTrackInfo Track, LmuTrackLayoutInfo Layout)? FindTrackAndLayoutByRawTrackName(string rawTrackName);
                   LmuTrackInfo? GeTrackInfoByVenue(string venue);
                   IReadOnlyCollection<LmuTrackInfo> GetTrackInfos();
                   ReadOnlyCollection<string> GetTrackNames();
               }

               """;
    }

    private static string BuildLayoutModelSource()
    {
        return """
               #nullable disable

               namespace SimRacingSdk.Lmu.Core.Models;

               public record LmuTrackLayoutInfo
               {
                   public double LengthKm => this.LengthM / 1000.0;
                   public double LengthMi => this.LengthM / 1609.34;
                   public int Corners { get; init; }
                   public int LengthM { get; init; }
                   public string Name { get; init; }

                   // The exact track names LMU reports for this layout, where known.
                   public IReadOnlyList<string> RawTrackNames { get; init; } = [];
               }

               """;
    }

    private static string BuildModelSource()
    {
        return """
               #nullable disable

               namespace SimRacingSdk.Lmu.Core.Models;

               public record LmuTrackInfo
               {
                   public string Country { get; init; }
                   public string CountryCode { get; init; }
                   public double Latitude { get; init; }
                   public IList<LmuTrackLayoutInfo> Layouts { get; init; }
                   public double Longitude { get; init; }
                   public string Name { get; init; }
                   public string ShortName { get; init; }
               }

               """;
    }

    private static string BuildProviderSource(IEnumerable<TrackInfo> tracks)
    {
        var entryLines = tracks.OrderBy(track => track.Name).Select(BuildEntryLine);

        return $$"""
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
                 {{string.Join(Environment.NewLine, entryLines)}}
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

                 """;
    }
}
