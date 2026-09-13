#nullable disable

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Pmr.Udp.Abstractions;
using SimRacingSdk.Pmr.Udp.Messages;

namespace SimRacingSdk.Pmr.Udp;

public class PmrUdpConnection : IPmrUdpConnection
{
    public const string DefaultMulticastGroup = "224.0.0.150";
    public const int DefaultPort = 7576;

    private readonly TimeSpan messageTimeout = TimeSpan.FromSeconds(5);
    private readonly PmrUdpMessageHandler pmrUdpMessageHandler;
    private readonly CompositeDisposable subscriptionSink = new();

    private bool isDisposed;
    private bool isStopped;
    private DateTime lastMessageReceivedAt;
    private Task listenerTask;
    private UdpClient udpClient;

    public PmrUdpConnection(int port = DefaultPort,
        bool useMulticast = false,
        string multicastGroup = DefaultMulticastGroup)
    {
        this.Port = port;
        this.UseMulticast = useMulticast;
        this.MulticastGroup = multicastGroup;
        this.ConnectionIdentifier = this.UseMulticast
            ? $"{this.MulticastGroup}:{this.Port}"
            : $"0.0.0.0:{this.Port}";

        this.pmrUdpMessageHandler = new PmrUdpMessageHandler();
    }

    public string ConnectionIdentifier { get; }
    public IObservable<LogMessage> LogMessages => this.pmrUdpMessageHandler.LogMessages;
    public string MulticastGroup { get; }
    public IObservable<PmrParticipantRaceState> ParticipantRaceStates =>
        this.pmrUdpMessageHandler.ParticipantRaceStates;
    public int Port { get; }
    public IObservable<PmrRaceInfo> RaceInfoUpdates => this.pmrUdpMessageHandler.RaceInfoUpdates;
    public IObservable<PmrSessionStopped> SessionStoppedUpdates =>
        this.pmrUdpMessageHandler.SessionStoppedUpdates;
    public bool UseMulticast { get; }
    public IObservable<PmrVehicleTelemetry> VehicleTelemetryUpdates =>
        this.pmrUdpMessageHandler.VehicleTelemetryUpdates;

    public void Connect()
    {
        try
        {
            this.udpClient = this.CreateUdpClient();
            this.lastMessageReceivedAt = DateTime.UtcNow;
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
        if(this.isStopped)
        {
            return;
        }

        this.LogMessage(LoggingLevel.Information, "Disconnecting from Project Motor Racing...");
        this.isStopped = true;
        this.subscriptionSink.Dispose();
        this.udpClient?.Close();
        this.udpClient?.Dispose();
        this.udpClient = null;
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

    private UdpClient CreateUdpClient()
    {
        var client = new UdpClient(this.Port);
        if(this.UseMulticast)
        {
            client.JoinMulticastGroup(IPAddress.Parse(this.MulticastGroup));
        }

        return client;
    }

    private async Task HandleMessages()
    {
        this.LogMessage(LoggingLevel.Information,
            $"Listening for Project Motor Racing UDP telemetry on {this.ConnectionIdentifier}...");

        while(!this.isStopped)
        {
            await this.ProcessNextMessage();
        }
    }

    private void LogMessage(LoggingLevel loggingLevel, string content)
    {
        this.pmrUdpMessageHandler.LogMessage(loggingLevel, content, nameof(PmrUdpConnection));
    }

    private async Task ProcessNextMessage()
    {
        try
        {
            var udpReceiveResult = await this.udpClient.ReceiveAsync();
            this.lastMessageReceivedAt = DateTime.UtcNow;
            await using var stream = new MemoryStream(udpReceiveResult.Buffer);
            using var reader = new BinaryReader(stream);
            this.pmrUdpMessageHandler.ProcessMessage(reader);
        }
        catch(Exception exception)
        {
            if(this.isStopped)
            {
                return;
            }

            this.LogMessage(LoggingLevel.Error, $"Unexpected error processing message: {exception.Message}");
            this.Stop();
        }
    }

    private void StartDisconnectedWatcher()
    {
        var subscription = Observable.Interval(this.messageTimeout)
                                      .Subscribe(_ =>
                                                 {
                                                     if(this.isStopped)
                                                     {
                                                         return;
                                                     }

                                                     var timeSinceLastUpdate =
                                                         DateTime.UtcNow - this.lastMessageReceivedAt;
                                                     if(timeSinceLastUpdate <= this.messageTimeout)
                                                     {
                                                         return;
                                                     }

                                                     this.LogMessage(LoggingLevel.Warning,
                                                         "No Project Motor Racing UDP telemetry received recently. The session may have ended.");
                                                 });
        this.subscriptionSink.Add(subscription);
    }
}
