/*
 * This demo shows how to use the AccUdpConnection to receive and process messages
 * from the UDP Broadcasting interface provided by ACC.
 */

using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Acc.Demo.Abstractions;
using SimRacingSdk.Acc.Udp.Abstractions;
using SimRacingSdk.Acc.Udp.Messages;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Acc.Demo.Demos;

public class UdpDemo : IUdpDemo
{
    private readonly IAccCompatibilityChecker accCompatibilityChecker;
    private readonly IAccLocalConfigProvider accLocalConfigProvider;
    private readonly IAccPathProvider accPathProvider;
    private readonly IAccUdpConnectionFactory accUdpConnectionFactory;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<UdpDemo> logger;

    private IAccUdpConnection? accUdpConnection;
    private CompositeDisposable subscriptionSink = null!;

    public UdpDemo(ILogger<UdpDemo> logger,
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

    public void Start()
    {
        this.Stop();
        this.Log("Starting UDP Demo...");

        var broadcastSettings = this.accLocalConfigProvider.GetBroadcastingSettings()!;
        this.accUdpConnection = this.accUdpConnectionFactory.Create("127.0.0.1",
            broadcastSettings.UdpListenerPort,
            "Sim Racing SDK ACC UDP Demo",
            broadcastSettings.ConnectionPassword,
            broadcastSettings.CommandPassword);

        this.PrepareUdpMessageHandling();
        this.accUdpConnection.Connect();
    }

    public void Stop()
    {
        if(this.accUdpConnection == null)
        {
            return;
        }

        this.Log("Stopping UDP Demo...");
        this.subscriptionSink?.Dispose();
        this.accUdpConnection?.Dispose();
        this.accUdpConnection = null!;
    }

    public bool Validate()
    {
        this.Log("Validating UDP Demo...");
        if (this.accCompatibilityChecker.HasValidBroadcastingSettings())
        {
            return true;
        }

        this.Log(
            $"ACC broadcasting has not been configured.  Please ensure at least the updListenerPort in {this.accPathProvider.BroadcastingSettingsFilePath} has been set to a value > 1023 and try again.");
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

    private void OnNexLogMessage(LogMessage logMessage)
    {
        this.Log(logMessage.ToString());
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
        if(this.accUdpConnection == null)
        {
            return;
        }

        this.subscriptionSink = new CompositeDisposable()
        {
            this.accUdpConnection.BroadcastingEvents.Subscribe(this.LogBroadcastingEvent),
            this.accUdpConnection.ConnectionStateChanges.Subscribe(this.OnNextConnectionStateChange),
            this.accUdpConnection.EntryListUpdates.Subscribe(this.OnNextEntryListUpdate),
            this.accUdpConnection.RealTimeCarUpdates.Subscribe(this.OnNextRealtimeCarUpdate),
            this.accUdpConnection.RealTimeUpdates.Subscribe(this.OnNextRealtimeUpdate),
            this.accUdpConnection.TrackDataUpdates.Subscribe(this.OnNextTrackDataUpdate),
            this.accUdpConnection.LogMessages.Subscribe(this.OnNexLogMessage)
        };
    }
}