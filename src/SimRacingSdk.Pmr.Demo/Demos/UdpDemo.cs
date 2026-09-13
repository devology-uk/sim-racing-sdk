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

    private IPmrUdpConnection? pmrUdpConnection;
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

    public void Start()
    {
        this.Stop();
        this.Log("Starting UDP Demo...");

        var (port, useMulticast, multicastGroup) = this.ResolveConnectionSettings();
        this.pmrUdpConnection = this.pmrUdpConnectionFactory.Create(port, useMulticast, multicastGroup);

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
        if(settings == null)
        {
            this.Log(
                "Could not read Project Motor Racing's local settings - falling back to the SDK defaults (127.0.0.1:7576).");
            return true;
        }

        if(!settings.UdpEnabled)
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

    private (int Port, bool UseMulticast, string MulticastGroup) ResolveConnectionSettings()
    {
        var settings = this.pmrLocalConfigProvider.GetLocalSettings();
        if(settings == null || settings.UdpPort <= 0)
        {
            return (PmrUdpConnection.DefaultPort, false, PmrUdpConnection.DefaultMulticastGroup);
        }

        // UDPHost is where the game sends its telemetry to - normally the loopback address
        // (127.0.0.1), but a user can point it at a multicast-range address instead, in which
        // case we need to join that group rather than just listen locally.
        var useMulticast = IsMulticastAddress(settings.UdpHost);
        this.Log($"Using Project Motor Racing's own UDP settings: Host={settings.UdpHost}, Port={settings.UdpPort}, Frequency={settings.UdpFrequencyHz}Hz.");

        return (settings.UdpPort, useMulticast, useMulticast ? settings.UdpHost : PmrUdpConnection.DefaultMulticastGroup);
    }
}
