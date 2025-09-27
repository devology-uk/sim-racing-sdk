namespace SimRacingSdk.Ams2.Udp.Abstractions;

public interface IAms2UdpConnectionFactory
{
    IAms2UdpConnection Create(string ipAddress, int port);
}