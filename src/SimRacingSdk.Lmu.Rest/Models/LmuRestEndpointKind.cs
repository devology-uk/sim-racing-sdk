namespace SimRacingSdk.Lmu.Rest.Models;

public enum LmuRestEndpointKind
{
    // A GET with no route values that changes nothing, so it is safe to call in bulk.
    SafeRead,
    ParameterisedRead,
    // Changes what the game shows (camera, replay, HUD); only ever called one at a time on request.
    Action
}
