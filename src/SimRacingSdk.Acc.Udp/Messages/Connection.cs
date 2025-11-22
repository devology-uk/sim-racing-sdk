#nullable disable

namespace SimRacingSdk.Acc.Udp.Messages;

public record Connection(
    int ConnectionId,
    bool IsConnected,
    bool IsReadOnly,
    string Error = null) { }