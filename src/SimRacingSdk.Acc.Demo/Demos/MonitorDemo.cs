/*
 * This demo provides and example of how you might combine the AccUdpConnection with the AccTelemetryConnection to
 * create a monitor within an application that captures and displays driving sessions and completed laps in real-time.
 *
 * The demo simply logs entities but in a real application you might want to save these entities in a database
 * and display the data.
 *
 * Bear in mind that the UDP Broadcasting interface provides data for all drivers on track, while the Shared Memory interface only
 * provides data for the player car.  Therefore, telemetry can only be captured for laps completed by the current player on the computer running
 * an application that uses this approach.
 */

using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Acc.Demo.Abstractions;
using SimRacingSdk.Acc.Monitor.Abstractions;
using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.Udp.Abstractions;

namespace SimRacingSdk.Acc.Demo.Demos;

public class MonitorDemo : IMonitorDemo
{
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<UdpDemo> logger;

    private CompositeDisposable? subscriptionSink;
    private bool isRunning;
    private readonly IAccMonitorFactory accMonitorFactory;
    private IAccMonitor? accMonitor;

    public MonitorDemo(ILogger<UdpDemo> logger,
        IConsoleLog consoleLog,
        IAccMonitorFactory accMonitorFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.accMonitorFactory = accMonitorFactory;
    }

    public void Start()
    {
        this.Stop();
        this.Log("Starting ACC Monitor Demo...");
        this.accMonitor = this.accMonitorFactory.Create("ACC Monitor Demo");
        
        this.subscriptionSink = new CompositeDisposable
        {
            this.accMonitor.LogMessages.Subscribe(this.OnNextLogMessage)
        };

        this.accMonitor.Start("ACC Monitor Demo");
        this.isRunning = true;
    }

    public void Stop()
    {
        if(!this.isRunning)
        {
            return;
        }

        this.Log("Stopping ACC Monitor Demo...");
        this.subscriptionSink?.Dispose();
        this.accMonitor?.Stop();
        this.isRunning = false;
    }

    public bool Validate()
    {
        return false;
    }

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.Log(logMessage.ToString());
    }
     
    private void Log(string message)
    {
        this.logger.LogInformation(message);
        this.consoleLog.Write(message);
    }
}