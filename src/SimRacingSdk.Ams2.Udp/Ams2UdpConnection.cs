#nullable disable

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Ams2.Udp.Abstractions;
using SimRacingSdk.Ams2.Udp.Messages;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ams2.Udp;

public class Ams2UdpConnection : IAms2UdpConnection
{
    private readonly Ams2UdpMessageHandler ams2UdpMessageHandler;
    private readonly ReplaySubject<ConnectionState> connectionStateUpdatesSubject = new();
    private readonly IPEndPoint ipEndPoint;
    private readonly TimeSpan messageTimeout = TimeSpan.FromSeconds(2);
    private readonly CompositeDisposable subscriptionSink = new();

    private bool isConnected;
    private bool isDisposed;
    private bool isStopped;
    private DateTime lastMessageReceivedAt;
    private Task listenerTask;
    private UdpClient? udpClient;
    private string currentConnectionId;

    public Ams2UdpConnection(string ipAddress, int port)
    {
        this.IpAddress = ipAddress;
        this.Port = port;
        this.ConnectionIdentifier = $"{this.IpAddress}:{this.Port}";
        this.ipEndPoint = IPEndPoint.Parse(this.ConnectionIdentifier);

        this.ams2UdpMessageHandler = new Ams2UdpMessageHandler(this.ConnectionIdentifier);
    }

    public string ConnectionIdentifier { get; }

    public IObservable<ConnectionState> ConnectionStateUpdates =>
        this.connectionStateUpdatesSubject.AsObservable();
    public IObservable<GameStateUpdate> GameStateUpdates => this.ams2UdpMessageHandler.GameStateUpdates;
    public string IpAddress { get; }
    public IObservable<LogMessage> LogMessages => this.ams2UdpMessageHandler.LogMessages;
    public IObservable<ParticipantsUpdate> ParticipantUpdates =>
        this.ams2UdpMessageHandler.ParticipantUpdates;
    public int Port { get; }
    public IObservable<RaceInfoUpdate> RaceInfoUpdates => this.ams2UdpMessageHandler.RaceInfoUpdates;
    public IObservable<TelemetryUpdate> TelemetryUpdates => this.ams2UdpMessageHandler.TelemetryUpdates;
    public IObservable<TimeStatsUpdate> TimeStatsUpdates => this.ams2UdpMessageHandler.TimeStatsUpdates;
    public IObservable<TimingsUpdate> TimingsUpdates => this.ams2UdpMessageHandler.TimingsUpdates;
    public IObservable<VehicleClassUpdate> VehicleClassUpdates =>
        this.ams2UdpMessageHandler.VehicleClassUpdates;
    public IObservable<VehicleInfoUpdate> VehicleInfoUpdates => this.ams2UdpMessageHandler.VehicleInfoUpdates;

    public void Connect(bool autoDetect = true)
    {
        try
        {
            if(autoDetect)
            {
                this.WaitUntilConnected();
            }

            this.listenerTask = this.HandleMessages();
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
        this.LogMessage(LoggingLevel.Information, "Processing messages from AMS2...");

        while(!this.isStopped)
        {
            await this.ProcessNextMessage();
        }
    }

    private void LogMessage(LoggingLevel loggingLevel, string content)
    {
        this.ams2UdpMessageHandler.LogMessage(loggingLevel, content);
    }

    private async Task ProcessNextMessage()
    {
        try
        {
            var udpReceiveResult = await this.udpClient.ReceiveAsync()!;
            this.lastMessageReceivedAt = DateTime.UtcNow;
            await using var stream = new MemoryStream(udpReceiveResult.Buffer);
            using var reader = new BinaryReader(stream);
            this.ams2UdpMessageHandler.ProcessMessage(reader);
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

        this.LogMessage(LoggingLevel.Information, "Disconnecting from AMS2...");
        this.isStopped = true;
        this.ams2UdpMessageHandler.Disconnect();
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
                                                        DateTime.Now - this.lastMessageReceivedAt;
                                                    if(!this.isConnected || timeSinceLastUpdate
                                                       <= this.messageTimeout)
                                                    {
                                                        return;
                                                    }

                                                    this.isConnected = false;
                                                    this.connectionStateUpdatesSubject.OnNext(new ConnectionState(false, this.currentConnectionId));
                                                    this.LogMessage(LoggingLevel.Information,
                                                        "AMS2 has stopped sending messages, the user has probably quit the session.");
                                                    this.Shutdown();
                                                });
        this.subscriptionSink.Add(subscription);
    }

    private void WaitUntilConnected()
    {
        this.LogMessage(LoggingLevel.Information, "Waiting for AMS2 connection...");
        var isAvailable = false;
        while(!isAvailable)
        {
            if(this.isStopped)
            {
                return;
            }

            UdpClient? client = null;
            try
            {
                var endPoint = IPEndPoint.Parse(this.ConnectionIdentifier);
                client = this.CreateUdpClient(endPoint);
                if(client.Available != 0)
                {
                    this.isConnected = true;
                    this.currentConnectionId = Guid.NewGuid()
                                                   .ToString();
                    this.connectionStateUpdatesSubject.OnNext(new ConnectionState(true, this.currentConnectionId));
                    var receiveBytes = client.Receive(ref endPoint);

                    using var stream = new MemoryStream(receiveBytes);
                    using var reader = new BinaryReader(stream);
                    isAvailable = true;
                    this.ams2UdpMessageHandler.ProcessMessage(reader);
                    this.udpClient = client;
                    return;
                }
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