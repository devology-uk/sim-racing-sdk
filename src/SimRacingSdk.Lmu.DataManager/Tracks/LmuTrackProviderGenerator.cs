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
                                                             + $"Name = {Text(layout.Name)} "
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
                 using System.Collections.ObjectModel;
                 using SimRacingSdk.Lmu.Core.Abstractions;
                 using SimRacingSdk.Lmu.Core.Models;

                 namespace SimRacingSdk.Lmu.Core.Services;

                 public class LmuTrackInfoProvider : ILmuTrackInfoProvider
                 {
                     private static LmuTrackInfoProvider? singletonInstance;

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
                         if(string.IsNullOrWhiteSpace(rawTrackName))
                         {
                             return null;
                         }

                         foreach(var track in this.tracks)
                         {
                             var layoutNameMatch = track.Layouts
                                 .Where(layout => layout.Name != "Default")
                                 .FirstOrDefault(layout => rawTrackName.Contains(layout.Name, StringComparison.InvariantCultureIgnoreCase));
                             if(layoutNameMatch != null)
                             {
                                 return (track, layoutNameMatch);
                             }
                         }

                         var venueTrack = this.GeTrackInfoByVenue(rawTrackName)
                             ?? this.tracks.FirstOrDefault(t => t.ShortName.Equals(rawTrackName, StringComparison.InvariantCultureIgnoreCase))
                             ?? this.tracks.FirstOrDefault(t => rawTrackName.Contains(t.ShortName, StringComparison.InvariantCultureIgnoreCase));

                         if(venueTrack == null)
                         {
                             return null;
                         }

                         var layout = venueTrack.Layouts.FirstOrDefault(l => l.Name == "Default") ?? venueTrack.Layouts.FirstOrDefault();
                         return layout == null ? null : (venueTrack, layout);
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
                 }

                 """;
    }
}
