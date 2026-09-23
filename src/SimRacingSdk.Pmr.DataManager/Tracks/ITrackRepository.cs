namespace SimRacingSdk.Pmr.DataManager.Tracks;

public interface ITrackRepository
{
    void Delete(string id);
    IReadOnlyList<TrackInfo> GetAll();
    void Save(TrackInfo track);
}
