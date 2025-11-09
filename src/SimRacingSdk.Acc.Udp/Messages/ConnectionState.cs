#nullable disable

namespace SimRacingSdk.Acc.Udp.Messages;

public record ConnectionState(
    int ConnectionId,
    bool IsConnected,
    bool IsReadOnly,
    bool WasConnected = false,
    string Error = null) { }