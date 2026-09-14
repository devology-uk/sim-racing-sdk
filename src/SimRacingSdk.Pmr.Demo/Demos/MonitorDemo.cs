using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Pmr.Demo.Abstractions;
using SimRacingSdk.Pmr.Monitor.Abstractions;
using SimRacingSdk.Pmr.Monitor.Messages;
using SimRacingSdk.Pmr.Udp;
using SimRacingSdk.Pmr.Udp.Messages;
using SimRacingSdk.Wpf.Shared.Logging;

namespace SimRacingSdk.Pmr.Demo.Demos;

public class MonitorDemo : IMonitorDemo
{
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<MonitorDemo> logger;
    private readonly IMonitorLog monitorLog;
    private readonly IPmrMonitorFactory pmrMonitorFactory;

    private string host = PmrUdpConnection.DefaultHost;
    private int participantUpdateCount;
    private IPmrMonitor? pmrMonitor;
    private int port = PmrUdpConnection.DefaultPort;
    private CompositeDisposable? subscriptionSink;
    private int telemetryFrameCount;

    public MonitorDemo(ILogger<MonitorDemo> logger,
        IConsoleLog consoleLog,
        IMonitorLog monitorLog,
        IPmrMonitorFactory pmrMonitorFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.monitorLog = monitorLog;
        this.pmrMonitorFactory = pmrMonitorFactory;
    }

    public void Configure(string udpHost, int udpPort)
    {
        this.host = udpHost;
        this.port = udpPort;
    }

    public void Start()
    {
        this.Stop();
        this.telemetryFrameCount = 0;
        this.participantUpdateCount = 0;
        this.Log($"Starting PMR Monitor Demo, listening on Host={this.host}, Port={this.port}...");

        // Host is where the game (or a relay tool like SimHub) sends telemetry to - see
        // UdpDemo's own comment for why this needs to be resolved to a multicast join or not.
        var useMulticast = IsMulticastAddress(this.host);
        this.pmrMonitor = this.pmrMonitorFactory.Create();
        this.PrepareMessageHandling();
        this.pmrMonitor.Start(this.port,
            useMulticast,
            useMulticast ? this.host : PmrUdpConnection.DefaultMulticastGroup);
    }

    public void Stop()
    {
        if(this.pmrMonitor == null)
        {
            return;
        }

        this.Log("Stopping PMR Monitor Demo...");
        this.Log($"Total Telemetry Frames: {this.telemetryFrameCount}, Participant Updates: {this.participantUpdateCount}");
        this.subscriptionSink?.Dispose();
        this.pmrMonitor?.Dispose();
        this.pmrMonitor = null;
    }

    public bool Validate()
    {
        return true;
    }

    private static bool IsMulticastAddress(string host)
    {
        return IPAddress.TryParse(host, out var address)
               && address.AddressFamily == AddressFamily.InterNetwork
               && address.GetAddressBytes()[0] is >= 224 and <= 239;
    }

    private void Log(string message)
    {
        this.logger.LogInformation(message);
        this.consoleLog.Write(message);
    }

    private void OnNextCompletedLap(PmrMonitorLap pmrMonitorLap)
    {
        this.Log(pmrMonitorLap.ToString());
    }

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.monitorLog.Log(logMessage);
    }

    private void OnNextParticipantUpdate(PmrParticipantRaceState participantRaceState)
    {
        this.participantUpdateCount++;
    }

    private void OnNextSessionCompleted(PmrMonitorSession pmrMonitorSession)
    {
        this.Log($"Session Completed: {pmrMonitorSession}");
    }

    private void OnNextSessionStarted(PmrMonitorSession pmrMonitorSession)
    {
        this.Log($"Session Started: {pmrMonitorSession}");
    }

    private void OnNextTelemetryFrame(PmrVehicleTelemetry pmrVehicleTelemetry)
    {
        this.telemetryFrameCount++;
    }

    private void PrepareMessageHandling()
    {
        if(this.pmrMonitor == null)
        {
            return;
        }

        this.subscriptionSink = new CompositeDisposable
        {
            this.pmrMonitor.LapCompleted.Subscribe(this.OnNextCompletedLap),
            this.pmrMonitor.LogMessages.Subscribe(this.OnNextLogMessage),
            this.pmrMonitor.ParticipantUpdates.Subscribe(this.OnNextParticipantUpdate),
            this.pmrMonitor.SessionCompleted.Subscribe(this.OnNextSessionCompleted),
            this.pmrMonitor.SessionStarted.Subscribe(this.OnNextSessionStarted),
            this.pmrMonitor.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };
    }
}
