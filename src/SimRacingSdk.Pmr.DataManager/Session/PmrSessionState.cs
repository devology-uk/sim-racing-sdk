using SimRacingSdk.DataManager.Shared.Session;

namespace SimRacingSdk.Pmr.DataManager.Session;

// Adds the Setup Maps page's place to the catalog pages' shared session state.
public class PmrSessionState : SessionState
{
    public string? SetupMapsCarId { get; set; }
    public int SetupMapsTabIndex { get; set; }
}
