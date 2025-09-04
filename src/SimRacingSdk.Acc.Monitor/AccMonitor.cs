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
using SimRacingSdk.Acc.Udp.Enums;
using SimRacingSdk.Acc.Udp.Messages;

namespace SimRacingSdk.Acc.Monitor;

public class AccMonitor : IAccMonitor
{
    private const string LocalhostIpAddress = "127.0.0.1";
    private readonly IAccCarInfoProvider accCarInfoProvider;
    private readonly IAccCompatibilityChecker accCompatibilityChecker;
    private readonly IAccLocalConfigProvider accLocalConfigProvider;
    private readonly IAccNationalityInfoProvider accNationalityInfoProvider;
    private readonly IAccTelemetryConnectionFactory accTelemetryConnectionFactory;
    private readonly IAccUdpConnectionFactory accUdpConnectionFactory;
    private readonly ReplaySubject<AccEvent> eventStartedSubject = new();
    private readonly List<AccEventEntry> entryList = [];
    private readonly ReplaySubject<IList<AccEventEntry>> entryListSubject = new();
    private readonly ReplaySubject<AccEvent> eventEndedSubject = new();
    private readonly ReplaySubject<AccEventEntry> eventEntriesSubject = new();
    private readonly ReplaySubject<LogMessage> logMessagesSubject = new();
    private readonly ReplaySubject<AccSession> sessionStartedSubject = new();
    private readonly ReplaySubject<AccSession> sessionEndedSubject = new();
    private readonly ReplaySubject<AccSessionPhase> phaseStartedSubject = new();
    private readonly ReplaySubject<AccSessionPhase> phaseEndedSubject = new();

    private IAccTelemetryConnection? accTelemetryConnection;
    private IAccUdpConnection? accUdpConnection;
    private AccEvent? currentEvent;
    private AccSession? currentSession;
    private CompositeDisposable? subscriptionSink;
    private AccSessionPhase? currentPhase;

    public AccMonitor(IAccUdpConnectionFactory accUdpConnectionFactory,
        IAccTelemetryConnectionFactory accTelemetryConnectionFactory,
        IAccCompatibilityChecker accCompatibilityChecker,
        IAccLocalConfigProvider accLocalConfigProvider,
        IAccCarInfoProvider accCarInfoProvider,
        IAccNationalityInfoProvider accNationalityInfoProvider)
    {
        this.accUdpConnectionFactory = accUdpConnectionFactory;
        this.accTelemetryConnectionFactory = accTelemetryConnectionFactory;
        this.accCompatibilityChecker = accCompatibilityChecker;
        this.accLocalConfigProvider = accLocalConfigProvider;
        this.accCarInfoProvider = accCarInfoProvider;
        this.accNationalityInfoProvider = accNationalityInfoProvider;
    }

