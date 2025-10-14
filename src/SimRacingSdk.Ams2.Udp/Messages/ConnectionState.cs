#nullable disable

namespace SimRacingSdk.Ams2.Udp.Messages;

public record ConnectionState(bool IsConnected, string ConnectionId)
{
}