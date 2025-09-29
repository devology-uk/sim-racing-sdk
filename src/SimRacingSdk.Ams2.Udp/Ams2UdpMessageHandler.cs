using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Ams2.Udp.Enums;
using SimRacingSdk.Ams2.Udp.Messages;

namespace SimRacingSdk.Ams2.Udp;

internal class Ams2UdpMessageHandler
{
    private readonly Subject<GameStateUpdate> gameStateUpdatesSubject = new();
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

    public IObservable<GameStateUpdate> GameStateUpdates => this.gameStateUpdatesSubject.AsObservable();

    internal IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();

    public void ProcessMessage(BinaryReader reader)
    {
        var header = this.ReadHeader(reader);
        switch(header.PacketType)
        {
            case InboundMessageType.Telemetry:
                this.ProcessTelemetryMessage(reader, header);
                break;
            case InboundMessageType.RaceInfo:
                this.ProcessRaceInfoMessage(reader, header);
                break;
            case InboundMessageType.Participants:
                this.ProcessParticipantsMessage(reader, header);
                break;
            case InboundMessageType.Timings:
                this.ProcessTimingsMessage(reader, header);
                break;
            case InboundMessageType.GameState:
                this.ProcessGameStateMessage(reader, header);
                break;
            case InboundMessageType.TimeStats:
                this.ProcessTimeStatsMessage(reader, header);
                break;
            case InboundMessageType.VehicleInfo:
                this.ProcessVehicleInfoMessage(reader, header);
                break;
            default:
                this.LogMessage(LoggingLevel.Warning, "Unknown message type");
                break;
        }
    }

    internal void Disconnect(bool sendUnregister = true) { }

    internal void LogMessage(LoggingLevel loggingLevel, string message, object? data = null)
    {
        this.logMessagesSubject.OnNext(new LogMessage(loggingLevel, message, data));
    }

    private void ProcessGameStateMessage(BinaryReader reader, MessageHeader header) { }

    private void ProcessParticipantsMessage(BinaryReader reader, MessageHeader header)
    {
        var gameStateUpdate = new GameStateUpdate(header)
        {
            BuildVersionNumber = reader.ReadInt16()
        };

        var state = reader.ReadByte();
        gameStateUpdate.SessionState = ((state & 240) >> 4);
        gameStateUpdate.GameState = (state & 15);
        gameStateUpdate.AmbientTemperature = reader.ReadByte();
        gameStateUpdate.TrackTemperature = reader.ReadByte();
        gameStateUpdate.RainDensity = reader.ReadByte();
        gameStateUpdate.SnowDensity = reader.ReadByte();
        gameStateUpdate.WindSpeed = reader.ReadByte();
        gameStateUpdate.WindDirectionX = reader.ReadByte();
        gameStateUpdate.WindDirectionY = reader.ReadByte();

        this.gameStateUpdatesSubject.OnNext(gameStateUpdate);
    }

    private void ProcessRaceInfoMessage(BinaryReader reader, MessageHeader header) { }

    private void ProcessTelemetryMessage(BinaryReader reader, MessageHeader header) { }

    private void ProcessTimeStatsMessage(BinaryReader reader, MessageHeader header) { }

    private void ProcessTimingsMessage(BinaryReader reader, MessageHeader header) { }

    private void ProcessVehicleInfoMessage(BinaryReader reader, MessageHeader header) { }

    private MessageHeader ReadHeader(BinaryReader reader)
    {
        return new MessageHeader
        {
            PacketNumber = reader.ReadInt32(),
            CategoryPacketNumber = reader.ReadInt32(),
            PartialPacketIndex = reader.ReadByte(),
            PartialPacketNumber = reader.ReadByte(),
            PacketType = (InboundMessageType)reader.ReadByte(),
            PacketVersion = reader.ReadByte(),
            PacketLength = reader.BaseStream.Length
        };
    }
}