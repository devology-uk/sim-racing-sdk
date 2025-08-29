using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Acc.Core;
using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Acc.Monitor.Abstractions;
using SimRacingSdk.Acc.Monitor.Exceptions;
using SimRacingSdk.Acc.Monitor.Messages;
using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.Udp.Abstractions;
using SimRacingSdk.Acc.Udp.Messages;

namespace SimRacingSdk.Acc.Monitor;

public class AccMonitor : IAccMonitor
{
    private const string LocalhostIpAddress = "127.0.0.1";

    private readonly IAccCompatibilityChecker accCompatibilityChecker;
    private readonly IAccLocalConfigProvider accLocalConfigProvider;
    private readonly IAccCarInfoProvider accCarInfoProvider;
    private readonly IAccTelemetryConnectionFactory accTelemetryConnectionFactory;
    private readonly IAccUdpConnectionFactory accUdpConnectionFactory;
    private readonly Subject<ConnectionState> connectionStateChangesSubject = new();
    private readonly Subject<LogMessage> logMessagesSubject = new();
    private readonly ReplaySubject<AccEvent> currentEventSubject = new();

    private IAccTelemetryConnection? accTelemetryConnection;
    private IAccUdpConnection? accUdpConnection;
    private CompositeDisposable? subscriptionSink;
    private AccEvent? currentEvent;

    public AccMonitor(IAccUdpConnectionFactory accUdpConnectionFactory,
        IAccTelemetryConnectionFactory accTelemetryConnectionFactory,
        IAccCompatibilityChecker accCompatibilityChecker,
        IAccLocalConfigProvider accLocalConfigProvider,
        IAccCarInfoProvider accCarInfoProvider)
    {
        this.accUdpConnectionFactory = accUdpConnectionFactory;
        this.accTelemetryConnectionFactory = accTelemetryConnectionFactory;
        this.accCompatibilityChecker = accCompatibilityChecker;
        this.accLocalConfigProvider = accLocalConfigProvider;
        this.accCarInfoProvider = accCarInfoProvider;
    }

    public IObservable<ConnectionState> ConnectionStateChanges =>
        this.connectionStateChangesSubject.AsObservable();

    public IObservable<AccEvent> CurrentEvent => this.currentEventSubject.AsObservable();

    public IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Start(string? connectionIdentifier = null)
    {
        this.LogMessage(LoggingLevel.Information,
            $"Starting ACC Monitor connection wit ID: {connectionIdentifier}");

        if(!this.accCompatibilityChecker.HasValidBroadcastingSettings())
        {
            this.LogMessage(LoggingLevel.Error,
                "ACC Monitor cannot start because ACC has not been configured for broadcasting.");
            throw new InvalidBroadcastingSettingsException();
        }

        this.LogMessage(LoggingLevel.Information, "Preparing connection to ACC UDP interface.");
        var broadcastingSettings = this.accLocalConfigProvider.GetBroadcastingSettings()!;
        this.accUdpConnection = this.accUdpConnectionFactory.Create(LocalhostIpAddress,
            broadcastingSettings.UdpListenerPort,
            connectionIdentifier ?? $"{LocalhostIpAddress}:{broadcastingSettings.UdpListenerPort}",
            broadcastingSettings.ConnectionPassword,
            broadcastingSettings.CommandPassword);

        this.LogMessage(LoggingLevel.Information, "Preparing connection to ACC Shared Memory interface.");
        this.accTelemetryConnection = this.accTelemetryConnectionFactory.Create();

        this.PrepareMessageProcessing();

        this.accUdpConnection.Connect();
    }

    public void Stop()
    {
        this.subscriptionSink?.Dispose();
        this.accUdpConnection?.Dispose();
        this.accUdpConnection = null;
        this.accTelemetryConnection?.Dispose();
        this.accTelemetryConnection = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!disposing)
        {
            return;
        }

        this.Stop();
    }

    private void LogMessage(LoggingLevel level, string message, object? data = null)
    {
        this.logMessagesSubject.OnNext(new LogMessage(level, message, data));
    }

    private void OnNextRealTimeUpdate(RealtimeUpdate realtimeUpdate)
    {
        var sessionType = realtimeUpdate.SessionType.ToFriendlyName();
        var sessionPhase = realtimeUpdate.Phase.ToFriendlyName();
    }

    private void OnNextTrackDataUpdate(TrackDataUpdate trackDataUpdate)
    {
        this.LogMessage(LoggingLevel.Information, $"Received Track Data Update: {trackDataUpdate}");
        this.currentEvent = new AccEvent(trackDataUpdate.TrackId,
            trackDataUpdate.TrackName,
            trackDataUpdate.TrackMeters);
        this.currentEventSubject.OnNext(this.currentEvent);
    }

    private void PrepareMessageProcessing()
    {
        this.subscriptionSink = new CompositeDisposable
        {
            this.accUdpConnection!.LogMessages.Subscribe(m => this.logMessagesSubject.OnNext(m)),
            this.accUdpConnection!.ConnectionStateChanges.Subscribe(
                m => this.connectionStateChangesSubject.OnNext(m)),
            this.accUdpConnection.EntryListUpdates.Subscribe(this.OnNextEntryListUpdate),
            this.accUdpConnection!.RealTimeUpdates.Subscribe(this.OnNextRealTimeUpdate),
            this.accUdpConnection!.TrackDataUpdates.Subscribe(this.OnNextTrackDataUpdate)
        };
    }
    private  void OnNextEntryListUpdate(EntryListUpdate entryListUpdate)
    {
        this.LogMessage(LoggingLevel.Information, $"Received Entry List Update: {entryListUpdate}");
        var accEventEntry = new AccEventEntry
        {
            Car = entryListUpdate.CarInfo,

        };

    }
}