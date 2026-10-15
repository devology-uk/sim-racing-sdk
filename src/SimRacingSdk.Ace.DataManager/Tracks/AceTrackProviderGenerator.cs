using System.IO;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Generation;
using SimRacingSdk.DataManager.Shared.Storage;
using static SimRacingSdk.DataManager.Shared.Generation.CSharpLiteral;

namespace SimRacingSdk.Ace.DataManager.Tracks;

// Regenerates SimRacingSdk.Ace.Core's whole track-catalog surface - both models, the interface and
// the provider - from the tracks captured here.
public class AceTrackProviderGenerator : ICatalogGenerator<TrackInfo>
{
    private readonly IDataPathProvider dataPathProvider;
    private readonly DataManagerOptions options;

    public AceTrackProviderGenerator(IDataPathProvider dataPathProvider, DataManagerOptions options)
    {
        this.dataPathProvider = dataPathProvider;
        this.options = options;
    }

    public string Generate(IReadOnlyCollection<TrackInfo> entities)
    {
        var coreRoot = this.dataPathProvider.GetProjectFolder(this.options.CoreProjectName);

        GeneratedSource.Write(Path.Combine(coreRoot, "Models", "AceTrackInfo.cs"), BuildModelSource());
        GeneratedSource.Write(Path.Combine(coreRoot, "Models", "AceTrackLayoutInfo.cs"), BuildLayoutModelSource());
        GeneratedSource.Write(Path.Combine(coreRoot, "Abstractions", "IAceTrackInfoProvider.cs"), BuildInterfaceSource());

        var providerPath = Path.Combine(coreRoot, "AceTrackInfoProvider.cs");
        GeneratedSource.Write(providerPath, BuildProviderSource(entities));
        return providerPath;
    }

    private static string BuildEntryLine(TrackInfo track)
    {
        var layoutEntries = track.Layouts.Select(layout => "new() { "
                                                             + $"Corners = {layout.Corners}, "
                                                             + $"LengthMeters = {layout.LengthMeters}, "
                                                             + $"MaxPitSlot = {layout.MaxPitSlot}, "
                                                             + $"Name = {Text(layout.Name)} "
                                                             + "}");

        return "        new() { "
               + $"Continent = {Text(track.Continent)}, "
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

               """;
    }

    private static string BuildLayoutModelSource()
    {
        return """
               #nullable disable

               namespace SimRacingSdk.Ace.Core.Models;

               public record AceTrackLayoutInfo
               {
                   public int Corners { get; init; }
                   public int LengthMeters { get; init; }
                   public int MaxPitSlot { get; init; }
                   public string Name { get; init; }
               }

               """;
    }

    private static string BuildModelSource()
    {
        return """
               #nullable disable

               namespace SimRacingSdk.Ace.Core.Models;

               public record AceTrackInfo
               {
                   public string Continent { get; init; }
                   public string CountryCode { get; init; }
                   public double Latitude { get; init; }
                   public IList<AceTrackLayoutInfo> Layouts { get; init; }
                   public double Longitude { get; init; }

                   // The track name ACE's shared memory reports, which is also its setup folder name.
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
                 using SimRacingSdk.Ace.Core.Abstractions;
                 using SimRacingSdk.Ace.Core.Models;

                 namespace SimRacingSdk.Ace.Core;

                 public class AceTrackInfoProvider : IAceTrackInfoProvider
                 {
                     private static AceTrackInfoProvider? singletonInstance;

                     private readonly List<AceTrackInfo> tracks =
                     [
                 {{string.Join(Environment.NewLine, entryLines)}}
                     ];

                     public static AceTrackInfoProvider Instance => singletonInstance ??= new AceTrackInfoProvider();

                     public AceTrackInfo? FindByName(string name)
                     {
                         return this.tracks.FirstOrDefault(t => t.Name == name);
                     }

                     public AceTrackInfo? FindByShortName(string shortName)
                     {
                         return this.tracks.FirstOrDefault(t => t.ShortName == shortName);
                     }

                     public ReadOnlyCollection<string> GetContinents()
                     {
                         return this.tracks.Select(t => t.Continent)
                                    .Distinct()
                                    .OrderBy(c => c)
                                    .ToList()
                                    .AsReadOnly();
                     }

                     public ReadOnlyCollection<AceTrackInfo> GetTrackInfos()
                     {
                         return this.tracks.AsReadOnly();
                     }

                     public ReadOnlyCollection<AceTrackInfo> GetTrackInfosForContinent(string continent)
                     {
                         return this.tracks.Where(t => t.Continent == continent)
                                    .ToList()
                                    .AsReadOnly();
                     }
                 }

                 """;
    }
}
