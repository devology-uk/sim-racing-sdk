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
using SimRacingSdk.Acc.Demo.Abstractions;
using SimRacingSdk.Acc.Udp.Abstractions;

namespace SimRacingSdk.Acc.Demo.Demos;

public class MonitorDemo : IMonitorDemo
{
    private readonly IAccCompatibilityChecker accCompatibilityChecker;
    private readonly IAccLocalConfigProvider accLocalConfigProvider;
    private readonly IAccPathProvider accPathProvider;
    private readonly IAccUdpConnectionFactory accUdpConnectionFactory;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<UdpDemo> logger;

    private IAccUdpConnection accUdpConnection = null!;
    private CompositeDisposable subscriptionSink = null!;

    public MonitorDemo(ILogger<UdpDemo> logger,
        IConsoleLog consoleLog,
        IAccCompatibilityChecker accCompatibilityChecker,
        IAccLocalConfigProvider accLocalConfigProvider,
        IAccPathProvider accPathProvider,
        IAccUdpConnectionFactory accUdpConnectionFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.accCompatibilityChecker = accCompatibilityChecker;
        this.accLocalConfigProvider = accLocalConfigProvider;

        this.accPathProvider = accPathProvider;
        this.accUdpConnectionFactory = accUdpConnectionFactory;
    }
    public void Start() { }
    public void Stop() { }

    public bool Validate()
    {
        return false;
    }
}