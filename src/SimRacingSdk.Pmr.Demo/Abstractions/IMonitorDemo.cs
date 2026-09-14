namespace SimRacingSdk.Pmr.Demo.Abstractions;

public interface IMonitorDemo : IDemo
{
    // Same Configure-before-Start pattern as IUdpDemo - see that interface's own comment for why.
    void Configure(string host, int port);
}
