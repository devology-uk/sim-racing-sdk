using System.IO;
using SimRacingSdk.DataManager.Shared;
using SimRacingSdk.DataManager.Shared.Catalog;
using SimRacingSdk.DataManager.Shared.Generation;
using SimRacingSdk.DataManager.Shared.Storage;
using static SimRacingSdk.DataManager.Shared.Generation.CSharpLiteral;

namespace SimRacingSdk.Ams2.DataManager.Tracks;

// Regenerates SimRacingSdk.Ams2.Core's whole track-catalog surface - both models, the interface and
// the provider - from the tracks captured here.
public class Ams2TrackProviderGenerator : ICatalogGenerator<TrackInfo>
{
    private readonly IDataPathProvider dataPathProvider;
    private readonly DataManagerOptions options;

    public Ams2TrackProviderGenerator(IDataPathProvider dataPathProvider, DataManagerOptions options)
    {
        this.dataPathProvider = dataPathProvider;
        this.options = options;
    }

    public string Generate(IReadOnlyCollection<TrackInfo> entities)
    {
        var coreRoot = this.dataPathProvider.GetProjectFolder(this.options.CoreProjectName);

        GeneratedSource.Write(Path.Combine(coreRoot, "Models", "Ams2TrackInfo.cs"), BuildModelSource());
        GeneratedSource.Write(Path.Combine(coreRoot, "Models", "Ams2TrackLayoutInfo.cs"), BuildLayoutModelSource());
        GeneratedSource.Write(Path.Combine(coreRoot, "Abstractions", "IAms2TrackInfoProvider.cs"), BuildInterfaceSource());

        var providerPath = Path.Combine(coreRoot, "Ams2TrackInfoProvider.cs");
        GeneratedSource.Write(providerPath, BuildProviderSource(entities));
        return providerPath;
    }

    private static string BuildEntryLine(TrackInfo track)
    {
        var layoutEntries = track.Layouts.Select(BuildLayoutEntry);

        return "        new() { "
               + $"AltitudeM = {track.AltitudeM}, "
               + $"Country = {Text(track.Country)}, "
               + $"CountryCode = {Text(track.CountryCode)}, "
               + $"FullName = {Text(track.FullName)}, "
               + $"Latitude = {Number(track.Latitude)}, "
               + $"Layouts = [{string.Join(", ", layoutEntries)}], "
               + $"Longitude = {Number(track.Longitude)}, "
               + $"RawTrackLocations = {TextArray(track.RawTrackLocations)}, "
               + $"ShortName = {Text(track.ShortName)} "
               + "},";
    }

    private static string BuildLayoutEntry(TrackLayoutInfo layout)
    {
        var assignments = new List<string>();
        if(layout.Ams2TrackId is not null)
        {
            assignments.Add($"Ams2TrackId = {Text(layout.Ams2TrackId)}");
        }

        assignments.Add($"Corners = {layout.Corners}");
        if(layout.DlcPack is not null)
        {
            assignments.Add($"DlcPack = {Text(layout.DlcPack)}");
        }

        assignments.Add($"Grade = {Text(layout.Grade)}");
        assignments.Add($"LengthM = {layout.LengthM}");
        assignments.Add($"MaxGridSize = {layout.MaxGridSize}");
        assignments.Add($"Name = {Text(layout.Name)}");
        assignments.Add($"TrackType = {Text(layout.TrackType)}");

        return $"new() {{ {string.Join(", ", assignments)} }}";
    }

    private static string BuildInterfaceSource()
    {
        return """
               using System.Collections.ObjectModel;
               using SimRacingSdk.Ams2.Core.Models;

               namespace SimRacingSdk.Ams2.Core.Abstractions;

               public interface IAms2TrackInfoProvider
               {
                   // Takes the track location AMS2's shared memory reports, as well as a ShortName.
                   Ams2TrackInfo? FindTrackByShortName(string shortName);
                   ReadOnlyCollection<Ams2TrackInfo> GetTrackInfos();
                   ReadOnlyCollection<string> GetTrackNames();
               }

               """;
    }

