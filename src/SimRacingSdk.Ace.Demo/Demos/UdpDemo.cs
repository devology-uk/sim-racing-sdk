/*
 * This demo shows how to use the AceUdpConnection to receive and process messages
 * from the UDP Broadcasting interface provided by ACE.
 */

using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Demo.Abstractions;
using SimRacingSdk.Ace.Udp.Abstractions;
using SimRacingSdk.Ace.Udp.Messages;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Wpf.Shared.Logging;

namespace SimRacingSdk.Ace.Demo.Demos;

public class UdpDemo : IUdpDemo
{
    private readonly IAceCompatibilityChecker aceCompatibilityChecker;
    private readonly IAceLocalConfigProvider aceLocalConfigProvider;
    private readonly IAcePathProvider acePathProvider;
    private readonly IAceUdpConnectionFactory aceUdpConnectionFactory;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<UdpDemo> logger;
    private readonly IUdpLog udpLog;

    private IAceUdpConnection? aceUdpConnection;
    private CompositeDisposable subscriptionSink = null!;

    public UdpDemo(ILogger<UdpDemo> logger,
        IConsoleLog consoleLog,
        IUdpLog udpLog,
        IAceCompatibilityChecker aceCompatibilityChecker,
        IAceLocalConfigProvider aceLocalConfigProvider,
        IAcePathProvider acePathProvider,
        IAceUdpConnectionFactory aceUdpConnectionFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.udpLog = udpLog;
        this.aceCompatibilityChecker = aceCompatibilityChecker;
        this.aceLocalConfigProvider = aceLocalConfigProvider;

        this.acePathProvider = acePathProvider;
        this.aceUdpConnectionFactory = aceUdpConnectionFactory;
    }

    public void Start()
    {
        this.Stop();
        this.Log("Starting UDP Demo...");

        var broadcastSettings = this.aceLocalConfigProvider.GetBroadcastingSettings()!;
        this.aceUdpConnection = this.aceUdpConnectionFactory.Create("127.0.0.1",
            broadcastSettings.UdpListenerPort,
            "Sim Racing SDK ACE UDP Demo",
            broadcastSettings.ConnectionPassword,
            broadcastSettings.CommandPassword);

        this.PrepareUdpMessageHandling();
        this.aceUdpConnection.Connect();
    }

    public void Stop()
    {
        if(this.aceUdpConnection == null)
        {
            return;
        }

        this.Log("Stopping UDP Demo...");
        this.subscriptionSink?.Dispose();
        this.aceUdpConnection?.Dispose();
        this.aceUdpConnection = null!;
    }

    public bool Validate()
    {
        this.Log("Validating UDP Demo...");
        if (this.aceCompatibilityChecker.HasValidBroadcastingSettings())
        {
            return true;
        }

        this.Log(
            $"ACE broadcasting has not been configured.  Please ensure at least the updListenerPort in {this.acePathProvider.BroadcastingSettingsFilePath} has been set to a value > 1023 and try again.");
        return false;
    }

    private void Log(string message)
    {
        this.logger.LogInformation(message);
        this.consoleLog.Write(message);
    }

    private void LogBroadcastingEvent(BroadcastingEvent broadcastingEvent)
    {
        this.Log(broadcastingEvent.ToString());
    }

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.udpLog.Log(logMessage);
    }

    private void OnNextConnectionStateChange(Connection connection)
    {
        this.Log(connection.ToString());
    }

    private void OnNextEntryListUpdate(EntryListUpdate entryListUpdate)
    {
        this.Log(entryListUpdate.ToString());
    }

    private void OnNextRealtimeCarUpdate(RealtimeCarUpdate realtimeCarUpdate)
    {
        this.Log(realtimeCarUpdate.ToString());
    }

    private void OnNextRealtimeUpdate(RealtimeUpdate realtimeUpdate)
    {
        this.Log(realtimeUpdate.ToString());
    }

    private void OnNextTrackDataUpdate(TrackDataUpdate trackDataUpdate)
    {
        this.Log(trackDataUpdate.ToString());
    }

    private void PrepareUdpMessageHandling()
    {
        if(this.aceUdpConnection == null)
        {
            return;
        }

        this.subscriptionSink = new CompositeDisposable()
        {
            this.aceUdpConnection.BroadcastingEvents.Subscribe(this.LogBroadcastingEvent),
            this.aceUdpConnection.ConnectionStateChanges.Subscribe(this.OnNextConnectionStateChange),
            this.aceUdpConnection.EntryListUpdates.Subscribe(this.OnNextEntryListUpdate),
            this.aceUdpConnection.RealTimeCarUpdates.Subscribe(this.OnNextRealtimeCarUpdate),
            this.aceUdpConnection.RealTimeUpdates.Subscribe(this.OnNextRealtimeUpdate),
            this.aceUdpConnection.TrackDataUpdates.Subscribe(this.OnNextTrackDataUpdate),
            this.aceUdpConnection.LogMessages.Subscribe(this.OnNextLogMessage)
        };
    }
}
