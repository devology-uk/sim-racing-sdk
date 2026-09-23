using System.IO;
using System.Text.Json;
using SimRacingSdk.Pmr.DataManager.Storage;

namespace SimRacingSdk.Pmr.DataManager.Tracks;

public class TrackRepository : ITrackRepository
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    private readonly string tracksFolder;

    public TrackRepository(IDataPathProvider dataPathProvider)
    {
        this.tracksFolder = dataPathProvider.GetEntityFolder("tracks");
    }

    public void Delete(string id)
    {
        var path = this.PathFor(id);
        if(File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public IReadOnlyList<TrackInfo> GetAll()
    {
        return Directory.EnumerateFiles(this.tracksFolder, "*.json")
            .Select(path => JsonSerializer.Deserialize<TrackInfo>(File.ReadAllText(path))!)
            .ToList();
    }

    // Always writes to {track.Id}.json, derived fresh from the track's current Name - renaming it
    // leaves the old file behind rather than renaming it in place. Not handled yet since there's no
    // delete UI to trigger it; deal with it when that exists (mirrors CarRepository.Save).
    public void Save(TrackInfo track)
    {
        File.WriteAllText(this.PathFor(track.Id), JsonSerializer.Serialize(track, SerializerOptions));
    }

    private string PathFor(string id)
    {
        return Path.Combine(this.tracksFolder, $"{id}.json");
    }
}
