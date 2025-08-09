#nullable disable

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using SimRacingSdk.Acc.Udp.Abstractions;
using SimRacingSdk.Acc.Udp.Messages;

namespace SimRacingSdk.Acc.Udp;

public class AccUdpConnection : IAccUdpConnection
{
    private readonly AccUdpMessageHandler broadcastingMessageHandler;
    private readonly IPEndPoint ipEndPoint;
    private readonly CompositeDisposable subscriptionSink = new();

    private bool isConnected;
    private bool isDisposed;
    private bool isStopped;
    private Task listenerTask;
    private UdpClient udpClient ;

    public AccUdpConnection(string ipAddress,
        int port,
        string displayName,
        string connectionPassword,
        string commandPassword,
        int updateInterval)
    {
        this.IpAddress = ipAddress;
        this.Port = port;
        this.DisplayName = displayName;
        this.ConnectionPassword = connectionPassword;
        this.CommandPassword = commandPassword;
        this.UpdateInterval = updateInterval;
        this.ConnectionIdentifier = $"{this.IpAddress}:{this.Port}";
        this.ipEndPoint = IPEndPoint.Parse(this.ConnectionIdentifier);

        this.broadcastingMessageHandler = new AccUdpMessageHandler(this.ConnectionIdentifier);
    }

    public IObservable<BroadcastingEvent> BroadcastingEvents =>
        this.broadcastingMessageHandler.BroadcastingEvents;
    public string CommandPassword { get; }
    public string ConnectionIdentifier { get; }
    public string ConnectionPassword { get; }
    public IObservable<ConnectionState> ConnectionStateChanges =>
        this.broadcastingMessageHandler.ConnectionStateChanges;
    public string DisplayName { get; }
    public IObservable<EntryListUpdate> EntryListUpdates => this.broadcastingMessageHandler.EntryListUpdates;
    public string IpAddress { get; }
    public IObservable<string> LogMessages => this.broadcastingMessageHandler.LogMessages;
    public int Port { get; }
    public IObservable<RealtimeCarUpdate> RealTimeCarUpdates =>
        this.broadcastingMessageHandler.RealTimeCarUpdates;
    public IObservable<RealtimeUpdate> RealTimeUpdates => this.broadcastingMessageHandler.RealTimeUpdates;
    public IObservable<TrackDataUpdate> TrackDataUpdates => this.broadcastingMessageHandler.TrackDataUpdates;
    public int UpdateInterval { get; }

    public void Connect(bool autoDetect = true)
    {
        this.subscriptionSink.Add(
            this.broadcastingMessageHandler.ConnectionStateChanges
                .Subscribe(this.OnNextConnectionStateChange));
        this.subscriptionSink.Add(
            this.broadcastingMessageHandler.DispatchedMessages.Subscribe(this.OnNextDispatchedMessage));

        try
        {
            if(autoDetect)
            {
                this.WaitUntilRegistered();
            }

            this.listenerTask = this.HandleMessages();
            this.broadcastingMessageHandler.RequestTrackData();
            this.broadcastingMessageHandler.RequestEntryList();
        }
        catch(Exception exception)
        {
            this.LogMessage(exception.Message);
            Debug.WriteLine(exception.Message);
            throw;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        this.Dispose(true);
    }

    public void SetActiveCamera(string cameraSetName, string cameraName)
    {
        this.broadcastingMessageHandler.SetCamera(cameraSetName, cameraName);
    }

    public void SetActiveCamera(string cameraSetName, string cameraName, int carIndex)
    {
        this.broadcastingMessageHandler.SetFocus((ushort)carIndex, cameraSetName, cameraName);
    }

    public void SetFocus(int carIndex)
    {
        this.broadcastingMessageHandler.SetFocus((ushort)carIndex);
    }

    public void SetHudPage(string hudPage)
    {
        this.broadcastingMessageHandler.RequestHUDPage(hudPage);
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
                this.Shutdown();
            }
            catch(Exception exception)
            {
                this.LogMessage(exception.Message);
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
        this.LogMessage("Processing messages from ACC...");

        while(!this.isStopped)
        {
            await this.ProcessNextMessage();
        }
    }

    private void LogMessage(string message)
    {
        this.broadcastingMessageHandler.LogMessage(message);
    }

    private void OnNextConnectionStateChange(ConnectionState connectionState)
    {
        this.isConnected = connectionState.IsConnected;
        this.LogMessage($"Connection State Changed: {connectionState}");
    }

    private void OnNextDispatchedMessage(byte[] message)
    {
        try
        {
            this.udpClient?.Send(message, message.Length);
        }
        catch(Exception exception)
        {
            this.LogMessage(exception.Message);
            Debug.WriteLine(exception);
        }
    }

    private async Task ProcessNextMessage()
    {
        try
        {
            var udpReceiveResult = await this.udpClient.ReceiveAsync()!;
            await using var stream = new MemoryStream(udpReceiveResult.Buffer);
            using var reader = new BinaryReader(stream);
            this.broadcastingMessageHandler.ProcessMessage(reader);
        }
        catch(Exception exception)
        {
            this.LogMessage($"Unexpected Error Processing Message: {exception.Message}");
            this.Shutdown();
        }
    }

    private void Shutdown()
    {
        this.LogMessage("Disconnecting from ACC Broadcasting API...");
        this.isStopped = true;
        this.broadcastingMessageHandler.Disconnect();
        this.subscriptionSink?.Dispose();
        this.udpClient?.Close();
        this.udpClient?.Dispose();
        this.udpClient = null;
    }

    private void WaitUntilRegistered()
    {
        this.LogMessage("Waiting for ACC registration...");
        var isRegistered = false;
        while(!isRegistered)
        {
            UdpClient client = null;
            try
            {
                var endPoint = IPEndPoint.Parse(this.ConnectionIdentifier);
                var message = this.broadcastingMessageHandler.CreateRegisterCommandApplicationMessage(
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
                this.broadcastingMessageHandler.ProcessMessage(reader);
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