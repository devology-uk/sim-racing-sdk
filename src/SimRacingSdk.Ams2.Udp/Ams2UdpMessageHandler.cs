using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Ams2.Udp.Enums;
using SimRacingSdk.Ams2.Udp.Extensions;
using SimRacingSdk.Ams2.Udp.Messages;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ams2.Udp;

internal class Ams2UdpMessageHandler
{
    private readonly Subject<GameStateUpdate> gameStateUpdatesSubject = new();
    private readonly Subject<LogMessage> logMessagesSubject = new();
    private readonly Subject<ParticipantsUpdate> participantUpdatesSubject = new();
    private readonly Subject<RaceInfoUpdate> raceInfoUpdatesSubject = new();
    private readonly Subject<TelemetryUpdate> telemetryUpdatesSubject = new();
    private readonly Subject<TimeStatsUpdate> timeStatsUpdatesSubject = new();
    private readonly Subject<TimingsUpdate> timingsUpdatesSubject = new();
    private readonly Subject<VehicleClassUpdate> vehicleClassUpdatesSubject = new();
    private readonly Subject<VehicleInfoUpdate> vehicleInfoUpdatesSubject = new();

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
    public IObservable<ParticipantsUpdate> ParticipantUpdates => this.participantUpdatesSubject.AsObservable();
    public IObservable<RaceInfoUpdate> RaceInfoUpdates => this.raceInfoUpdatesSubject.AsObservable();
    public IObservable<TelemetryUpdate> TelemetryUpdates => this.telemetryUpdatesSubject.AsObservable();
    public IObservable<TimeStatsUpdate> TimeStatsUpdates => this.timeStatsUpdatesSubject.AsObservable();
    public IObservable<TimingsUpdate> TimingsUpdates => this.timingsUpdatesSubject.AsObservable();
    public IObservable<VehicleClassUpdate> VehicleClassUpdates => this.vehicleClassUpdatesSubject.AsObservable();
    public IObservable<VehicleInfoUpdate> VehicleInfoUpdates => this.vehicleInfoUpdatesSubject.AsObservable();
    internal IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();

    public void ProcessMessage(BinaryReader reader)
    {
        var header = reader.ReadHeader();
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
                this.ProcessVehicleMessage(reader, header);
                break;
            default:
                this.LogMessage(LoggingLevel.Warning, "Unknown message type");
                break;
        }
    }

    internal void Disconnect(bool sendUnregister = true) { }

    internal void LogMessage(LoggingLevel loggingLevel, string content)
    {
        this.logMessagesSubject.OnNext(new LogMessage(loggingLevel, content));
    }

    private void ProcessGameStateMessage(BinaryReader reader, MessageHeader header)
    {
        var gameStateUpdate = reader.ReadGameStateUpdate(header);

        this.gameStateUpdatesSubject.OnNext(gameStateUpdate);
    }

    private void ProcessParticipantsMessage(BinaryReader reader, MessageHeader header)
    {
        var participantsUpdate = reader.ReadParticipantsUpdate(header);

        this.participantUpdatesSubject.OnNext(participantsUpdate);
    }

    private void ProcessRaceInfoMessage(BinaryReader reader, MessageHeader header)
    {
        var raceInfoUpdate = reader.ReadRaceInfoUpdate(header);

        this.raceInfoUpdatesSubject.OnNext(raceInfoUpdate);
    }

    private void ProcessTelemetryMessage(BinaryReader reader, MessageHeader header)
    {
        var telemetryUpdate = reader.ReadTelemetryUpdate(header);

        this.telemetryUpdatesSubject.OnNext(telemetryUpdate);
    }

    private void ProcessTimeStatsMessage(BinaryReader reader, MessageHeader header)
    {
        var timeStats = reader.ReadTimeStatsUpdate(header);

        this.timeStatsUpdatesSubject.OnNext(timeStats);
    }

    private void ProcessTimingsMessage(BinaryReader reader, MessageHeader header)
    {
        var timingsUpdate = reader.ReadTimingsUpdate(header);

        this.timingsUpdatesSubject.OnNext(timingsUpdate);
    }

    private void ProcessVehicleClassesUpdate(BinaryReader reader, MessageHeader header)
    {
        var vehicleClassUpdate = reader.ReadVehicleClassUpdate(header);

        this.vehicleClassUpdatesSubject.OnNext(vehicleClassUpdate);
    }

    private void ProcessVehicleInfoUpdate(BinaryReader reader, MessageHeader header)
    {
        var vehicleInfoUpdate = reader.ReadVehicleInfoUpdate(header);

        this.vehicleInfoUpdatesSubject.OnNext(vehicleInfoUpdate);
    }

    private void ProcessVehicleMessage(BinaryReader reader, MessageHeader header)
    {
        if(reader.BaseStream.Length == 1164)
        {
            this.ProcessVehicleInfoUpdate(reader, header);
            return;
        }

        this.ProcessVehicleClassesUpdate(reader, header);
    }
}