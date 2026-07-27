namespace SimRacingSdk.Ace.Udp.Abstractions;

public interface IAceUdpConnectionFactory
{
    IAceUdpConnection Create(string ipAddress,
        int port,
        string displayName,
        string connectionPassword,
        string commandPassword,
        int updateInterval = 100);
}
