/*
 * This demo provides an example of how to use the AccMonitor component, which combines the AccUdpConnection and AccTelemetryConnection components
 * to provide data objects an application could use.
 *
 * The demo simply logs entities but in a real application you might want to save these entities in a database
 * and display the data.
 *
 * Bear in mind that the UDP Broadcasting interface provides data for all drivers on track, while the Shared Memory interface only
 * provides data for the player car.  Therefore, telemetry can only be captured for laps completed by the current player on the computer running
 * an application that uses this monitoring component.
 */

using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Acc.Demo.Abstractions;
using SimRacingSdk.Acc.Monitor.Abstractions;

namespace SimRacingSdk.Acc.Demo.Demos;

public class MonitorDemo : IMonitorDemo
{
    private readonly IAccMonitorFactory accMonitorFactory;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<UdpDemo> logger;
    private IAccMonitor? accMonitor;

    private CompositeDisposable? subscriptionSink;

    public MonitorDemo(ILogger<UdpDemo> logger, IConsoleLog consoleLog, IAccMonitorFactory accMonitorFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.accMonitorFactory = accMonitorFactory;
    }

    public void Start()
    {
        this.Stop();
        this.Log("Starting ACC Monitor Demo...");
        this.accMonitor = this.accMonitorFactory.Create();

        this.subscriptionSink = new CompositeDisposable
        {
            this.accMonitor.LogMessages.Subscribe(this.OnNextLogMessage)
        };

        this.accMonitor.Start("ACC Monitor Demo");
    }

    public void Stop()
    {
        if(this.accMonitor == null)
        {
            return;
        }

        this.Log("Stopping ACC Monitor Demo...");
        this.subscriptionSink?.Dispose();
        this.accMonitor?.Stop();
        this.accMonitor = null;
    }

    public bool Validate()
    {
        return false;
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
}