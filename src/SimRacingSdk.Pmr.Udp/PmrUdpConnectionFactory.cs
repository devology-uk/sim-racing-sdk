using SimRacingSdk.Pmr.Udp.Abstractions;

namespace SimRacingSdk.Pmr.Udp;

public class PmrUdpConnectionFactory : IPmrUdpConnectionFactory
{
    private static PmrUdpConnectionFactory? singletonInstance;

    public static PmrUdpConnectionFactory Instance => singletonInstance ??= new PmrUdpConnectionFactory();

    public IPmrUdpConnection Create(int port = PmrUdpConnection.DefaultPort,
        bool useMulticast = false,
        string multicastGroup = PmrUdpConnection.DefaultMulticastGroup)
    {
        return new PmrUdpConnection(port, useMulticast, multicastGroup);
    }
}
