namespace SimRacingSdk.Ams2.Udp.Messages;

public record MessageHeader
{
    public int CategoryPacketNumber { get; init; }
    public InboundMessageType PacketType { get; init; }
    public int PacketNumber { get; init; }
    public byte PacketVersion { get; init; }
    public byte PartialPacketIndex { get; init; }
    public byte PartialPacketNumber { get; init; }
}