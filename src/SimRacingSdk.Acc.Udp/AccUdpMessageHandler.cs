using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Udp.Enums;
using SimRacingSdk.Acc.Udp.Extensions;
using SimRacingSdk.Acc.Udp.Messages;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Acc.Udp;

internal class AccUdpMessageHandler
{
    internal const int BroadcastingProtocolVersion = 4;

    private readonly Subject<BroadcastingEvent> broadcastingEventSubject = new();
    private readonly Subject<ConnectionState> connectionStateChangeSubject = new();
    private readonly Subject<byte[]> dispatchedMessagesSubject = new();
    private readonly IList<CarInfo> entryList = new List<CarInfo>();
    private readonly Subject<EntryListUpdate> entryListUpdateSubject = new();
    private readonly Subject<LogMessage> logMessagesSubject = new();
    private readonly Subject<RealtimeCarUpdate> realTimeCarUpdateSubject = new();
    private readonly Subject<RealtimeUpdate> realTimeUpdateSubject = new();
    private readonly Subject<TrackDataUpdate> trackDataUpdateSubject = new();

    private DateTime lastEntryListRequest = DateTime.UtcNow;

    internal AccUdpMessageHandler(string connectionIdentifier)
    {
        if(string.IsNullOrEmpty(connectionIdentifier))
        {
            throw new ArgumentException(
                "No connection identifier provided.  A unique identifier is required for managing connections. IP Address and Port is a good identifier e.g. 127.0.0.1:9000");
        }

        this.ConnectionIdentifier = connectionIdentifier;
    }

    internal IObservable<BroadcastingEvent> BroadcastingEvents =>
        this.broadcastingEventSubject.AsObservable();
    internal string ConnectionIdentifier { get; }
    internal IObservable<ConnectionState> ConnectionStateChanges =>
        this.connectionStateChangeSubject.AsObservable();
    internal IObservable<byte[]> DispatchedMessages => this.dispatchedMessagesSubject.AsObservable();
    internal IObservable<EntryListUpdate> EntryListUpdates => this.entryListUpdateSubject.AsObservable();
    internal IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();
    internal IObservable<RealtimeCarUpdate> RealTimeCarUpdates =>
        this.realTimeCarUpdateSubject.AsObservable();
    internal IObservable<RealtimeUpdate> RealTimeUpdates => this.realTimeUpdateSubject.AsObservable();
    internal IObservable<TrackDataUpdate> TrackDataUpdates => this.trackDataUpdateSubject.AsObservable();

    private int ConnectionId { get; set; }

    internal byte[] CreateRegisterCommandApplicationMessage(string displayName,
        string connectionPassword,
        int updateInterval,
        string commandPassword)
    {
        MemoryStream stream = null;
        try
        {
            stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            writer.Write((byte)OutboundMessageTypes.RegisterCommandApplication);
            writer.Write((byte)BroadcastingProtocolVersion);

            writer.WriteString(displayName);
            writer.WriteString(connectionPassword ?? string.Empty);
            writer.Write(updateInterval);
            writer.WriteString(commandPassword ?? string.Empty);
            return stream.ToArray();
        }
        catch
        {
            stream?.Dispose();
            throw;
        }
    }

    internal void Disconnect(bool sendUnregister = true)
    {
        this.connectionStateChangeSubject.OnNext(new ConnectionState(this.ConnectionId, false, false));

        if(sendUnregister)
        {
            try
            {
                using var stream = new MemoryStream();
                using var writer = new BinaryWriter(stream);
                writer.Write((byte)OutboundMessageTypes.UnregisterCommandApplication);
                this.DispatchMessage(stream.ToArray());
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }

    internal void LogMessage(LoggingLevel loggingLevel, string content)
    {
        this.logMessagesSubject.OnNext(new LogMessage(loggingLevel, content));
    }

    internal void ProcessMessage(BinaryReader reader)
    {
        var messageType = (InboundMessageType)reader.ReadByte();
        switch(messageType)
        {
            case InboundMessageType.BroadcastingEvent:
                this.ProcessBroadCastingEventMessage(reader);
                break;
            case InboundMessageType.EntryList:
                this.ProcessEntryListMessage(reader);
                break;
            case InboundMessageType.EntryListCar:
                this.ProcessEntryListCarMessage(reader);
                break;
            case InboundMessageType.RealtimeUpdate:
                this.ProcessRealtimeUpdateMessage(reader);
                break;
            case InboundMessageType.RealtimeCarUpdate:
                this.ProcessRealtimeCarUpdateMessage(reader);
                break;
            case InboundMessageType.RegistrationResult:
                this.ProcessRegistrationResultMessage(reader);
                break;
            case InboundMessageType.TrackData:
                this.ProcessTrackDataMessage(reader);
                break;
            default:
                this.LogMessage(LoggingLevel.Warning, "Unknown message type");
                break;
        }
    }

    internal void RequestConnection(string displayName,
        string connectionPassword,
        int updateInterval,
        string commandPassword)
    {
        var message = this.CreateRegisterCommandApplicationMessage(displayName,
            connectionPassword,
            updateInterval,
            commandPassword);

        this.DispatchMessage(message);
    }

    internal void RequestEntryList()
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        writer.Write((byte)OutboundMessageTypes.RequestEntryList);
        writer.Write(this.ConnectionId);

        this.DispatchMessage(stream.ToArray());
    }

    internal void RequestHUDPage(string hudPage)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        writer.Write((byte)OutboundMessageTypes.ChangeHudPage);
        writer.Write(this.ConnectionId);
        writer.WriteString(hudPage);

        this.DispatchMessage(stream.ToArray());
    }

