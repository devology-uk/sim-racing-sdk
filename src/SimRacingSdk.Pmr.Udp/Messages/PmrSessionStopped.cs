namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrSessionStopped
{
    public ushort PacketVersion { get; init; }
}
