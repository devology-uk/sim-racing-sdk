namespace SimRacingSdk.Lmu.Rest.Models;

public enum LmuRestEndpointKind
{
    // A GET with no route values that changes nothing, so it is safe to call in bulk.
    SafeRead,
    // A GET that reads the player's car. It crashes LMU's REST server outside a live session (seen in the main
    // menu on 2026-10-19 and 2026-10-24, and with a replay loaded on 2026-10-24), so only call it while
    // watch/sessionInfo reports inRealtime.
    RealtimeRead,
    ParameterisedRead,
    // Changes what the game shows (camera, replay, HUD); only ever called one at a time on request.
    Action
}
