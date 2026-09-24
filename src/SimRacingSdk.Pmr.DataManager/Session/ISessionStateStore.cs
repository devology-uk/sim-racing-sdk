namespace SimRacingSdk.Pmr.DataManager.Session;

public interface ISessionStateStore
{
    SessionState State { get; }
    void Save();
}
