using SimRacingSdk.Ams2.Udp.Abstractions;

namespace SimRacingSdk.Ams2.Udp;

public class Ams2UdpConnectionFactory : IAms2UdpConnectionFactory
{
    private static Ams2UdpConnectionFactory? singletonInstance;

    public static Ams2UdpConnectionFactory Instance => singletonInstance ??= new Ams2UdpConnectionFactory();

    public IAms2UdpConnection Create(string ipAddress, int port)
    {
        return new Ams2UdpConnection(ipAddress, port);
    }
}