#nullable disable

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using SimRacingSdk.Ace.Udp.Abstractions;
using SimRacingSdk.Ace.Udp.Messages;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ace.Udp;

public class AceUdpConnection : IAceUdpConnection
{
    private readonly AceUdpMessageHandler aceUdpMessageHandler;
    private readonly IPEndPoint ipEndPoint;
    private readonly CompositeDisposable subscriptionSink = new();

    private bool isConnected;
    private bool isDisposed;
    private bool isStopped;
    private Task listenerTask;
    private UdpClient udpClient;

    public AceUdpConnection(string ipAddress,
        int port,
        string displayName,
        string connectionPassword,
        string commandPassword,
        int updateInterval = 100)
    {
        this.IpAddress = ipAddress;
        this.Port = port;
        this.DisplayName = displayName;
        this.ConnectionPassword = connectionPassword;
        this.CommandPassword = commandPassword;
        this.UpdateInterval = updateInterval;
        this.ConnectionIdentifier = $"{this.IpAddress}:{this.Port}";
        this.ipEndPoint = IPEndPoint.Parse(this.ConnectionIdentifier);

        this.aceUdpMessageHandler = new AceUdpMessageHandler(this.ConnectionIdentifier);
    }

    public IObservable<BroadcastingEvent> BroadcastingEvents => this.aceUdpMessageHandler.BroadcastingEvents.AsObservable();
    public string CommandPassword { get; }
    public IObservable<Connection> ConnectionStateChanges => this.aceUdpMessageHandler.ConnectionStateChanges;
    public string ConnectionIdentifier { get; }
    public string ConnectionPassword { get; }
    public string DisplayName { get; }
    public IObservable<EntryListUpdate> EntryListUpdates => this.aceUdpMessageHandler.EntryListUpdates;
    public string IpAddress { get; }
    public IObservable<LogMessage> LogMessages => this.aceUdpMessageHandler.LogMessages;
    public int Port { get; }
    public IObservable<RealtimeCarUpdate> RealTimeCarUpdates => this.aceUdpMessageHandler.RealTimeCarUpdates;
    public IObservable<RealtimeUpdate> RealTimeUpdates => this.aceUdpMessageHandler.RealTimeUpdates;
    public IObservable<TrackDataUpdate> TrackDataUpdates => this.aceUdpMessageHandler.TrackDataUpdates;
    public int UpdateInterval { get; }

