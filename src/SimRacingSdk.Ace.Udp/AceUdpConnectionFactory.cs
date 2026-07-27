using SimRacingSdk.Ace.Udp.Abstractions;

namespace SimRacingSdk.Ace.Udp;

public class AceUdpConnectionFactory : IAceUdpConnectionFactory
{
    private static AceUdpConnectionFactory? singletonInstance;

    public static AceUdpConnectionFactory Instance => singletonInstance ??= new AceUdpConnectionFactory();

    public IAceUdpConnection Create(string ipAddress,
        int port,
        string displayName,
        string connectionPassword,
        string commandPassword,
        int updateInterval = 100)
    {
        return new AceUdpConnection(ipAddress,
            port,
            displayName,
            connectionPassword,
            commandPassword,
            updateInterval);
    }
}