    public IObservable<IList<AccEventEntry>> EntryList => this.entryListSubject.AsObservable();
    public IObservable<AccEvent> EventEnded => this.eventEndedSubject.AsObservable();
    public IObservable<AccEventEntry> EventEntries => this.eventEntriesSubject.AsObservable();
    public IObservable<AccEvent> EventStarted => this.eventStartedSubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();
    public IObservable<AccSession> SessionStarted => this.sessionStartedSubject.AsObservable();
    public IObservable<AccSession> SessionEnded => this.sessionEndedSubject.AsObservable();
    public IObservable<AccSessionPhase> PhaseStarted => this.phaseStartedSubject.AsObservable();
    public IObservable<AccSessionPhase> PhaseEnded => this.phaseEndedSubject.AsObservable();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Start(string? connectionIdentifier = null)
    {
        this.LogMessage(LoggingLevel.Information,
            $"Starting ACC Monitor connection with ID: {connectionIdentifier}");

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

        this.LogMessage(LoggingLevel.Information,
            "Preparing connection to ACC Shared Memory interface for telemetry.");
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

    private void OnNextConnectionStateChange(ConnectionState connectionState)
    {
        this.LogMessage(LoggingLevel.Information, connectionState.ToString());
        if(connectionState.IsConnected)
        {
            this.accUdpConnection?.RequestTrackData();
        }
        else if(this.currentEvent != null)
        {
            this.eventEndedSubject.OnNext(this.currentEvent);
        }
    }

    private void OnNextEntryListUpdate(EntryListUpdate entryListUpdate)
    {
        this.LogMessage(LoggingLevel.Information, entryListUpdate.ToString());
        var carInfo = entryListUpdate.CarInfo;
        var car = this.accCarInfoProvider.FindByModelId(carInfo.CarModelType);
        var drivers = carInfo.Drivers.Select(d => new AccDriverEntry(d.FirstName,
                                                 d.LastName,
                                                 d.ShortName,
                                                 d.Category.ToString(),
                                                 this.accNationalityInfoProvider
                                                     .GetCountryCode(d.Nationality)))
                             .ToList();
        var eventEntry = new AccEventEntry
        {
            AccCarModelId = carInfo.CarModelType,
            CarManufacturer = car!.ManufacturerTag,
            CarModelName = car.DisplayName,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            Drivers = drivers,
            EventId = this.currentEvent!.Id,
            Index = carInfo.CarIndex,
            RaceNumber = carInfo.RaceNumber,
            TeamName = carInfo.TeamName
        };

        this.entryList.Add(eventEntry);
        this.eventEntriesSubject.OnNext(eventEntry);
        this.entryListSubject.OnNext(this.entryList);
    }

    private void OnNextRealTimeCarUpdate(RealtimeCarUpdate realTimeCarUpdate) { }

    private void OnNextRealTimeUpdate(RealtimeUpdate realtimeUpdate)
    {
        this.LogMessage(LoggingLevel.Information, realtimeUpdate.ToString());

        if(this.currentEvent == null)
        {
            return;
        }

        var sessionType = realtimeUpdate.SessionType.ToFriendlyName();
        var sessionPhase = realtimeUpdate.Phase.ToFriendlyName();

        if(this.currentSession?.SessionType != sessionType)
        {
            if(this.currentSession != null)
            {
                this.sessionEndedSubject.OnNext(this.currentSession);
            }
            this.currentSession = new AccSession(this.currentEvent.Id, sessionType);
            this.sessionStartedSubject.OnNext(this.currentSession);
        }

        if(this.currentSession == null || this.currentPhase?.Phase == sessionPhase)
        {
            return;
        }

        if(this.currentPhase != null)
        {
            this.phaseEndedSubject.OnNext(this.currentPhase);
        }

        this.currentPhase =
            new AccSessionPhase(this.currentEvent.Id, this.currentSession.Id, sessionPhase);
        this.phaseStartedSubject.OnNext(this.currentPhase);
    }

    private void OnNextTrackDataUpdate(TrackDataUpdate trackDataUpdate)
    {
        this.LogMessage(LoggingLevel.Information, trackDataUpdate.ToString());
        this.currentEvent = new AccEvent(trackDataUpdate.TrackId,
            trackDataUpdate.TrackName,
            trackDataUpdate.TrackMeters);
        this.eventStartedSubject.OnNext(this.currentEvent);
        this.entryList.Clear();
        this.entryListSubject.OnNext(this.entryList);
        this.accUdpConnection?.RequestEntryList();
    }

    private void PrepareMessageProcessing()
    {
        this.subscriptionSink = new CompositeDisposable
        {
            this.accUdpConnection!.LogMessages.Subscribe(m => this.logMessagesSubject.OnNext(m)),
            this.accUdpConnection!.ConnectionStateChanges.Subscribe(this.OnNextConnectionStateChange),
            this.accUdpConnection.EntryListUpdates.Subscribe(this.OnNextEntryListUpdate),
            this.accUdpConnection!.RealTimeUpdates.Subscribe(this.OnNextRealTimeUpdate),
            this.accUdpConnection!.RealTimeCarUpdates.Subscribe(this.OnNextRealTimeCarUpdate),
            this.accUdpConnection!.TrackDataUpdates.Subscribe(this.OnNextTrackDataUpdate)
        };
    }
}