    private static string BuildLayoutModelSource()
    {
        return """
               #nullable disable

               namespace SimRacingSdk.Ams2.Core.Models;

               public record Ams2TrackLayoutInfo
               {
                   private string ams2TrackId;
                   public double LengthKm => this.LengthM / 1000.0;
                   public double LengthMi => this.LengthM / 1609.34;
                   public int Corners { get; init; }
                   public string DlcPack { get; init; }
                   public string Grade { get; init; }
                   public int LengthM { get; init; }
                   public int MaxGridSize { get; init; }
                   public string Name { get; init; }
                   public string TrackType { get; init; }

                   // The track variation AMS2's shared memory reports.
                   public string Ams2TrackId
                   {
                       get => this.ams2TrackId ?? this.Name.Replace(" ", "_");
                       set => this.ams2TrackId = value;
                   }
               }

               """;
    }

    private static string BuildModelSource()
    {
        return """
               #nullable disable

               namespace SimRacingSdk.Ams2.Core.Models;

               public record Ams2TrackInfo
               {
                   public int AltitudeM { get; init; }
                   public string Country { get; init; }
                   public string CountryCode { get; init; }
                   public string FullName { get; init; }
                   public double Latitude { get; init; }
                   public IList<Ams2TrackLayoutInfo> Layouts { get; init; }
                   public double Longitude { get; init; }

                   // Track locations AMS2 reports that don't reduce to ShortName, e.g. "Nurburgring_2025".
                   public IReadOnlyList<string> RawTrackLocations { get; init; } = [];

                   public string ShortName { get; init; }
               }

               """;
    }

    private static string BuildProviderSource(IEnumerable<TrackInfo> tracks)
    {
        var entryLines = tracks.OrderBy(track => track.ShortName).Select(BuildEntryLine);

        return $$"""
                 using System.Collections.ObjectModel;
                 using System.Globalization;
                 using System.Text;
                 using SimRacingSdk.Ams2.Core.Abstractions;
                 using SimRacingSdk.Ams2.Core.Models;

                 namespace SimRacingSdk.Ams2.Core;

                 public class Ams2TrackInfoProvider : IAms2TrackInfoProvider
                 {
                     private static Ams2TrackInfoProvider? singletonInstance;

                     private readonly List<Ams2TrackInfo> tracks =
                     [
                 {{string.Join(Environment.NewLine, entryLines)}}
                     ];

                     public static Ams2TrackInfoProvider Instance => singletonInstance ??= new Ams2TrackInfoProvider();

                     public Ams2TrackInfo? FindTrackByShortName(string shortName)
                     {
                         var rawLocationMatch = this.tracks.FirstOrDefault(
                             t => t.RawTrackLocations.Contains(shortName, StringComparer.InvariantCultureIgnoreCase));
                         if(rawLocationMatch != null)
                         {
                             return rawLocationMatch;
                         }

                         var normalizedShortName = Normalize(shortName);
                         return this.tracks.FirstOrDefault(t => Normalize(t.ShortName) == normalizedShortName);
                     }

                     public ReadOnlyCollection<Ams2TrackInfo> GetTrackInfos()
                     {
                         return this.tracks.AsReadOnly();
                     }

                     public ReadOnlyCollection<string> GetTrackNames()
                     {
                         return this.tracks.Select(t => t.ShortName)
                                    .ToList()
                                    .AsReadOnly();
                     }

                     // Drops accents as well as spaces and punctuation, so "Nurburgring" from shared memory matches
                     // the catalog's "Nürburgring".
                     private static string Normalize(string value)
                     {
                         var withoutAccents = value.Normalize(NormalizationForm.FormD)
                             .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
                         return new string(withoutAccents.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
                     }
                 }

                 """;
    }
}