    internal void RequestInstantReplay(float startSessionTime,
        float durationMS,
        int initialFocusedCarIndex = -1,
        string initialCameraSet = "",
        string initialCamera = "")
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        writer.Write((byte)OutboundMessageTypes.InstantReplayRequest);
        writer.Write(this.ConnectionId);

        writer.Write(startSessionTime);
        writer.Write(durationMS);
        writer.Write(initialFocusedCarIndex);

        writer.WriteString(initialCameraSet);
        writer.WriteString(initialCamera);

        this.DispatchMessage(stream.ToArray());
    }

    internal void RequestTrackData()
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        writer.Write((byte)OutboundMessageTypes.RequestTrackData);
        writer.Write(this.ConnectionId);

        this.DispatchMessage(stream.ToArray());
    }

    internal void SessionTerminated()
    {
        this.connectionStateChangeSubject.OnNext(new ConnectionState(this.ConnectionId, false, false));
    }

    internal void SetCamera(string cameraSet, string camera)
    {
        this.SetFocusInternal(null, cameraSet, camera);
    }

    internal void SetFocus(ushort carIndex)
    {
        this.SetFocusInternal(carIndex, null, null);
    }

    internal void SetFocus(ushort carIndex, string cameraSet, string camera)
    {
        this.SetFocusInternal(carIndex, cameraSet, camera);
    }

    private void DispatchMessage(byte[] message)
    {
        this.dispatchedMessagesSubject.OnNext(message);
    }

    private void ProcessBroadCastingEventMessage(BinaryReader binaryReader)
    {
        var eventData = binaryReader.ReadBroadcastingEvent();
        eventData.CarData = this.entryList.FirstOrDefault(e => e.CarIndex == eventData.CarId)!;
        this.broadcastingEventSubject.OnNext(eventData);
    }

    private void ProcessEntryListCarMessage(BinaryReader reader)
    {
        var carId = reader.ReadUInt16();

        var carInfo = this.entryList.SingleOrDefault(x => x.CarIndex == carId);
        if(carInfo == null)
        {
            Debug.WriteLine($"Entry list update for unknown carIndex {carId}");
            return;
        }

        reader.UpdateCarInfo(carInfo);

        var update = new EntryListUpdate(this.ConnectionIdentifier, carInfo);
        this.entryListUpdateSubject.OnNext(update);
        Debug.WriteLine(update.ToString());
    }

    private void ProcessEntryListMessage(BinaryReader reader)
    {
        this.entryList.Clear();

        var connectionId = reader.ReadInt32();
        var carEntryCount = reader.ReadUInt16();
        for(var i = 0; i < carEntryCount; i++)
        {
            var carInfo = new CarInfo(reader.ReadUInt16());
            this.entryList.Add(carInfo);
            Debug.WriteLine(carInfo.ToString());
        }
    }

    private void ProcessRealtimeCarUpdateMessage(BinaryReader reader)
    {
        if(!this.entryList.Any())
        {
            return;
        }

        var update = reader.ReadRealtimeCarUpdate();
        var carEntry = this.entryList.FirstOrDefault(x => x.CarIndex == update.CarIndex);
        if(carEntry == null || carEntry.Drivers.Count != update.DriverCount)
        {
            if(!((DateTime.Now - this.lastEntryListRequest).TotalSeconds > 1))
            {
                return;
            }

            this.lastEntryListRequest = DateTime.Now;
            this.RequestEntryList();
            Debug.WriteLine(
                $"Car {update.CarIndex}|{update.DriverIndex} not known, requesting new Entry List");
        }
        else
        {
            this.realTimeCarUpdateSubject.OnNext(update);
            Debug.WriteLine(update.ToString());
        }
    }

    private void ProcessRealtimeUpdateMessage(BinaryReader reader)
    {
        var update = reader.ReadRealtimeUpdate();
        this.realTimeUpdateSubject.OnNext(update);
        Debug.WriteLine(update.ToString());
    }

    private void ProcessRegistrationResultMessage(BinaryReader reader)
    {
        this.ConnectionId = reader.ReadInt32();
        var connectionSuccess = reader.ReadByte() > 0;
        var isReadonly = reader.ReadByte() == 0;
        var errMsg = reader.ReadString();

        var connectionState = new ConnectionState(this.ConnectionId, connectionSuccess, isReadonly, errMsg);
        this.connectionStateChangeSubject.OnNext(connectionState);
        Debug.WriteLine(connectionState.ToString());
    }

    private void ProcessTrackDataMessage(BinaryReader reader)
    {
        var connectionId = reader.ReadInt32();
        var update = reader.ReadTrackDataUpdate(this.ConnectionIdentifier);
        this.trackDataUpdateSubject.OnNext(update);
        Debug.WriteLine(update.ToString());
    }

    private void SetFocusInternal(ushort? carIndex, string cameraSet, string camera)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        writer.Write((byte)OutboundMessageTypes.ChangeFocus);
        writer.Write(this.ConnectionId);

        if(!carIndex.HasValue)
        {
            writer.Write((byte)0);
        }
        else
        {
            writer.Write((byte)1);
            writer.Write(carIndex.Value);
        }

        if(string.IsNullOrEmpty(cameraSet) || string.IsNullOrEmpty(camera))
        {
            writer.Write((byte)0);
        }
        else
        {
            writer.Write((byte)1);
            writer.WriteString(cameraSet);
            writer.WriteString(camera);
        }

        this.DispatchMessage(stream.ToArray());
    }
}