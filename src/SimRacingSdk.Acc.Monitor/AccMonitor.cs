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
using SimRacingSdk.Acc.SharedMemory.Enums;
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
    private readonly Subject<AccMonitorEntry> entriesSubject = new();
    private readonly List<AccMonitorEntry> entryList = [];
    private readonly ReplaySubject<IList<AccMonitorEntry>> entryListSubject = new();
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
    private readonly Subject<AccMonitorSession> sessionCompletedSubject = new();
    private readonly Subject<AccMonitorSession> sessionStartedSubject = new();
    private readonly Subject<AccMonitorSessionTypeChange> sessionTypeChangedSubject = new();
    private readonly Subject<AccTelemetryFrame> telemetrySubject = new();

    private IAccSharedMemoryConnection? accSharedMemoryConnection;
    private AccSharedMemoryEvent? accSharedMemoryEvent;
    private AccSharedMemorySession? accSharedMemorySession;
    private IAccUdpConnection? accUdpConnection;
    private string? connectionIdentifier;
    private AccAppStatus currentAppStatus;
    private AccMonitorSession? currentSession;
    private TimeSpan currentSessionTime = TimeSpan.Zero;
    private SessionPhase currentUdpPhase = SessionPhase.NONE;
    private RaceSessionType currentUdpSessionType = RaceSessionType.NONE;
    private bool isWhiteFlagActive;
    private bool isYellowFlagActive;
    private CompositeDisposable? sharedMemorySubscriptionSink;
    private TrackDataUpdate? trackData;
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
    public IObservable<AccMonitorEntry> Entries => this.entriesSubject.AsObservable();
    public IObservable<IList<AccMonitorEntry>> EntryList => this.entryListSubject.AsObservable();
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
    public IObservable<AccMonitorSession> SessionCompleted => this.sessionCompletedSubject.AsObservable();
    public IObservable<AccMonitorSession> SessionStarted => this.sessionStartedSubject.AsObservable();
    public IObservable<AccMonitorSessionTypeChange> SessionTypeChanged =>
        this.sessionTypeChangedSubject.AsObservable();
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

        this.StartSharedMemoryConnection();
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

    private void AddEntryIfNotExists(CarInfo carInfo)
    {
        var entry = this.FindEntryByCarIndex(carInfo.CarIndex);
        if(entry != null)
        {
            return;
        }

        var car = this.accCarInfoProvider.FindByModelId(carInfo.CarModelType);
        var drivers = carInfo.Drivers.Select(d => new AccMonitorDriver(d.FirstName,
                                                 d.LastName,
                                                 d.ShortName,
                                                 d.Category.ToString(),
                                                 this.accNationalityInfoProvider
                                                     .GetCountryCode(d.Nationality)))
                             .ToList();
        var eventEntry = new AccMonitorEntry
        {
            AccCarModelId = carInfo.CarModelType,
            CarManufacturer = car!.ManufacturerTag,
            CarModelName = car.DisplayName,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CarLocation = CarLocation.Pitlane,
            CurrentMonitorDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            Drivers = drivers,
            ConnectionId = this.connectionIdentifier,
            CarIndex = carInfo.CarIndex,
            RaceNumber = carInfo.RaceNumber,
            TeamName = carInfo.TeamName
        };

        this.entryList.Add(eventEntry);
        this.entriesSubject.OnNext(eventEntry);
        this.entryListSubject.OnNext(this.entryList);
    }

    private void CompleteCurrentSession()
    {
        if(this.currentSession == null)
        {
            return;
        }

        this.currentSession.IsRunning = false;
        this.LogMessage(LoggingLevel.Information, this.currentSession.ToString());
        this.sessionCompletedSubject.OnNext(this.currentSession);
        this.currentSession = null;
    }

    private AccMonitorEntry? FindEntryByCarIndex(int carIndex)
    {
        return this.entryList.FirstOrDefault(e => e.CarIndex == carIndex);
        ;
    }

    private void LogMessage(LoggingLevel level, string content)
    {
        this.logMessagesSubject.OnNext(new LogMessage(level, content, nameof(AccMonitor)));
    }

    private void OnNextAppStatusChange(AccAppStatusChange appStatusChange)
    {
        this.LogMessage(LoggingLevel.Information, appStatusChange.ToString());
        this.currentAppStatus = appStatusChange.To;
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

    private void OnNextEntryListUpdate(EntryListUpdate entryListUpdate)
    {
        this.LogMessage(LoggingLevel.Information, entryListUpdate.ToString());
        var carInfo = entryListUpdate.CarInfo;
        this.AddEntryIfNotExists(carInfo);
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
        var eventEntry = this.FindEntryByCarIndex(realTimeCarUpdate.CarIndex);
        if(eventEntry != null)
        {
            eventEntry.CarLocation = realTimeCarUpdate.CarLocation;
        }
    }

    private void OnNextRealTimeUpdate(RealtimeUpdate realtimeUpdate)
    {
        this.LogMessage(LoggingLevel.Information, realtimeUpdate.ToString());

        if(!this.entryList.Any())
        {
            this.LogMessage(LoggingLevel.Information, "No ACC entry list.");
            return;
        }

        var sessionPhase = realtimeUpdate.Phase;
        var sessionType = realtimeUpdate.SessionType;
        var hasSessionTypeChanged = this.currentUdpSessionType != sessionType;
        var hasPhaseChanged = this.currentUdpPhase != sessionPhase;

        if(hasSessionTypeChanged)
        {
            var accMonitorSessionTypeChange =
                new AccMonitorSessionTypeChange(this.currentUdpSessionType, sessionType);
            this.sessionTypeChangedSubject.OnNext(accMonitorSessionTypeChange);
            this.LogMessage(LoggingLevel.Information, accMonitorSessionTypeChange.ToString());
        }

        if(hasPhaseChanged)
        {
            var accMonitorSessionPhaseChange =
                new AccMonitorSessionPhaseChange(this.currentUdpPhase, sessionPhase);
            this.phaseChangedSubject.OnNext(accMonitorSessionPhaseChange);
            this.LogMessage(LoggingLevel.Information, accMonitorSessionPhaseChange.ToString());
        }

        if((hasPhaseChanged && sessionPhase == SessionPhase.Session)
           || (!hasPhaseChanged && this.currentUdpPhase == SessionPhase.Session
                                && realtimeUpdate.SessionTime.TotalMilliseconds
                                < this.currentSessionTime.TotalMilliseconds))
        {
            this.CompleteCurrentSession();
            this.StartNewUdpSession(realtimeUpdate, sessionType);
        }

        this.currentUdpSessionType = sessionType;
        this.currentUdpPhase = sessionPhase;
        this.currentSessionTime = realtimeUpdate.SessionTime;
    }

    private void OnNextSharedMemoryEventEnded(AccSharedMemoryEvent accSharedMemoryEvent)
    {
        this.LogMessage(LoggingLevel.Information, accSharedMemoryEvent.ToString());
        this.accSharedMemoryEvent = null;
        this.StopUdpConnection();
    }

    private void OnNextSharedMemoryEventStarted(AccSharedMemoryEvent accSharedMemoryEvent)
    {
        this.LogMessage(LoggingLevel.Information, accSharedMemoryEvent.ToString());
        this.accSharedMemoryEvent = accSharedMemoryEvent;
        this.StartUdpConnection();
    }

    private void OnNextSharedMemorySessionEnded(AccSharedMemorySession accSharedMemorySession)
    {
        this.LogMessage(LoggingLevel.Information, accSharedMemorySession.ToString());
        this.accSharedMemorySession = null;
    }

    private void OnNextSharedMemorySessionStarted(AccSharedMemorySession accSharedMemorySession)
    {
        this.LogMessage(LoggingLevel.Information, accSharedMemorySession.ToString());
        this.accSharedMemorySession = accSharedMemorySession;
    }

    private void OnNextTelemetryFrame(AccTelemetryFrame telemetryFrame)
    {
        this.LogMessage(LoggingLevel.Information, telemetryFrame.ToString());
        this.telemetrySubject.OnNext(telemetryFrame);
    }

    private void OnNextTrackDataUpdate(TrackDataUpdate trackDataUpdate)
    {
        this.trackData = trackDataUpdate;
        this.LogMessage(LoggingLevel.Information, trackDataUpdate.ToString());
    }

    private void PrepareSharedMemoryMessageProcessing()
    {
        this.sharedMemorySubscriptionSink = new CompositeDisposable
        {
            this.accSharedMemoryConnection!.AppStatusChanges.Subscribe(this.OnNextAppStatusChange),
            this.accSharedMemoryConnection.FlagState.Subscribe(this.OnNextFlagState),
            this.accSharedMemoryConnection.EventEnded.Subscribe(this.OnNextSharedMemoryEventEnded),
            this.accSharedMemoryConnection.EventStarted.Subscribe(this.OnNextSharedMemoryEventStarted),
            this.accSharedMemoryConnection.SessionEnded.Subscribe(this.OnNextSharedMemorySessionEnded),
            this.accSharedMemoryConnection.SessionStarted.Subscribe(this.OnNextSharedMemorySessionStarted),
            this.accSharedMemoryConnection.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };
    }

    private void PrepareUdpMessageProcessing()
    {
        this.udpSubscriptionSink = new CompositeDisposable
        {
            this.accUdpConnection!.LogMessages.Subscribe(m => this.logMessagesSubject.OnNext(m)),
            this.accUdpConnection.BroadcastingEvents.Subscribe(this.OnNextBroadcastEvent),
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

        this.AddEntryIfNotExists(carInfo);

        var accAccident = new AccMonitorAccident()
        {
            AccCarModelId = carInfo.CarModelType,
            CarManufacturer = car!.ManufacturerTag,
            CarModelName = car.DisplayName,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentMonitorDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            CarIndex = carInfo.CarIndex,
            RaceNumber = carInfo.RaceNumber,
            SessionId = this.currentSession!.SessionId.ToString(),
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

        this.AddEntryIfNotExists(carInfo);

        var accLap = new AccMonitorLap()
        {
            AccCarModelId = carInfo.CarModelType,
            CarManufacturer = car!.ManufacturerTag,
            CarModelName = car.DisplayName,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            CarIndex = carInfo.CarIndex,
            LapTime = broadcastingEvent.Message,
            RaceNumber = carInfo.RaceNumber,
            SessionId = this.currentSession!.SessionId.ToString(),
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

        this.AddEntryIfNotExists(carInfo);

        var accLap = new AccMonitorLap()
        {
            AccCarModelId = carInfo.CarModelType,
            CarManufacturer = car!.ManufacturerTag,
            CarModelName = car.DisplayName,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            CarIndex = carInfo.CarIndex,
            LapTime = broadcastingEvent.Message,
            RaceNumber = carInfo.RaceNumber,
            SessionId = this.currentSession!.SessionId.ToString(),
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

        this.AddEntryIfNotExists(carInfo);

        var accLap = new AccMonitorLap()
        {
            AccCarModelId = carInfo.CarModelType,
            CarManufacturer = car!.ManufacturerTag,
            CarModelName = car.DisplayName,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            CarIndex = carInfo.CarIndex,
            LapTime = broadcastingEvent.Message,
            RaceNumber = carInfo.RaceNumber,
            SessionId = this.currentSession!.SessionId.ToString(),
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


        this.AddEntryIfNotExists(carInfo);

        var accPenalty = new AccMonitorPenalty()
        {
            AccCarModelId = carInfo.CarModelType,
            CarManufacturer = car!.ManufacturerTag,
            CarModelName = car.DisplayName,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentMonitorDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            Index = carInfo.CarIndex,
            Penalty = broadcastingEvent.Message,
            RaceNumber = carInfo.RaceNumber,
            SessionId = this.currentSession!.SessionId.ToString(),
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

    // private void StartNewSession()
    // {
    //     this.accUdpConnection!.RequestEntryList();
    //     this.currentSession = new AccMonitorSession
    //     {
    //         Duration = TimeSpan.FromMilliseconds(this.accSharedMemorySession!.DurationMs),
    //         EventId = this.accSharedMemorySession.EventId,
    //         IsOnline = this.accSharedMemorySession.IsOnline,
    //         IsRunning = true,
    //         NumberOfCars = this.accSharedMemorySession.NumberOfCars,
    //         SessionId = this.accSharedMemorySession.SessionId,
    //         SessionType = this.accSharedMemorySession.SessionType,
    //         TrackName = this.accSharedMemorySession.TrackName
    //     };
    //
    //     this.sessionStartedSubject.OnNext(this.currentSession);
    //     this.LogMessage(LoggingLevel.Information, this.currentSession.ToString());
    // }

    private void StartNewUdpSession(RealtimeUpdate realtimeUpdate, RaceSessionType sessionType)
    {
        this.currentSession = new AccMonitorSession
        {
            Duration = TimeSpan.FromMilliseconds(this.accSharedMemorySession!.DurationMs),
            EventId = this.accSharedMemoryEvent!.EventId,
            IsOnline = this.accSharedMemoryEvent.IsOnline,
            IsRunning = true,
            NumberOfCars = this.accSharedMemoryEvent.NumberOfCars,
            SessionId = this.accSharedMemorySession.SessionId,
            SessionType = sessionType.ToFriendlyName(),
            TrackName = this.accSharedMemorySession.TrackName
        };
        this.sessionStartedSubject.OnNext(this.currentSession);
        this.LogMessage(LoggingLevel.Information, $"Session Started: {this.currentSession}");
    }

    private void StartSharedMemoryConnection()
    {
        this.LogMessage(LoggingLevel.Information, "Preparing connection to ACC Shared Memory interface.");
        this.accSharedMemoryConnection = this.accSharedMemoryConnectionFactory.Create();

        this.PrepareSharedMemoryMessageProcessing();

        this.accSharedMemoryConnection.Start();
    }

    private void StartUdpConnection()
    {
        this.LogMessage(LoggingLevel.Information, "Preparing connection to ACC UDP interface.");
        var broadcastingSettings = this.accLocalConfigProvider.GetBroadcastingSettings()!;
        this.accUdpConnection = this.accUdpConnectionFactory.Create(LocalhostIpAddress,
            broadcastingSettings.UdpListenerPort,
            this.connectionIdentifier ?? $"{LocalhostIpAddress}:{broadcastingSettings.UdpListenerPort}",
            broadcastingSettings.ConnectionPassword,
            broadcastingSettings.CommandPassword);

        this.PrepareUdpMessageProcessing();

        this.accUdpConnection.Connect();
    }

    private void StopUdpConnection()
    {
        if(this.accUdpConnection == null)
        {
            return;
        }

        this.CompleteCurrentSession();
        this.udpSubscriptionSink?.Dispose();
        this.accUdpConnection?.Dispose();
        this.accUdpConnection = null;
        this.entryList.Clear();
        this.entryListSubject.OnNext(this.entryList);
    }
}