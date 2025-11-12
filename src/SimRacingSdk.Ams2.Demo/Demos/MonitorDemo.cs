using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Ams2.Core.Abstractions;
using SimRacingSdk.Ams2.Demo.Abstractions;
using SimRacingSdk.Ams2.Monitor.Abstractions;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ams2.Demo.Demos;

public class MonitorDemo : IMonitorDemo
{
    private readonly IAms2CompatibilityChecker ams2CompatibilityChecker;
    private readonly IAms2MonitorFactory ams2MonitorFactory;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<IMonitorDemo> logger;
    private readonly IMonitorLog monitorLog;
    private IAms2Monitor? ams2Monitor;
    private CompositeDisposable? subscriptionSink;

    public MonitorDemo(ILogger<IMonitorDemo> logger,
        IConsoleLog consoleLog,
        IMonitorLog monitorLog,
        IAms2CompatibilityChecker ams2CompatibilityChecker,
        IAms2MonitorFactory ams2MonitorFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.monitorLog = monitorLog;
        this.ams2CompatibilityChecker = ams2CompatibilityChecker;
        this.ams2MonitorFactory = ams2MonitorFactory;
    }

    public void Start()
    {
        this.Stop();
        this.Log("Starting Monitor Demo...");

        this.ams2Monitor = this.ams2MonitorFactory.Create();
        this.PrepareMessageHandling();
        this.ams2Monitor.Start();
    }

    public void Stop()
    {
        if(this.ams2Monitor == null)
        {
            return;
        }

        this.Log("Stopping Monitor Demo...");
        this.subscriptionSink?.Dispose();
        this.ams2Monitor?.Dispose();
        this.ams2Monitor = null;
    }

    public bool Validate()
    {
        return this.ams2CompatibilityChecker.IsAms2Installed();
    }

    private void Log(string message)
    {
        this.logger.LogInformation(message);
        this.consoleLog.Write(message);
    }

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.monitorLog.Log(logMessage.ToString());
    }

    private void PrepareMessageHandling()
    {
        if(this.ams2Monitor == null)
        {
            return;
        }

        this.subscriptionSink = new CompositeDisposable
        {
            this.ams2Monitor.LogMessages.Subscribe(this.OnNextLogMessage)
        };
    }
}