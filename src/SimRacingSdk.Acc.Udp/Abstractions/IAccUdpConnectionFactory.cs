namespace SimRacingSdk.Acc.Udp.Abstractions;

public interface IAccUdpConnectionFactory
{
    IAccUdpConnection Create(string ipAddress,
        int port,
        string displayName,
        string connectionPassword,
        string commandPassword,
        int updateInterval);
}