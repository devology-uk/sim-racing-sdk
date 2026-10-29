using SimRacingSdk.DataManager.Shared.Storage;
using SimRacingSdk.R3e.DataManager.GameData;

namespace SimRacingSdk.R3e.DataManager.Tracks;

public class TrackCatalogImporter : ICatalogImporter<TrackInfo>
{
    private readonly IGameCatalogReader gameCatalogReader;
    private readonly IEntityRepository<TrackInfo> repository;

    public TrackCatalogImporter(IGameCatalogReader gameCatalogReader, IEntityRepository<TrackInfo> repository)
    {
        this.gameCatalogReader = gameCatalogReader;
        this.repository = repository;
    }

    public ImportResult Import()
    {
        var gameTracks = this.gameCatalogReader.Read().Tracks;
        var capturedTracks = this.repository.GetAll().ToDictionary(track => track.GameId);
        var merge = new CatalogMerge<TrackInfo>(this.repository);

        foreach(var gameTrack in gameTracks)
        {
            var capturedTrack = capturedTracks.GetValueOrDefault(gameTrack.GameId);
            merge.Apply(capturedTrack, this.Merge(capturedTrack, gameTrack));
        }

        var gameIds = gameTracks.Select(track => track.GameId).ToHashSet();
        return merge.Result(capturedTracks.Keys.Count(gameId => !gameIds.Contains(gameId)));
    }

    private TrackInfo Merge(TrackInfo? capturedTrack, GameTrack gameTrack)
    {
        var capturedLayouts = capturedTrack?.Layouts ?? [];
        var layouts = gameTrack.Layouts.Select(gameLayout => this.Merge(capturedLayouts, gameLayout))
                               .Concat(this.LayoutsNoLongerInGame(capturedLayouts, gameTrack))
                               .ToList();

        if(capturedTrack is null)
        {
            return new TrackInfo
            {
                GameId = gameTrack.GameId,
                Layouts = layouts,
                Name = gameTrack.Name
            };
        }

        return capturedTrack with
        {
            Layouts = layouts,
            Name = gameTrack.Name
        };
    }

    private TrackLayoutInfo Merge(IEnumerable<TrackLayoutInfo> capturedLayouts, GameLayout gameLayout)
    {
        var capturedLayout = capturedLayouts.FirstOrDefault(layout => layout.GameId == gameLayout.GameId);
        if(capturedLayout is null)
        {
            return new TrackLayoutInfo
            {
                GameId = gameLayout.GameId,
                GridSize = gameLayout.GridSize,
                Name = gameLayout.Name
            };
        }

        return capturedLayout with
        {
            GridSize = gameLayout.GridSize,
            Name = gameLayout.Name
        };
    }

    // Kept rather than dropped, so curated values survive a layout the game stops listing.
    private IEnumerable<TrackLayoutInfo> LayoutsNoLongerInGame(IEnumerable<TrackLayoutInfo> capturedLayouts,
        GameTrack gameTrack)
    {
        return capturedLayouts.Where(layout => gameTrack.Layouts.All(gameLayout => gameLayout.GameId != layout.GameId));
    }
}
