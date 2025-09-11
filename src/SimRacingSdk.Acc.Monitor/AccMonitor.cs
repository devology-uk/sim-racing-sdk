using System.Diagnostics;
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
using SimRacingSdk.Acc.SharedMemory.Models;
using SimRacingSdk.Acc.Udp.Abstractions;
using SimRacingSdk.Acc.Udp.Enums;
using SimRacingSdk.Acc.Udp.Messages;

namespace SimRacingSdk.Acc.Monitor;

public class AccMonitor : IAccMonitor
{
    private const string LocalhostIpAddress = "127.0.0.1";

    private readonly IAccCarInfoProvider accCarInfoProvider;
    private readonly IAccCompatibilityChecker accCompatibilityChecker;
    private readonly Subject<AccMonitorAccident> accidentsSubject = new();
    private readonly IAccLocalConfigProvider accLocalConfigProvider;
    private readonly IAccNationalityInfoProvider accNationalityInfoProvider;
    private readonly IAccSharedMemoryConnectionFactory accSharedMemoryConnectionFactory;
    private readonly IAccUdpConnectionFactory accUdpConnectionFactory;
    private readonly List<AccMonitorEventEntry> entryList = [];
    private readonly ReplaySubject<IList<AccMonitorEventEntry>> entryListSubject = new();
    private readonly ReplaySubject<AccMonitorEvent> eventEndedSubject = new();
    private readonly ReplaySubject<AccMonitorEventEntry> eventEntriesSubject = new();
    private readonly ReplaySubject<AccMonitorEvent> eventStartedSubject = new();
    private readonly Subject<AccMonitorGreenFlag> greenFlagSubject = new();
    private readonly Subject<bool> isWhiteFlagActiveSubject = new();
    private readonly Subject<bool> isYellowFlagActiveSubject = new();
    private readonly Subject<AccMonitorLap> lapCompletedSubject = new();
    private readonly ReplaySubject<LogMessage> logMessagesSubject = new();
    private readonly Subject<AccMonitorPenalty> penaltiesSubject = new();
    private readonly Subject<AccMonitorLap> personalBestLapSubject = new();
    private readonly ReplaySubject<AccMonitorSessionPhase> phaseEndedSubject = new();
    private readonly ReplaySubject<AccMonitorSessionPhase> phaseStartedSubject = new();
    private readonly Subject<RealtimeCarUpdate> realtimeCarUpdatesSubject = new();
    private readonly Subject<AccMonitorLap> sessionBestLapSubject = new();
    private readonly ReplaySubject<AccMonitorSession> sessionEndedSubject = new();
    private readonly Subject<AccMonitorSession> sessionOverSubject = new();
    private readonly ReplaySubject<AccMonitorSession> sessionStartedSubject = new();
    private readonly Subject<AccTelemetryFrame> telemetrySubject = new();

    private IAccSharedMemoryConnection? accSharedMemoryConnection;
    private IAccUdpConnection? accUdpConnection;
    private AccMonitorEvent? currentEvent;
    private AccMonitorSessionPhase? currentPhase;
    private AccMonitorSession? currentSession;
    private bool isWhiteFlagActive;
    private bool isYellowFlagActive;
    private CompositeDisposable? sharedMemorySubscriptionSink;
    private CompositeDisposable? udpSubscriptionSink;

    public AccMonitor(IAccUdpConnectionFactory accUdpConnectionFactory,
        IAccSharedMemoryConnectionFactory accSharedMemoryConnectionFactory,
        IAccCompatibilityChecker accCompatibilityChecker,
        IAccLocalConfigProvider accLocalConfigProvider,
        IAccCarInfoProvider accCarInfoProvider,
        IAccNationalityInfoProvider accNationalityInfoProvider)
    {
        this.accUdpConnectionFactory = accUdpConnectionFactory;
        this.accSharedMemoryConnectionFactory = accSharedMemoryConnectionFactory;
        this.accCompatibilityChecker = accCompatibilityChecker;
        this.accLocalConfigProvider = accLocalConfigProvider;
        this.accCarInfoProvider = accCarInfoProvider;
        this.accNationalityInfoProvider = accNationalityInfoProvider;
    }

