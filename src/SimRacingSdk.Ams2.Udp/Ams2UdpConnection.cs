using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Ams2.Udp.Abstractions;

namespace SimRacingSdk.Ams2.Udp;

public class Ams2UdpConnection : IAms2UdpConnection
{
    private readonly Ams2UdpMessageHandler ams2UdpMessageHandler;
    private readonly IPEndPoint ipEndPoint;
    private readonly CompositeDisposable subscriptionSink = new();

    private bool isConnected;
    private bool isDisposed;
    private bool isStopped;
    private UdpClient? udpClient;

    public Ams2UdpConnection(string ipAddress, int port)
    {
        this.IpAddress = ipAddress;
        this.Port = port;
        this.ConnectionIdentifier = $"{this.IpAddress}:{this.Port}";
        this.ipEndPoint = IPEndPoint.Parse(this.ConnectionIdentifier);

        this.ams2UdpMessageHandler = new Ams2UdpMessageHandler(this.ConnectionIdentifier);
    }

    public string ConnectionIdentifier { get; }
    public string IpAddress { get; }
    public int Port { get; }
    public IObservable<LogMessage> LogMessages => this.ams2UdpMessageHandler.LogMessages;

    public void Connect(bool autoDetect = true)
    {
        if(autoDetect)
        {
            this.WaitUntilConnected();
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

    private void LogMessage(LoggingLevel loggingLevel, string message, object? data = null)
    {
        this.ams2UdpMessageHandler.LogMessage(loggingLevel, message, data);
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

    private void WaitUntilConnected()
    {
        this.LogMessage(LoggingLevel.Information, "Waiting for AMS2 connection...");
        var isRegistered = false;
        while(!isRegistered)
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
                    var receiveBytes = client.Receive(ref endPoint);

                    using var stream = new MemoryStream(receiveBytes);
                    using var reader = new BinaryReader(stream);
                    isRegistered = true;
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