using SimRacingSdk.DataManager.Shared.Session;

namespace SimRacingSdk.R3e.DataManager.Session;

// Adds the Setup Maps page's place to the catalog pages' shared session state.
public class R3eSessionState : SessionState
{
    public int? SetupMapsCarGameId { get; set; }
    public string? SetupMapsFilter { get; set; }
    public string? SetupMapsSearchText { get; set; }
}
