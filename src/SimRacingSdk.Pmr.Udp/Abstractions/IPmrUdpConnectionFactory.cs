namespace SimRacingSdk.Pmr.Udp.Abstractions;

public interface IPmrUdpConnectionFactory
{
    IPmrUdpConnection Create(int port, bool useMulticast, string multicastGroup);
}