    public void Connect(bool autoDetect = true)
    {
        this.subscriptionSink.Add(
            this.aceUdpMessageHandler.ConnectionStateChanges.Subscribe(this.OnNextConnectionStateChange));
        this.subscriptionSink.Add(
            this.aceUdpMessageHandler.DispatchedMessages.Subscribe(this.OnNextDispatchedMessage));
        this.subscriptionSink.Add(
            this.aceUdpMessageHandler.TrackDataUpdates.Subscribe(this.OnNextTrackDataUpdate));

        try
        {
            if(autoDetect)
            {
                this.WaitUntilRegistered();
            }

            this.listenerTask = this.HandleMessages();
            this.aceUdpMessageHandler.RequestTrackData();
        }
        catch(Exception exception)
        {
            this.LogMessage(LoggingLevel.Error, exception.Message);
            Debug.WriteLine(exception.Message);
            throw;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        this.Dispose(true);
    }

    public void RequestEntryList()
    {
        this.aceUdpMessageHandler.RequestEntryList();
    }

    public void SetActiveCamera(string cameraSetName, string cameraName)
    {
        this.aceUdpMessageHandler.SetCamera(cameraSetName, cameraName);
    }

    public void SetActiveCamera(string cameraSetName, string cameraName, int carIndex)
    {
        this.aceUdpMessageHandler.SetFocus((ushort)carIndex, cameraSetName, cameraName);
    }

    public void SetFocus(int carIndex)
    {
        this.aceUdpMessageHandler.SetFocus((ushort)carIndex);
    }

    public void SetHudPage(string hudPage)
    {
        this.aceUdpMessageHandler.RequestHUDPage(hudPage);
    }

    public void Stop()
    {
        this.isStopped = true;
        if(this.isConnected)
        {
            this.Shutdown();
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if(this.isDisposed)
        {
            return;
        }

        if(disposing)
        {
            try
            {
                this.Stop();
            }
            catch(Exception exception)
            {
                this.LogMessage(LoggingLevel.Error, exception.Message);
                Debug.WriteLine(exception);
            }
        }

        this.isDisposed = true;
    }

    private UdpClient CreateUdpClient(IPEndPoint ipEndPoint)
    {
        var client = new UdpClient();
        client.Client.ReceiveTimeout = 5000;
        client.Connect(ipEndPoint);
        return client;
    }

    private async Task HandleMessages()
    {
        this.LogMessage(LoggingLevel.Information, "Processing messages from Ace...");

        while(!this.isStopped)
        {
            await this.ProcessNextMessage();
        }
    }

    private void LogMessage(LoggingLevel level, string content)
    {
        this.aceUdpMessageHandler.LogMessage(level, content, nameof(AceUdpConnection));
    }

    private void OnNextConnectionStateChange(Connection connection)
    {
        this.isConnected = connection.IsConnected;
        this.LogMessage(LoggingLevel.Information, connection.ToString());
    }

    private void OnNextDispatchedMessage(byte[] message)
    {
        try
        {
            this.udpClient?.Send(message, message.Length);
        }
        catch(Exception exception)
        {
            this.LogMessage(LoggingLevel.Error, exception.Message);
            Debug.WriteLine(exception);
        }
    }

    private void OnNextTrackDataUpdate(TrackDataUpdate trackDataUpdate)
    {
        this.aceUdpMessageHandler.RequestEntryList();
    }

    private async Task ProcessNextMessage()
    {
        try
        {
            var udpReceiveResult = await this.udpClient.ReceiveAsync()!;
            await using var stream = new MemoryStream(udpReceiveResult.Buffer);
            using var reader = new BinaryReader(stream);
            this.aceUdpMessageHandler.ProcessMessage(reader);
        }
        catch(Exception exception)
        {
            this.LogMessage(LoggingLevel.Error, $"Unexpected Error Processing Message: {exception.Message}");
            this.Stop();
        }
    }

    private void Shutdown()
    {
        if(this.isStopped)
        {
            return;
        }

        this.LogMessage(LoggingLevel.Information, "Disconnecting from Ace Broadcasting API...");
        this.isStopped = true;
        this.aceUdpMessageHandler.Disconnect(true);
        this.subscriptionSink?.Dispose();
        this.udpClient?.Close();
        this.udpClient?.Dispose();
        this.udpClient = null;
    }

    private void WaitUntilRegistered()
    {
        this.LogMessage(LoggingLevel.Information, "Waiting for Ace registration...");
        var isRegistered = false;
        while(!isRegistered)
        {
            if(this.isStopped)
            {
                return;
            }

            UdpClient client = null;
            try
            {
                var endPoint = IPEndPoint.Parse(this.ConnectionIdentifier);
                var message = this.aceUdpMessageHandler.CreateRegisterCommandApplicationMessage(
                    this.DisplayName,
                    this.ConnectionPassword,
                    this.UpdateInterval,
                    this.CommandPassword);
                client = this.CreateUdpClient(endPoint);
                client.Send(message, message.Length);
                var receiveBytes = client.Receive(ref endPoint);

                using var stream = new MemoryStream(receiveBytes);
                using var reader = new BinaryReader(stream);
                isRegistered = true;
                this.aceUdpMessageHandler.ProcessMessage(reader);
                this.udpClient = client;
                return;
            }
            catch(SocketException socketException)
            {
                Console.WriteLine(socketException.Message);
                client?.Close();
            }

            Task.Delay(TimeSpan.FromSeconds(2))
                .GetAwaiter()
                .GetResult();
        }
    }
}
