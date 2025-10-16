#nullable disable

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Acc.Udp.Abstractions;
using SimRacingSdk.Acc.Udp.Messages;

namespace SimRacingSdk.Acc.Udp;

public class AccUdpConnection : IAccUdpConnection
{
    private readonly AccUdpMessageHandler accUdpMessageHandler;
    private readonly IPEndPoint ipEndPoint;
    private readonly TimeSpan messageTimeout = TimeSpan.FromSeconds(2);
    private readonly CompositeDisposable subscriptionSink = new();

    private bool isConnected;
    private bool isDisposed;
    private bool isStopped;
    private DateTime lastRealTimeUpdate;
    private Task listenerTask;
    private UdpClient udpClient;

    public AccUdpConnection(string ipAddress,
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

        this.accUdpMessageHandler = new AccUdpMessageHandler(this.ConnectionIdentifier);
    }

    public IObservable<BroadcastingEvent> BroadcastingEvents =>
        this.accUdpMessageHandler.BroadcastingEvents.AsObservable();
    public string CommandPassword { get; }
    public string ConnectionIdentifier { get; }
    public string ConnectionPassword { get; }
    public IObservable<ConnectionState> ConnectionStateChanges =>
        this.accUdpMessageHandler.ConnectionStateChanges;
    public string DisplayName { get; }
    public IObservable<EntryListUpdate> EntryListUpdates => this.accUdpMessageHandler.EntryListUpdates;
    public string IpAddress { get; }
    public IObservable<LogMessage> LogMessages => this.accUdpMessageHandler.LogMessages;
    public int Port { get; }
    public IObservable<RealtimeCarUpdate> RealTimeCarUpdates => this.accUdpMessageHandler.RealTimeCarUpdates;
    public IObservable<RealtimeUpdate> RealTimeUpdates => this.accUdpMessageHandler.RealTimeUpdates;
    public IObservable<TrackDataUpdate> TrackDataUpdates => this.accUdpMessageHandler.TrackDataUpdates;
    public int UpdateInterval { get; }

    public void Connect(bool autoDetect = true)
    {
        this.subscriptionSink.Add(
            this.accUdpMessageHandler.ConnectionStateChanges.Subscribe(this.OnNextConnectionStateChange));
        this.subscriptionSink.Add(
            this.accUdpMessageHandler.DispatchedMessages.Subscribe(this.OnNextDispatchedMessage));
        this.subscriptionSink.Add(
            this.accUdpMessageHandler.RealTimeUpdates.Subscribe(this.OnNextRealTimeUpdate));
        this.subscriptionSink.Add(
            this.accUdpMessageHandler.TrackDataUpdates.Subscribe(this.OnNextTrackDataUpdate));

        try
        {
            if(autoDetect)
            {
                this.WaitUntilRegistered();
            }

            this.listenerTask = this.HandleMessages();
            this.accUdpMessageHandler.RequestTrackData();
            this.StartDisconnectedWatcher();
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
        this.accUdpMessageHandler.RequestEntryList();
    }

    public void SetActiveCamera(string cameraSetName, string cameraName)
    {
        this.accUdpMessageHandler.SetCamera(cameraSetName, cameraName);
    }

    public void SetActiveCamera(string cameraSetName, string cameraName, int carIndex)
    {
        this.accUdpMessageHandler.SetFocus((ushort)carIndex, cameraSetName, cameraName);
    }

    public void SetFocus(int carIndex)
    {
        this.accUdpMessageHandler.SetFocus((ushort)carIndex);
    }

    public void SetHudPage(string hudPage)
    {
        this.accUdpMessageHandler.RequestHUDPage(hudPage);
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
        this.LogMessage(LoggingLevel.Information, "Processing messages from ACC...");

        while(!this.isStopped)
        {
            await this.ProcessNextMessage();
        }
    }

    private void LogMessage(LoggingLevel level, string message, object? data = null)
    {
        this.accUdpMessageHandler.LogMessage(level, message, data);
    }

    private void OnNextConnectionStateChange(ConnectionState connectionState)
    {
        this.isConnected = connectionState.IsConnected;
        this.LogMessage(LoggingLevel.Information, connectionState.ToString());
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

    private void OnNextRealTimeUpdate(RealtimeUpdate realtimeUpdate)
    {
        this.lastRealTimeUpdate = DateTime.Now;
    }

    private void OnNextTrackDataUpdate(TrackDataUpdate trackDataUpdate)
    {
        this.accUdpMessageHandler.RequestEntryList();
    }

    private async Task ProcessNextMessage()
    {
        try
        {
            var udpReceiveResult = await this.udpClient.ReceiveAsync()!;
            await using var stream = new MemoryStream(udpReceiveResult.Buffer);
            using var reader = new BinaryReader(stream);
            this.accUdpMessageHandler.ProcessMessage(reader);
        }
        catch(Exception exception)
        {
            this.LogMessage(LoggingLevel.Error, $"Unexpected Error Processing Message: {exception.Message}");
            this.Shutdown();
        }
    }

    private void Shutdown()
    {
        if(this.isStopped)
        {
            return;
        }

        this.LogMessage(LoggingLevel.Information, "Disconnecting from ACC Broadcasting API...");
        this.isStopped = true;
        this.accUdpMessageHandler.Disconnect();
        this.subscriptionSink?.Dispose();
        this.udpClient?.Close();
        this.udpClient?.Dispose();
        this.udpClient = null;
    }

    private void StartDisconnectedWatcher()
    {
        var subscription = Observable.Interval(this.messageTimeout)
                                     .Subscribe(n =>
                                                {
                                                    var timeSinceLastUpdate =
                                                        DateTime.Now - this.lastRealTimeUpdate;
                                                    if(!this.isConnected || timeSinceLastUpdate
                                                       <= this.messageTimeout)
                                                    {
                                                        return;
                                                    }

                                                    this.accUdpMessageHandler.SessionTerminated();
                                                    this.LogMessage(LoggingLevel.Information,
                                                        "ACC has stopped sending messages, the user has probably quit the session.");
                                                    this.Shutdown();
                                                });
        this.subscriptionSink.Add(subscription);
    }

    private void WaitUntilRegistered()
    {
        this.LogMessage(LoggingLevel.Information, "Waiting for ACC registration...");
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
                var message = this.accUdpMessageHandler.CreateRegisterCommandApplicationMessage(
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
                this.accUdpMessageHandler.ProcessMessage(reader);
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