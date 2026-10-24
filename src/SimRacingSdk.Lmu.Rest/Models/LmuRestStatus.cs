namespace SimRacingSdk.Lmu.Rest.Models;

// What the game is doing, read once per poll. Either part is null when LMU has nothing to say
// (main menu: empty session info, 503 game state).
public record LmuRestStatus(LmuRestSessionInfo? SessionInfo, LmuRestGameState? GameState)
{
    public bool IsInSession => this.GameState != null;

    public bool IsInRealtime => this.SessionInfo?.InRealtime == true;

    // getVehicleCondition crashes LMU's REST server in the main menu and with a replay loaded. GetGameState
    // looked fine during the replay crash, so inRealtime is the deciding flag and the rest only narrows it.
    public bool CanReadPlayerVehicle => this.IsInRealtime
                                        && this.GameState is { PlayerVehicleLoaded: true, IsReplayActive: false };

    public string Describe()
    {
        if(this.SessionInfo == null && this.GameState == null)
        {
            return "Not in a session";
        }

        return $"session {this.SessionInfo?.Session}, inRealtime {this.IsInRealtime}, "
               + $"phase {this.GameState?.GamePhase}, multiStint {this.GameState?.MultiStintState}, "
               + $"pit {this.GameState?.PitState}, inMonitor {this.GameState?.InMonitor}, "
               + $"replay {this.GameState?.IsReplayActive}, vehicleLoaded {this.GameState?.PlayerVehicleLoaded}, "
               + $"inControl {this.GameState?.InControlOfVehicle}";
    }
}
