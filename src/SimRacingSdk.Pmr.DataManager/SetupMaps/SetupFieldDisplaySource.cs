namespace SimRacingSdk.Pmr.DataManager.SetupMaps;

public enum SetupFieldDisplaySource
{
    // The on-screen value follows exactly from the saved setup value.
    Setup,

    // The game measures the on-screen value off the settled car in its physics simulation (ride
    // height, bump stops, fuel and driver weight all move it), and never saves it. A consumer can't
    // reproduce it, so it must say so rather than show a near-miss; how far the setting has been
    // adjusted (clicks from minimum, from the raw value) is still exact.
    GameSimulation
}
