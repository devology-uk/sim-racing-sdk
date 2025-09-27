using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Ams2.Core.Abstractions;
using SimRacingSdk.Ams2.Demo.Abstractions;
using SimRacingSdk.Ams2.Udp.Abstractions;

namespace SimRacingSdk.Ams2.Demo.Demos;

public class UdpDemo : IUdpDemo
{
    private readonly IAms2CompatibilityChecker ams2CompatibilityChecker;
    private readonly IAms2UdpConnectionFactory ams2UdpConnectionFactory;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<UdpDemo> logger;

    private IAms2UdpConnection? ams2UdpConnection;
    private CompositeDisposable subscriptionSink = null!;

    public UdpDemo(ILogger<UdpDemo> logger,
        IConsoleLog consoleLog,
        IAms2CompatibilityChecker ams2CompatibilityChecker,
        IAms2UdpConnectionFactory ams2UdpConnectionFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.ams2CompatibilityChecker = ams2CompatibilityChecker;
        this.ams2UdpConnectionFactory = ams2UdpConnectionFactory;
    }

    public void Start()
    {
        this.Stop();
        this.Log("Starting UDP Demo...");

        this.ams2UdpConnection = this.ams2UdpConnectionFactory.Create("127.0.0.1", 5606);

        this.PrepareUdpMessageHandling();
        this.ams2UdpConnection.Connect();
    }

    public void Stop()
    {
        if(this.ams2UdpConnection == null)
        {
            return;
        }

        this.Log("Stopping UDP Demo...");
        this.subscriptionSink?.Dispose();
        this.ams2UdpConnection?.Dispose();
        this.ams2UdpConnection = null!;
    }

    public bool Validate()
    {
        return true;
    }

    private void Log(string message)
    {
        this.logger.LogInformation(message);
        this.consoleLog.Write(message);
    }

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.Log(logMessage.ToString());
    }

    private void PrepareUdpMessageHandling()
    {
        if(this.ams2UdpConnection == null)
        {
            return;
        }

        this.subscriptionSink = new CompositeDisposable
        {
            this.ams2UdpConnection.LogMessages.Subscribe(this.OnNextLogMessage)
        };
    }
}