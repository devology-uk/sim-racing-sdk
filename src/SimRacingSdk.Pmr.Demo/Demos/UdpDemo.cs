/*
 * This demo shows how to use the PmrUdpConnection to receive and process messages
 * from the UDP telemetry broadcast by Project Motor Racing.
 */

using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Pmr.Core.Abstractions;
using SimRacingSdk.Pmr.Demo.Abstractions;
using SimRacingSdk.Pmr.Udp;
using SimRacingSdk.Pmr.Udp.Abstractions;
using SimRacingSdk.Pmr.Udp.Messages;
using SimRacingSdk.Wpf.Shared.Logging;

namespace SimRacingSdk.Pmr.Demo.Demos;

public class UdpDemo : IUdpDemo
{
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<UdpDemo> logger;
    private readonly IPmrLocalConfigProvider pmrLocalConfigProvider;
    private readonly IPmrUdpConnectionFactory pmrUdpConnectionFactory;
    private readonly IUdpLog udpLog;

    private string host = PmrUdpConnection.DefaultHost;
    private IPmrUdpConnection? pmrUdpConnection;
    private int port = PmrUdpConnection.DefaultPort;
    private CompositeDisposable subscriptionSink = null!;

    public UdpDemo(ILogger<UdpDemo> logger,
        IConsoleLog consoleLog,
        IUdpLog udpLog,
        IPmrLocalConfigProvider pmrLocalConfigProvider,
        IPmrUdpConnectionFactory pmrUdpConnectionFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.udpLog = udpLog;
        this.pmrLocalConfigProvider = pmrLocalConfigProvider;
        this.pmrUdpConnectionFactory = pmrUdpConnectionFactory;
    }

    public void Configure(string udpHost, int udpPort)
    {
        this.host = udpHost;
        this.port = udpPort;
    }

    public void Start()
    {
        this.Stop();
        this.Log($"Starting UDP Demo, listening on Host={this.host}, Port={this.port}...");

        // Host is where the game (or a relay tool like SimHub, for the "several apps want the
        // one UDP stream" case) sends telemetry to - normally a plain unicast address, but a
        // multicast-range address here means we need to join that group instead of just
        // listening locally.
        var useMulticast = IsMulticastAddress(this.host);
        this.pmrUdpConnection = this.pmrUdpConnectionFactory.Create(this.port,
            useMulticast,
            useMulticast ? this.host : PmrUdpConnection.DefaultMulticastGroup);

        this.PrepareUdpMessageHandling();
        this.pmrUdpConnection.Connect();
    }

    public void Stop()
    {
        if(this.pmrUdpConnection == null)
        {
            return;
        }

        this.Log("Stopping UDP Demo...");
        this.subscriptionSink?.Dispose();
        this.pmrUdpConnection?.Dispose();
        this.pmrUdpConnection = null;
    }

    public bool Validate()
    {
        var settings = this.pmrLocalConfigProvider.GetLocalSettings();
        if(settings is { UdpEnabled: false })
        {
            this.Log(
                "UDP is currently disabled in Project Motor Racing (Settings > Preferences > UDP Enabled) - no telemetry will arrive until it's turned on.");
        }

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

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.udpLog.Log(logMessage);
    }

    private void OnNextParticipantRaceState(PmrParticipantRaceState participantRaceState)
    {
        this.Log(participantRaceState.ToString());
    }

    private void OnNextRaceInfo(PmrRaceInfo raceInfo)
    {
        this.Log(raceInfo.ToString());
    }

    private void OnNextSessionStopped(PmrSessionStopped sessionStopped)
    {
        this.Log(sessionStopped.ToString());
    }

    private void OnNextVehicleTelemetry(PmrVehicleTelemetry vehicleTelemetry)
    {
        this.Log(vehicleTelemetry.ToString());
    }

    private void PrepareUdpMessageHandling()
    {
        if(this.pmrUdpConnection == null)
        {
            return;
        }

        this.subscriptionSink = new CompositeDisposable
        {
            this.pmrUdpConnection.RaceInfoUpdates.Subscribe(this.OnNextRaceInfo),
            this.pmrUdpConnection.ParticipantRaceStates.Subscribe(this.OnNextParticipantRaceState),
            this.pmrUdpConnection.VehicleTelemetryUpdates.Subscribe(this.OnNextVehicleTelemetry),
            this.pmrUdpConnection.SessionStoppedUpdates.Subscribe(this.OnNextSessionStopped),
            this.pmrUdpConnection.LogMessages.Subscribe(this.OnNextLogMessage)
        };
    }
}
