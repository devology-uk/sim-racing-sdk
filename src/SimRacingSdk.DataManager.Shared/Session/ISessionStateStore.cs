namespace SimRacingSdk.DataManager.Shared.Session;

public interface ISessionStateStore
{
    SessionState State { get; }
    void Save();
}

public interface ISessionStateStore<out TState> : ISessionStateStore where TState : SessionState
{
    new TState State { get; }
}
