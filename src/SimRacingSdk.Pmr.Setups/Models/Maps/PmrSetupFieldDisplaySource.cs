namespace SimRacingSdk.Pmr.Setups.Models.Maps;

public enum PmrSetupFieldDisplaySource
{
    // The on-screen value follows exactly from the saved setup value.
    Setup,

    // The game measures the on-screen value off the settled car in its physics simulation and never
    // saves it (Camber, Toe-in), so it can't be reproduced outside the game.
    GameSimulation
}