    public IObservable<AccMonitorAccident> Accidents => this.accidentsSubject.AsObservable();
    public IObservable<IList<AccMonitorEventEntry>> EntryList => this.entryListSubject.AsObservable();
    public IObservable<AccMonitorEvent> EventEnded => this.eventEndedSubject.AsObservable();
    public IObservable<AccMonitorEventEntry> EventEntries => this.eventEntriesSubject.AsObservable();
    public IObservable<AccMonitorEvent> EventStarted => this.eventStartedSubject.AsObservable();
    public IObservable<AccMonitorGreenFlag> GreenFlag => this.greenFlagSubject.AsObservable();
    public IObservable<bool> IsWhiteFlagActive => this.isWhiteFlagActiveSubject.AsObservable();
    public IObservable<bool> IsYellowFlagActive => this.isYellowFlagActiveSubject.AsObservable();
    public IObservable<AccMonitorLap> LapCompleted => this.lapCompletedSubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();
    public IObservable<AccMonitorPenalty> Penalties => this.penaltiesSubject.AsObservable();
    public IObservable<AccMonitorLap> PersonalBestLap => this.personalBestLapSubject.AsObservable();
    public IObservable<AccMonitorSessionPhase> PhaseEnded => this.phaseEndedSubject.AsObservable();
    public IObservable<AccMonitorSessionPhase> PhaseStarted => this.phaseStartedSubject.AsObservable();
    public IObservable<RealtimeCarUpdate> RealtimeCarUpdates => this.realtimeCarUpdatesSubject.AsObservable();
    public IObservable<AccMonitorLap> SessionBestLap => this.sessionBestLapSubject.AsObservable();
    public IObservable<AccMonitorSession> SessionEnded => this.sessionEndedSubject.AsObservable();
    public IObservable<AccMonitorSession> SessionOver => this.sessionOverSubject.AsObservable();
    public IObservable<AccMonitorSession> SessionStarted => this.sessionStartedSubject.AsObservable();
    public IObservable<AccTelemetryFrame> Telemetry => this.telemetrySubject.AsObservable();

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

        this.PrepareUdpMessageProcessing();

