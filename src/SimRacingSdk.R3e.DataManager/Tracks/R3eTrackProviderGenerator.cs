using System.IO;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Generation;
using SimRacingSdk.DataManager.Shared.Storage;
using static SimRacingSdk.DataManager.Shared.Generation.CSharpLiteral;

namespace SimRacingSdk.R3e.DataManager.Tracks;

// Regenerates SimRacingSdk.R3e.Core's whole track-catalog surface - both models, the interface and
// the provider - from the tracks captured here.
public class R3eTrackProviderGenerator : ICatalogGenerator<TrackInfo>
{
    private readonly IDataPathProvider dataPathProvider;
    private readonly DataManagerOptions options;

    public R3eTrackProviderGenerator(IDataPathProvider dataPathProvider, DataManagerOptions options)
    {
        this.dataPathProvider = dataPathProvider;
        this.options = options;
    }

    public string Generate(IReadOnlyCollection<TrackInfo> entities)
    {
        var coreRoot = this.dataPathProvider.GetProjectFolder(this.options.CoreProjectName);

        GeneratedSource.Write(Path.Combine(coreRoot, "Models", "R3eTrackInfo.cs"), BuildModelSource());
        GeneratedSource.Write(Path.Combine(coreRoot, "Models", "R3eTrackLayoutInfo.cs"), BuildLayoutModelSource());
        GeneratedSource.Write(Path.Combine(coreRoot, "Abstractions", "IR3eTrackInfoProvider.cs"), BuildInterfaceSource());

        var providerPath = Path.Combine(coreRoot, "R3eTrackInfoProvider.cs");
        GeneratedSource.Write(providerPath, BuildProviderSource(entities));
        return providerPath;
    }

    private static string BuildEntryLine(TrackInfo track)
    {
        var layoutEntries = track.Layouts.Select(layout => "new() { "
                                                             + $"GameId = {layout.GameId}, "
                                                             + $"GridSize = {layout.GridSize}, "
                                                             + $"LengthKm = {NullableNumber(layout.LengthKm)}, "
                                                             + $"Name = {Text(layout.Name)} "
                                                             + "}");

        return "        new() { "
               + $"Country = {Text(track.Country)}, "
               + $"CountryCode = {Text(track.CountryCode)}, "
               + $"GameId = {track.GameId}, "
               + $"Latitude = {NullableNumber(track.Latitude)}, "
               + $"Layouts = [{string.Join(", ", layoutEntries)}], "
               + $"Longitude = {NullableNumber(track.Longitude)}, "
               + $"Name = {Text(track.Name)}, "
               + $"NotInGameSelector = {(track.NotInGameSelector ? "true" : "false")} "
               + "},";
    }

    private static string BuildInterfaceSource()
    {
        return """
               using SimRacingSdk.R3e.Core.Models;

               namespace SimRacingSdk.R3e.Core.Abstractions;

               public interface IR3eTrackInfoProvider
               {
                   R3eTrackInfo? FindByGameId(int trackGameId);

                   /// <summary>
                   ///     Layout ids are unique across every track, so a layout id alone (shared memory's LayoutId)
                   ///     identifies both.
                   /// </summary>
                   (R3eTrackInfo Track, R3eTrackLayoutInfo Layout)? FindByLayoutGameId(int layoutGameId);

                   R3eTrackInfo? FindByName(string name);
                   IReadOnlyCollection<R3eTrackInfo> GetTrackInfos();
               }

               """;
    }

    private static string BuildLayoutModelSource()
    {
        return """
               #nullable disable

               namespace SimRacingSdk.R3e.Core.Models;

               public record R3eTrackLayoutInfo
               {
                   // The game's own layout id, as shared memory reports it.
                   public int GameId { get; init; }
                   public int GridSize { get; init; }
                   public double? LengthKm { get; init; }
                   public string Name { get; init; }
               }

               """;
    }

    private static string BuildModelSource()
    {
        return """
               #nullable disable

               namespace SimRacingSdk.R3e.Core.Models;

               public record R3eTrackInfo
               {
                   public string Country { get; init; }
                   public string CountryCode { get; init; }
                   // The game's own track id, as shared memory reports it.
                   public int GameId { get; init; }
                   public double? Latitude { get; init; }
                   public IList<R3eTrackLayoutInfo> Layouts { get; init; }
                   public double? Longitude { get; init; }
                   public string Name { get; init; }
                   // Known to the game but not offered in its track selector.
                   public bool NotInGameSelector { get; init; }
               }

               """;
    }

    private static string BuildProviderSource(IEnumerable<TrackInfo> tracks)
    {
        var entryLines = tracks.OrderBy(track => track.Name).Select(BuildEntryLine);

        return $$"""
                 using SimRacingSdk.R3e.Core.Abstractions;
                 using SimRacingSdk.R3e.Core.Models;

                 namespace SimRacingSdk.R3e.Core;

                 public class R3eTrackInfoProvider : IR3eTrackInfoProvider
                 {
                     private static R3eTrackInfoProvider? singletonInstance;

                     private readonly List<R3eTrackInfo> tracks =
                     [
                 {{string.Join(Environment.NewLine, entryLines)}}
                     ];

                     public static R3eTrackInfoProvider Instance => singletonInstance ??= new R3eTrackInfoProvider();

                     public R3eTrackInfo? FindByGameId(int trackGameId)
                     {
                         return this.tracks.FirstOrDefault(track => track.GameId == trackGameId);
                     }

                     public (R3eTrackInfo Track, R3eTrackLayoutInfo Layout)? FindByLayoutGameId(int layoutGameId)
                     {
                         return this.tracks.SelectMany(track => track.Layouts.Select(layout => (Track: track, Layout: layout)))
                                    .Where(match => match.Layout.GameId == layoutGameId)
                                    .Select(match => ((R3eTrackInfo Track, R3eTrackLayoutInfo Layout)?)match)
                                    .FirstOrDefault();
                     }

                     public R3eTrackInfo? FindByName(string name)
                     {
                         return this.tracks.FirstOrDefault(track => track.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                     }

                     public IReadOnlyCollection<R3eTrackInfo> GetTrackInfos()
                     {
                         return this.tracks.AsReadOnly();
                     }
                 }

                 """;
    }
}
