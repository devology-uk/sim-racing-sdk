#nullable disable

namespace SimRacingSdk.Acc.Udp.Messages;

public record ConnectionState
{
    public ConnectionState(int connectionId, bool isConnected, bool isReadOnly, string error = null)
    {
        this.ConnectionId = connectionId;
        this.IsConnected = isConnected;
        this.IsReadOnly = isReadOnly;
        this.Error = error;
    }

    public int ConnectionId { get; init; }
    public string Error { get; init; }
    public bool IsConnected { get; init; }
    public bool IsReadOnly { get; init; }
}