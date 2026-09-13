using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Core.Services;
using SimRacingSdk.Pmr.Udp.Enums;
using SimRacingSdk.Pmr.Udp.Extensions;
using SimRacingSdk.Pmr.Udp.Messages;

namespace SimRacingSdk.Pmr.Udp;

internal class PmrUdpMessageHandler
{
    private const ushort ExpectedRaceInfoVersion = 1;
    private const ushort ExpectedParticipantRaceStateVersion = 1;
    private const ushort ExpectedVehicleTelemetryVersion = 2;

    private readonly LogMessageBroker logMessageBroker = new(nameof(PmrUdpMessageHandler));
    private readonly Subject<PmrParticipantRaceState> participantRaceStateSubject = new();
    private readonly Subject<PmrRaceInfo> raceInfoSubject = new();
    private readonly Subject<PmrSessionStopped> sessionStoppedSubject = new();
    private readonly Subject<PmrVehicleTelemetry> vehicleTelemetrySubject = new();

    internal IObservable<LogMessage> LogMessages => this.logMessageBroker.Messages;
    internal IObservable<PmrParticipantRaceState> ParticipantRaceStates =>
        this.participantRaceStateSubject.AsObservable();
    internal IObservable<PmrRaceInfo> RaceInfoUpdates => this.raceInfoSubject.AsObservable();
    internal IObservable<PmrSessionStopped> SessionStoppedUpdates =>
        this.sessionStoppedSubject.AsObservable();
    internal IObservable<PmrVehicleTelemetry> VehicleTelemetryUpdates =>
        this.vehicleTelemetrySubject.AsObservable();

    internal void LogMessage(LoggingLevel loggingLevel, string content, string? source = null)
    {
        this.logMessageBroker.Log(loggingLevel, content, source);
    }

    internal void ProcessMessage(BinaryReader reader)
    {
        var packetType = (PmrPacketType)reader.ReadByte();
        switch(packetType)
        {
            case PmrPacketType.RaceInfo:
                this.ProcessRaceInfoMessage(reader);
                break;
            case PmrPacketType.ParticipantRaceState:
                this.ProcessParticipantRaceStateMessage(reader);
                break;
            case PmrPacketType.ParticipantVehicleTelemetry:
                this.ProcessVehicleTelemetryMessage(reader);
                break;
            case PmrPacketType.SessionStopped:
                this.ProcessSessionStoppedMessage(reader);
                break;
            default:
                this.LogMessage(LoggingLevel.Warning, $"Unknown packet type: {(byte)packetType}");
                break;
        }
    }

    private bool IsExpectedVersion(ushort packetVersion, ushort expectedVersion, string packetName)
    {
        if(packetVersion == expectedVersion)
        {
            return true;
        }

        this.LogMessage(LoggingLevel.Warning,
            $"{packetName} packet version mismatch. Expected: {expectedVersion} - Got: {packetVersion}");
        return false;
    }

    private void ProcessParticipantRaceStateMessage(BinaryReader reader)
    {
        var packetVersion = reader.ReadUInt16();
        if(!this.IsExpectedVersion(packetVersion, ExpectedParticipantRaceStateVersion,
               nameof(PmrParticipantRaceState)))
        {
            return;
        }

        var participantRaceState = reader.ReadPmrParticipantRaceState() with { PacketVersion = packetVersion };
        this.participantRaceStateSubject.OnNext(participantRaceState);
    }

    private void ProcessRaceInfoMessage(BinaryReader reader)
    {
        var packetVersion = reader.ReadUInt16();
        if(!this.IsExpectedVersion(packetVersion, ExpectedRaceInfoVersion, nameof(PmrRaceInfo)))
        {
            return;
        }

        var raceInfo = reader.ReadPmrRaceInfo() with { PacketVersion = packetVersion };
        this.raceInfoSubject.OnNext(raceInfo);
    }

    private void ProcessSessionStoppedMessage(BinaryReader reader)
    {
        var packetVersion = reader.ReadUInt16();
        this.sessionStoppedSubject.OnNext(new PmrSessionStopped { PacketVersion = packetVersion });
    }

    private void ProcessVehicleTelemetryMessage(BinaryReader reader)
    {
        var packetVersion = reader.ReadUInt16();
        if(!this.IsExpectedVersion(packetVersion, ExpectedVehicleTelemetryVersion, nameof(PmrVehicleTelemetry)))
        {
            return;
        }

        var vehicleTelemetry = reader.ReadPmrVehicleTelemetry() with { PacketVersion = packetVersion };
        this.vehicleTelemetrySubject.OnNext(vehicleTelemetry);
    }
}
