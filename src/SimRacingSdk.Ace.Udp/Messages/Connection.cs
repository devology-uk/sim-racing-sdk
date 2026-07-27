#nullable disable

namespace SimRacingSdk.Ace.Udp.Messages;

public record Connection(
    int ConnectionId,
    bool IsConnected,
    bool IsReadOnly,
    string Error = null) { }