        this.accUdpConnection.Connect();
    }

    public void Stop()
    {
        this.udpSubscriptionSink?.Dispose();
        this.sharedMemorySubscriptionSink?.Dispose();
        this.accUdpConnection?.Dispose();
        this.accUdpConnection = null;
        this.accSharedMemoryConnection?.Dispose();
        this.accSharedMemoryConnection = null;
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

    private void OnNextBroadcastEvent(BroadcastingEvent broadcastingEvent)
    {
        this.LogMessage(LoggingLevel.Information, broadcastingEvent.ToString());
        switch(broadcastingEvent.BroadcastingEventType)
        {
            case BroadcastingEventType.GreenFlag:
                this.ProcessGreenFlagEvent(broadcastingEvent);
                break;
            case BroadcastingEventType.SessionOver:
                this.ProcessSessionOverEvent(broadcastingEvent);
                break;
            case BroadcastingEventType.PenaltyCommMsg:
                this.ProcessPenaltyComMsgEvent(broadcastingEvent);
                break;
            case BroadcastingEventType.Accident:
                this.ProcessAccidentEvent(broadcastingEvent);
                break;
            case BroadcastingEventType.LapCompleted:
                this.ProcessLapCompletedEvent(broadcastingEvent);
                break;
            case BroadcastingEventType.BestSessionLap:
                this.ProcessBestSessionLapEvent(broadcastingEvent);
                break;
            case BroadcastingEventType.BestPersonalLap:
                this.ProcessBestPersonalLap(broadcastingEvent);
                break;
            case BroadcastingEventType.None:
            default:
                Debug.WriteLine("Unknown broadcast event type received.");
                break;
        }
    }

    private void OnNextConnectionStateChange(ConnectionState connectionState)
    {
        this.LogMessage(LoggingLevel.Information, connectionState.ToString());
        if(connectionState.IsConnected || this.currentEvent == null)
        {
            return;
        }

        this.eventEndedSubject.OnNext(this.currentEvent);
        this.currentEvent = null;
        this.accSharedMemoryConnection?.Dispose();
    }

    private void OnNextEntryListUpdate(EntryListUpdate entryListUpdate)
    {
        this.LogMessage(LoggingLevel.Information, entryListUpdate.ToString());
        var carInfo = entryListUpdate.CarInfo;
        var car = this.accCarInfoProvider.FindByModelId(carInfo.CarModelType);
        var drivers = carInfo.Drivers.Select(d => new AccMonitorDriver(d.FirstName,
                                                 d.LastName,
                                                 d.ShortName,
                                                 d.Category.ToString(),
                                                 this.accNationalityInfoProvider
                                                     .GetCountryCode(d.Nationality)))
                             .ToList();
        var eventEntry = new AccMonitorEventEntry
        {
            AccCarModelId = carInfo.CarModelType,
            CarManufacturer = car!.ManufacturerTag,
            CarModelName = car.DisplayName,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentMonitorDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            Drivers = drivers,
            EventId = this.currentEvent!.Id,
            CarIndex = carInfo.CarIndex,
            RaceNumber = carInfo.RaceNumber,
            TeamName = carInfo.TeamName
        };

        this.entryList.Add(eventEntry);
        this.eventEntriesSubject.OnNext(eventEntry);
        this.entryListSubject.OnNext(this.entryList);
    }

    private void OnNextFlagState(AccFlagState accFlagState)
    {
        this.LogMessage(LoggingLevel.Information, accFlagState.ToString());
        this.ProcessYellowFlagState(accFlagState);
        this.ProcessWhiteFlagState(accFlagState);
    }

    private void OnNextRealTimeCarUpdate(RealtimeCarUpdate realTimeCarUpdate)
    {
        this.LogMessage(LoggingLevel.Information, realTimeCarUpdate.ToString());
        this.realtimeCarUpdatesSubject.OnNext(realTimeCarUpdate);
    }

    private void OnNextRealTimeUpdate(RealtimeUpdate realtimeUpdate)
    {
        this.LogMessage(LoggingLevel.Information, realtimeUpdate.ToString());

        if(this.currentEvent == null)
        {
            return;
        }

        var sessionType = realtimeUpdate.SessionType.ToFriendlyName();
        var sessionPhase = realtimeUpdate.Phase.ToFriendlyName();

        if(this.currentPhase != null && this.currentPhase.Phase != sessionPhase)
        {
            this.phaseEndedSubject.OnNext(this.currentPhase!);
            this.currentPhase = null;
        }

        if(this.currentSession?.SessionType != sessionType)
        {
            if(this.currentSession != null)
            {
                this.sessionEndedSubject.OnNext(this.currentSession);
            }

            this.currentSession = new AccMonitorSession(this.currentEvent.Id, sessionType);
            this.sessionStartedSubject.OnNext(this.currentSession);
        }

        if(this.currentSession == null || this.currentPhase?.Phase == sessionPhase)
        {
            return;
        }

        this.currentPhase =
            new AccMonitorSessionPhase(this.currentEvent.Id, this.currentSession.Id, sessionPhase);
        this.phaseStartedSubject.OnNext(this.currentPhase);
    }

    private void OnNextTelemetryFrame(AccTelemetryFrame telemetryFrame)
    {
        this.LogMessage(LoggingLevel.Information, telemetryFrame.ToString());

        this.telemetrySubject.OnNext(telemetryFrame);
    }

    private void OnNextTrackDataUpdate(TrackDataUpdate trackDataUpdate)
    {
        this.LogMessage(LoggingLevel.Information, trackDataUpdate.ToString());
        this.currentEvent = new AccMonitorEvent(trackDataUpdate.TrackId,
            trackDataUpdate.TrackName,
            trackDataUpdate.TrackMeters);
        this.eventStartedSubject.OnNext(this.currentEvent);
        this.entryList.Clear();
        this.entryListSubject.OnNext(this.entryList);

        this.PrepareSharedMemoryConnection();
    }

    private void PrepareSharedMemoryConnection()
    {
        this.LogMessage(LoggingLevel.Information,
            "Preparing connection to ACC Shared Memory interface for telemetry.");
        this.accSharedMemoryConnection = this.accSharedMemoryConnectionFactory.Create();

        this.sharedMemorySubscriptionSink = new CompositeDisposable
        {
            this.accSharedMemoryConnection.FlagState.Subscribe(this.OnNextFlagState),
            this.accSharedMemoryConnection.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };

        this.accSharedMemoryConnection.Start();
    }

    private void PrepareUdpMessageProcessing()
    {
        this.udpSubscriptionSink = new CompositeDisposable
        {
            this.accUdpConnection!.LogMessages.Subscribe(m => this.logMessagesSubject.OnNext(m)),
            this.accUdpConnection.BroadcastingEvents.Subscribe(this.OnNextBroadcastEvent),
            this.accUdpConnection.ConnectionStateChanges.Subscribe(this.OnNextConnectionStateChange),
            this.accUdpConnection.EntryListUpdates.Subscribe(this.OnNextEntryListUpdate),
            this.accUdpConnection.RealTimeUpdates.Subscribe(this.OnNextRealTimeUpdate),
            this.accUdpConnection.RealTimeCarUpdates.Subscribe(this.OnNextRealTimeCarUpdate),
            this.accUdpConnection.TrackDataUpdates.Subscribe(this.OnNextTrackDataUpdate)
        };
    }

    private void ProcessAccidentEvent(BroadcastingEvent broadcastingEvent)
    {
        var carInfo = broadcastingEvent.CarData;
        var car = this.accCarInfoProvider.FindByModelId(carInfo.CarModelType);
        var drivers = carInfo.Drivers.Select(d => new AccMonitorDriver(d.FirstName,
                                                 d.LastName,
                                                 d.ShortName,
                                                 d.Category.ToString(),
                                                 this.accNationalityInfoProvider
                                                     .GetCountryCode(d.Nationality)))
                             .ToList();

        var accAccident = new AccMonitorAccident()
        {
            AccCarModelId = carInfo.CarModelType,
            CarManufacturer = car!.ManufacturerTag,
            CarModelName = car.DisplayName,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentMonitorDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            EventId = this.currentEvent!.Id,
            CarIndex = carInfo.CarIndex,
            RaceNumber = carInfo.RaceNumber,
            TeamName = carInfo.TeamName
        };

        this.accidentsSubject.OnNext(accAccident);
    }

    private void ProcessBestPersonalLap(BroadcastingEvent broadcastingEvent)
    {
        var carInfo = broadcastingEvent.CarData;
        var car = this.accCarInfoProvider.FindByModelId(carInfo.CarModelType);
        var drivers = carInfo.Drivers.Select(d => new AccMonitorDriver(d.FirstName,
                                                 d.LastName,
                                                 d.ShortName,
                                                 d.Category.ToString(),
                                                 this.accNationalityInfoProvider
                                                     .GetCountryCode(d.Nationality)))
                             .ToList();

        var accLap = new AccMonitorLap()
        {
            AccCarModelId = carInfo.CarModelType,
            CarManufacturer = car!.ManufacturerTag,
            CarModelName = car.DisplayName,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            EventId = this.currentEvent!.Id,
            CarIndex = carInfo.CarIndex,
            LapTime = broadcastingEvent.Message,
            RaceNumber = carInfo.RaceNumber,
            TeamName = carInfo.TeamName
        };

        this.personalBestLapSubject.OnNext(accLap);
    }

    private void ProcessBestSessionLapEvent(BroadcastingEvent broadcastingEvent)
    {
        var carInfo = broadcastingEvent.CarData;
        var car = this.accCarInfoProvider.FindByModelId(carInfo.CarModelType);
        var drivers = carInfo.Drivers.Select(d => new AccMonitorDriver(d.FirstName,
                                                 d.LastName,
                                                 d.ShortName,
                                                 d.Category.ToString(),
                                                 this.accNationalityInfoProvider
                                                     .GetCountryCode(d.Nationality)))
                             .ToList();

        var accLap = new AccMonitorLap()
        {
            AccCarModelId = carInfo.CarModelType,
            CarManufacturer = car!.ManufacturerTag,
            CarModelName = car.DisplayName,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            EventId = this.currentEvent!.Id,
            CarIndex = carInfo.CarIndex,
            LapTime = broadcastingEvent.Message,
            RaceNumber = carInfo.RaceNumber,
            TeamName = carInfo.TeamName
        };

        this.sessionBestLapSubject.OnNext(accLap);
    }

    private void ProcessGreenFlagEvent(BroadcastingEvent broadcastingEvent)
    {
        this.greenFlagSubject.OnNext(new AccMonitorGreenFlag(this.currentSession?.Id));
    }

    private void ProcessLapCompletedEvent(BroadcastingEvent broadcastingEvent)
    {
        var carInfo = broadcastingEvent.CarData;
        var car = this.accCarInfoProvider.FindByModelId(carInfo.CarModelType);
        var drivers = carInfo.Drivers.Select(d => new AccMonitorDriver(d.FirstName,
                                                 d.LastName,
                                                 d.ShortName,
                                                 d.Category.ToString(),
                                                 this.accNationalityInfoProvider
                                                     .GetCountryCode(d.Nationality)))
                             .ToList();

        var accLap = new AccMonitorLap()
        {
            AccCarModelId = carInfo.CarModelType,
            CarManufacturer = car!.ManufacturerTag,
            CarModelName = car.DisplayName,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            EventId = this.currentEvent!.Id,
            CarIndex = carInfo.CarIndex,
            LapTime = broadcastingEvent.Message,
            RaceNumber = carInfo.RaceNumber,
            TeamName = carInfo.TeamName
        };

        this.lapCompletedSubject.OnNext(accLap);
    }

    private void ProcessPenaltyComMsgEvent(BroadcastingEvent broadcastingEvent)
    {
        var carInfo = broadcastingEvent.CarData;
        var car = this.accCarInfoProvider.FindByModelId(carInfo.CarModelType);
        var drivers = carInfo.Drivers.Select(d => new AccMonitorDriver(d.FirstName,
                                                 d.LastName,
                                                 d.ShortName,
                                                 d.Category.ToString(),
                                                 this.accNationalityInfoProvider
                                                     .GetCountryCode(d.Nationality)))
                             .ToList();

        var accPenalty = new AccMonitorPenalty()
        {
            AccCarModelId = carInfo.CarModelType,
            CarManufacturer = car!.ManufacturerTag,
            CarModelName = car.DisplayName,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentMonitorDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            EventId = this.currentEvent!.Id,
            Index = carInfo.CarIndex,
            Penalty = broadcastingEvent.Message,
            RaceNumber = carInfo.RaceNumber,
            TeamName = carInfo.TeamName
        };

        this.penaltiesSubject.OnNext(accPenalty);
    }

    private void ProcessSessionOverEvent(BroadcastingEvent broadcastingEvent)
    {
        this.sessionOverSubject.OnNext(this.currentSession!);
    }

    private void ProcessWhiteFlagState(AccFlagState accFlagState)
    {
        if(this.isWhiteFlagActive == accFlagState.IsWhiteFlagActive)
        {
            return;
        }

        this.isWhiteFlagActive = accFlagState.IsWhiteFlagActive;
        this.isWhiteFlagActiveSubject.OnNext(this.isWhiteFlagActive);
    }

    private void ProcessYellowFlagState(AccFlagState accFlagState)
    {
        if(this.isYellowFlagActive == accFlagState.IsYellowFlagActive)
        {
            return;
        }

        this.isYellowFlagActive = accFlagState.IsYellowFlagActive;
        this.isYellowFlagActiveSubject.OnNext(this.isYellowFlagActive);
    }
}