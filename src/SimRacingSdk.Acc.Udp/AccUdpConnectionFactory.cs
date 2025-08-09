using SimRacingSdk.Acc.Udp.Abstractions;

namespace SimRacingSdk.Acc.Udp;

public class AccUdpConnectionFactory : IAccUdpConnectionFactory
{
    private static AccUdpConnectionFactory? singletonInstance;

    public static AccUdpConnectionFactory Instance => singletonInstance ??= new AccUdpConnectionFactory();

    public IAccUdpConnection Create(string ipAddress,
        int port,
        string displayName,
        string connectionPassword,
        string commandPassword,
        int updateInterval)
    {
        return new AccUdpConnection(ipAddress,
            port,
            displayName,
            connectionPassword,
            commandPassword,
            updateInterval);
    }
}