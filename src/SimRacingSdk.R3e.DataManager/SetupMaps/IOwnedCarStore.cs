namespace SimRacingSdk.R3e.DataManager.SetupMaps;

// R3E sells cars one by one, so only the cars Mike owns can be mapped; this remembers which those are.
public interface IOwnedCarStore
{
    bool IsOwned(int carGameId);
    void SetOwned(int carGameId, bool isOwned);
}
