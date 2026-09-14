namespace SimRacingSdk.Pmr.Demo.Abstractions;

public interface IUdpDemo : IDemo
{
    // Called before Start() - the SDK connection classes already take an explicit port (no
    // hardcoded assumption of owning the game's own configured port), but this is what lets the
    // Demo's UI override what PmrLocalConfigProvider read from the game's settings file. Needed
    // because many sim racers run several UDP-consuming apps (SimHub, Fanatec App, CrewChief)
    // and only one process can bind the game's actual configured port - a relay tool forwards to
    // a different port for everyone else, which this demo needs to be able to target too.
    void Configure(string host, int port);
}
