using SimRacingSdk.Lmu.Rest.Models;

namespace SimRacingSdk.Lmu.Demo.RestExplorer;

// A folder name for the monitor's captures, worked out from what LMU reports instead of picked by hand.
public static class RestMonitorStateLabel
{
    public static string For(LmuRestStatus status)
    {
        if(status.GameState is not { } gameState)
        {
            return "Main menu";
        }

        if(gameState.IsReplayActive)
        {
            return "Replay";
        }

        if(status.IsInRealtime)
        {
            return gameState.InMonitor ? "Realtime in Monitor" : "On track";
        }

        return gameState.InMonitor ? "Monitor" : "In session, not realtime";
    }
}
