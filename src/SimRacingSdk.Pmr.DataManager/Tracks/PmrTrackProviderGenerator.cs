using System.Globalization;
using System.IO;
using SimRacingSdk.Pmr.DataManager.Storage;

namespace SimRacingSdk.Pmr.DataManager.Tracks;

// Regenerates SimRacingSdk.Pmr.Core's whole track-catalog surface from the tracks captured here -
// the model, layout model and interface as well as the provider, not just the provider's data, so
// nothing in Core has to pre-exist for a track to reach it (mirrors PmrCarProviderGenerator).
//
// DataManager's own TrackInfo/TrackLayoutInfo use the game's own vocabulary (AltitudeMeters,
// GridSlots) rather than Core's existing PmrTrackInfo/PmrTrackLayoutInfo names
// (AltitudeMetersAmsl, GridSize) - those Core names predate this generator, so BuildEntryLine maps
// between them rather than renaming either side.
//
// GameId writes empty for any track where it's still null here (FindByGameId returns null for
// those until it's looked up and filled in).
public class PmrTrackProviderGenerator : IPmrTrackProviderGenerator
{
    private readonly IDataPathProvider dataPathProvider;

    public PmrTrackProviderGenerator(IDataPathProvider dataPathProvider)
    {
        this.dataPathProvider = dataPathProvider;
    }

    public string Generate(IEnumerable<TrackInfo> tracks)
    {
        var coreRoot = Path.Combine(this.dataPathProvider.GetRepoRoot(), "src", "SimRacingSdk.Pmr.Core");
        var trackList = tracks.ToList();

        File.WriteAllText(Path.Combine(coreRoot, "Models", "PmrTrackInfo.cs"), BuildModelSource());
        File.WriteAllText(Path.Combine(coreRoot, "Models", "PmrTrackLayoutInfo.cs"), BuildLayoutModelSource());
        File.WriteAllText(Path.Combine(coreRoot, "Abstractions", "IPmrTrackInfoProvider.cs"), BuildInterfaceSource());

        var providerPath = Path.Combine(coreRoot, "PmrTrackInfoProvider.cs");
        File.WriteAllText(providerPath, BuildSource(trackList));
        return providerPath;
    }

    private static string BuildEntryLine(TrackInfo track)
    {
        var layoutEntries = track.Layouts.Select(layout => "new() { "
                                                             + $"GridSize = {layout.GridSlots}, "
                                                             + $"LengthMeters = {CsNumber(layout.LengthMeters)}, "
                                                             + $"Name = {CsString(layout.Name)}, "
                                                             + $"Turns = {layout.Turns} "
                                                             + "}");

        return "        new() { "
               + $"AltitudeMetersAmsl = {CsNumber(track.AltitudeMeters)}, "
               + $"Continent = {CsString(track.Continent)}, "
               + $"Country = {CsString(track.Country)}, "
               + $"CountryCode = {CsString(track.CountryCode)}, "
               + $"GameId = {CsString(track.GameId ?? string.Empty)}, "
               + $"Latitude = {CsNumber(track.Latitude)}, "
               + $"Layouts = [{string.Join(", ", layoutEntries)}], "
               + $"Longitude = {CsNumber(track.Longitude)}, "
               + $"Name = {CsString(track.Name)} "
               + "},";
    }

    private static string BuildInterfaceSource()
    {
        return """
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

               """;
    }

    private static string BuildLayoutModelSource()
    {
        return """
               #nullable disable

               namespace SimRacingSdk.Pmr.Core.Models;

               public record PmrTrackLayoutInfo
               {
                   public int GridSize { get; init; }
                   public double LengthMeters { get; init; }
                   public string Name { get; init; }
                   public int Turns { get; init; }
               }

               """;
    }

    private static string BuildModelSource()
    {
        return """
               #nullable disable

               namespace SimRacingSdk.Pmr.Core.Models;

               public record PmrTrackInfo
               {
                   public double AltitudeMetersAmsl { get; init; }
                   public string Continent { get; init; }
                   public string Country { get; init; }
                   public string CountryCode { get; init; }

                   // The game's own track id (e.g. "ID_SpaFrancorchamps") - shared by every layout, since the
                   // game has no id of its own for an individual layout.
                   public string GameId { get; init; }

                   public double Latitude { get; init; }
                   public IList<PmrTrackLayoutInfo> Layouts { get; init; }
                   public double Longitude { get; init; }
                   public string Name { get; init; }
               }

               """;
    }

    private static string BuildSource(IEnumerable<TrackInfo> tracks)
    {
        var entryLines = tracks.OrderBy(track => track.Name).Select(BuildEntryLine);

        return $$"""
                 using System.Collections.ObjectModel;
                 using SimRacingSdk.Pmr.Core.Abstractions;
                 using SimRacingSdk.Pmr.Core.Models;

                 namespace SimRacingSdk.Pmr.Core;

                 public class PmrTrackInfoProvider : IPmrTrackInfoProvider
                 {
                     private static PmrTrackInfoProvider? singletonInstance;

                     private readonly List<PmrTrackInfo> tracks =
                     [
                 {{string.Join(Environment.NewLine, entryLines)}}
                     ];

                     public static PmrTrackInfoProvider Instance => singletonInstance ??= new PmrTrackInfoProvider();

                     public PmrTrackInfo? FindByGameId(string gameId)
                     {
                         return this.tracks.FirstOrDefault(t => string.Equals(t.GameId, gameId, StringComparison.OrdinalIgnoreCase));
                     }

                     public PmrTrackInfo? FindByName(string name)
                     {
                         return this.tracks.FirstOrDefault(t => t.Name == name);
                     }

                     public ReadOnlyCollection<string> GetContinents()
                     {
                         return this.tracks.Select(t => t.Continent)
                                            .Distinct()
                                            .OrderBy(c => c)
                                            .ToList()
                                            .AsReadOnly();
                     }

                     public ReadOnlyCollection<PmrTrackInfo> GetTrackInfos()
                     {
                         return this.tracks.AsReadOnly();
                     }

                     public ReadOnlyCollection<PmrTrackInfo> GetTrackInfosForContinent(string continent)
                     {
                         return this.tracks.Where(t => t.Continent == continent)
                                            .OrderBy(t => t.Name)
                                            .ToList()
                                            .AsReadOnly();
                     }
                 }

                 """;
    }

    private static string CsNumber(double value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    private static string CsString(string value)
    {
        return $"\"{value.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";
    }
}
