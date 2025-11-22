using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Acc.Core;
using SimRacingSdk.Acc.Core.Abstractions;
using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Monitor.Abstractions;
using SimRacingSdk.Acc.Monitor.Exceptions;
using SimRacingSdk.Acc.Monitor.Messages;
using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.SharedMemory.Models;
using SimRacingSdk.Acc.Udp.Abstractions;
using SimRacingSdk.Acc.Udp.Enums;
using SimRacingSdk.Acc.Udp.Messages;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;

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
    private readonly Subject<AccMonitorEvent> eventCompletedSubject = new();
    private readonly Subject<AccMonitorEventEntry> eventEntriesSubject = new();
    private readonly ReplaySubject<AccMonitorEvent> eventStartedSubject = new();
    private readonly Subject<AccMonitorGreenFlag> greenFlagSubject = new();
    private readonly Subject<bool> isWhiteFlagActiveSubject = new();
    private readonly Subject<bool> isYellowFlagActiveSubject = new();
    private readonly Subject<AccMonitorLap> lapCompletedSubject = new();
    private readonly Subject<LogMessage> logMessagesSubject = new();
    private readonly Subject<AccMonitorPenalty> penaltiesSubject = new();
    private readonly Subject<AccMonitorLap> personalBestLapSubject = new();
    private readonly Subject<AccMonitorSessionPhaseChange> phaseChangedSubject = new();
    private readonly Subject<RealtimeCarUpdate> realtimeCarUpdatesSubject = new();
    private readonly Subject<AccMonitorLap> sessionBestLapSubject = new();
    private readonly Subject<AccMonitorSessionTypeChange> sessionChangedSubject = new();
    private readonly Subject<AccMonitorSession> sessionCompletedSubject = new();
    private readonly Subject<AccMonitorSession> sessionStartedSubject = new();
    private readonly Subject<AccTelemetryFrame> telemetrySubject = new();

    private IAccSharedMemoryConnection? accSharedMemoryConnection;
    private AccSharedMemoryEvent? accSharedMemoryEvent;
    private IAccUdpConnection? accUdpConnection;
    private AccMonitorEvent? currentEvent;
    private SessionPhase currentPhase = SessionPhase.NONE;
    private AccMonitorSession? currentSession;
    private TimeSpan currentSessionTime = TimeSpan.Zero;
    private RaceSessionType currentSessionType = RaceSessionType.NONE;
    private bool isWhiteFlagActive;
    private bool isYellowFlagActive;
    private CompositeDisposable? sharedMemorySubscriptionSink;
    private CompositeDisposable? udpSubscriptionSink;
    private string? connectionIdentifier;

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
    public IObservable<AccMonitorEvent> EventCompleted => this.eventCompletedSubject.AsObservable();
    public IObservable<AccMonitorEventEntry> EventEntries => this.eventEntriesSubject.AsObservable();
    public IObservable<AccMonitorEvent> EventStarted => this.eventStartedSubject.AsObservable();
    public IObservable<AccMonitorGreenFlag> GreenFlag => this.greenFlagSubject.AsObservable();
    public IObservable<bool> IsWhiteFlagActive => this.isWhiteFlagActiveSubject.AsObservable();
    public IObservable<bool> IsYellowFlagActive => this.isYellowFlagActiveSubject.AsObservable();
    public IObservable<AccMonitorLap> LapCompleted => this.lapCompletedSubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();
    public IObservable<AccMonitorPenalty> Penalties => this.penaltiesSubject.AsObservable();
    public IObservable<AccMonitorLap> PersonalBestLap => this.personalBestLapSubject.AsObservable();
    public IObservable<AccMonitorSessionPhaseChange> PhaseChanged => this.phaseChangedSubject.AsObservable();
    public IObservable<RealtimeCarUpdate> RealtimeCarUpdates => this.realtimeCarUpdatesSubject.AsObservable();
    public IObservable<AccMonitorLap> SessionBestLap => this.sessionBestLapSubject.AsObservable();
    public IObservable<AccMonitorSessionTypeChange> SessionChanged =>
        this.sessionChangedSubject.AsObservable();
    public IObservable<AccMonitorSession> SessionCompleted => this.sessionCompletedSubject.AsObservable();
    public IObservable<AccMonitorSession> SessionStarted => this.sessionStartedSubject.AsObservable();
    public IObservable<AccTelemetryFrame> Telemetry => this.telemetrySubject.AsObservable();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void RequestEntryList()
    {
        this.accUdpConnection?.RequestEntryList();
    }

    public void Start(string? connectionIdentifier = null)
    {
        this.connectionIdentifier = connectionIdentifier;
        this.LogMessage(LoggingLevel.Information,
            $"Starting ACC Monitor connection with ID: {connectionIdentifier}");

        if(!this.accCompatibilityChecker.HasValidBroadcastingSettings())
        {
            this.LogMessage(LoggingLevel.Error,
                "ACC Monitor cannot start because ACC has not been configured for broadcasting.");
            throw new InvalidBroadcastingSettingsException();
        }

        this.PrepareAndStartNewAccConnection(connectionIdentifier);
    }

    public void Stop()
    {
        this.EndCurrentSession();
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

    private void EndCurrentSession()
    {
        if(this.currentSession == null)
        {
            return;
        }

        if(this.accSharedMemoryEvent != null)
        {
            this.currentSession.IsOnline = this.accSharedMemoryEvent.IsOnline;
            this.currentSession.NumberOfCars = this.accSharedMemoryEvent.NumberOfCars;
        }

        this.LogMessage(LoggingLevel.Information, $"Session Completed: {this.currentSession}");
        this.sessionCompletedSubject.OnNext(this.currentSession);
        this.currentSession = null;
    }

    private void LogMessage(LoggingLevel level, string content)
    {
        this.logMessagesSubject.OnNext(new LogMessage(level, content, nameof(AccMonitor)));
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

        if(connectionState is not { IsConnected: false, WasConnected: true })
        {
            return;
        }

        this.Stop();
        this.PrepareAndStartNewAccConnection(this.connectionIdentifier);
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
            CarLocation = CarLocation.Pitlane,
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

    private void OnNextNewEvent(AccSharedMemoryEvent accSharedMemoryEvent)
    {
        this.LogMessage(LoggingLevel.Information, accSharedMemoryEvent.ToString());
        this.accSharedMemoryEvent = accSharedMemoryEvent;
        if(this.currentSession == null)
        {
            return;
        }

        this.currentSession.IsOnline = accSharedMemoryEvent.IsOnline;
        this.currentSession.NumberOfCars = accSharedMemoryEvent.NumberOfCars;
    }

    private void OnNextNewSharedMemorySession(AccSharedMemorySession accSharedMemorySession)
    {
        this.LogMessage(LoggingLevel.Information, accSharedMemorySession.ToString());
    }

    private void OnNextRealTimeCarUpdate(RealtimeCarUpdate realTimeCarUpdate)
    {
        this.LogMessage(LoggingLevel.Information, realTimeCarUpdate.ToString());
        this.realtimeCarUpdatesSubject.OnNext(realTimeCarUpdate);
        var eventEntry = this.entryList.FirstOrDefault(e => e.CarIndex == realTimeCarUpdate.CarIndex);
        if(eventEntry != null)
        {
            eventEntry.CarLocation = realTimeCarUpdate.CarLocation;
        }
    }

    private void OnNextRealTimeUpdate(RealtimeUpdate realtimeUpdate)
    {
        this.LogMessage(LoggingLevel.Information, realtimeUpdate.ToString());

        if(this.currentEvent == null || !this.entryList.Any())
        {
            this.LogMessage(LoggingLevel.Information, "No event or entry list.");
            return;
        }

        var sessionPhase = realtimeUpdate.Phase;
        var sessionType = realtimeUpdate.SessionType;

        var startNewSession = this.currentSession == null 
                              || realtimeUpdate.SessionTime.TotalMilliseconds
                              < this.currentSessionTime.TotalMilliseconds ;

        this.currentSessionTime = realtimeUpdate.SessionTime;

        if(this.currentSessionType != sessionType)
        {
            this.LogMessage(LoggingLevel.Information,
                $"Session Type Changed: From={this.currentSessionType.ToFriendlyName()} To={sessionType.ToFriendlyName()}");
            this.sessionChangedSubject.OnNext(
                new AccMonitorSessionTypeChange(this.currentSessionType, sessionType));

            this.currentSessionType = sessionType;
            startNewSession = true;
        }

        if(this.currentPhase != sessionPhase)
        {
            this.LogMessage(LoggingLevel.Information,
                $"Phase Changed: From={this.currentPhase.ToFriendlyName()} To={sessionPhase.ToFriendlyName()}");
            this.phaseChangedSubject.OnNext(
                new AccMonitorSessionPhaseChange(this.currentPhase, sessionPhase));
            this.currentPhase = sessionPhase;
        }

        if(!startNewSession)
        {
            return;
        }

        this.EndCurrentSession();
        this.StartNewSession(realtimeUpdate, sessionType);
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

    private void PrepareAndStartNewAccConnection(string? connectionIdentifier)
    {
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

    private void PrepareSharedMemoryConnection()
    {
        this.LogMessage(LoggingLevel.Information,
            "Preparing connection to ACC Shared Memory interface for telemetry.");
        this.accSharedMemoryConnection = this.accSharedMemoryConnectionFactory.Create();

        this.sharedMemorySubscriptionSink = new CompositeDisposable
        {
            this.accSharedMemoryConnection.FlagState.Subscribe(this.OnNextFlagState),
            this.accSharedMemoryConnection.Telemetry.Subscribe(this.OnNextTelemetryFrame),
            this.accSharedMemoryConnection.NewEvent.Subscribe(this.OnNextNewEvent),
            this.accSharedMemoryConnection.NewSession.Subscribe(this.OnNextNewSharedMemorySession)
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
        this.sessionCompletedSubject.OnNext(this.currentSession!);
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

    private void StartNewSession(RealtimeUpdate realtimeUpdate, RaceSessionType sessionType)
    {
        this.accUdpConnection!.RequestEntryList();
        this.currentSession = new AccMonitorSession(this.currentEvent!.Id,
            sessionType.ToFriendlyName(),
            realtimeUpdate.SessionEndTime);
        this.sessionStartedSubject.OnNext(this.currentSession);
        this.LogMessage(LoggingLevel.Information, $"Session Started: {this.currentSession}");
    }
}