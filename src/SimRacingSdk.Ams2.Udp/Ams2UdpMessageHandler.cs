using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Ams2.Udp.Messages;

namespace SimRacingSdk.Ams2.Udp;

internal class Ams2UdpMessageHandler
{
    private readonly Subject<LogMessage> logMessagesSubject = new();

    internal Ams2UdpMessageHandler(string connectionIdentifier)
    {
        if(string.IsNullOrEmpty(connectionIdentifier))
        {
            throw new ArgumentException(
                "No connection identifier provided.  A unique identifier is required for managing connections. IP Address and Port is a good identifier e.g. 127.0.0.1:9000");
        }

        this.ConnectionIdentifier = connectionIdentifier;
    }

    public string ConnectionIdentifier { get; }

    internal IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();

    public void ProcessMessage(BinaryReader reader)
    {
        var header = this.ReadHeader(reader);
        switch(header.PacketType)
        {
            case InboundMessageType.Telemetry:
                break;
            case InboundMessageType.RaceInfo:
                break;
            case InboundMessageType.Participants:
                break;
            case InboundMessageType.Timings:
                break;
            case InboundMessageType.GameState:
                break;
            case InboundMessageType.TimeStats:
                break;
            case InboundMessageType.VehicleInfo:
                break;
            default:
                this.LogMessage(LoggingLevel.Warning, "Unknown message type");
                break;
        }
    }

    private MessageHeader ReadHeader(BinaryReader reader)
    {
        return new MessageHeader
        {
            PacketNumber = reader.ReadInt32(),
            CategoryPacketNumber = reader.ReadInt32(),
            PartialPacketIndex = reader.ReadByte(),
            PartialPacketNumber = reader.ReadByte(),
            PacketType = (InboundMessageType)reader.ReadByte(),
            PacketVersion = reader.ReadByte()
        };
    }

    internal void Disconnect(bool sendUnregister = true)
    {
    }

    internal void LogMessage(LoggingLevel loggingLevel, string message, object? data = null)
    {
        this.logMessagesSubject.OnNext(new LogMessage(loggingLevel, message, data));
    }
}