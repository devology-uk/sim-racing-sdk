using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Ace.Core;
using SimRacingSdk.Ace.Core.Abstractions;
using SimRacingSdk.Ace.Core.Enums;
using SimRacingSdk.Ace.Monitor.Abstractions;
using SimRacingSdk.Ace.Monitor.Exceptions;
using SimRacingSdk.Ace.Monitor.Messages;
using SimRacingSdk.Ace.SharedMemory.Abstractions;
using SimRacingSdk.Ace.SharedMemory.Enums;
using SimRacingSdk.Ace.SharedMemory.Models;
using SimRacingSdk.Ace.Udp.Abstractions;
using SimRacingSdk.Ace.Udp.Enums;
using SimRacingSdk.Ace.Udp.Messages;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ace.Monitor;

// Session/event lifecycle mirrors AccMonitor (shared memory drives event lifecycle, UDP drives
// session lifecycle within an event, with a disconnect watcher). Two deliberate departures from
// the Acc port: flag handling reads AceFlagState's Flag/GlobalFlag pair instead of Acc's
// per-sector yellow booleans (Evo doesn't expose those), and car lookups are null-safe rather
// than using Acc's `car!.` null-forgiving pattern, because AceCarInfoProvider.FindByModelId
// relies on an unverified index-based mapping (see AceCarInfoProvider.cs) that may not resolve
// every car until confirmed against real broadcasting traffic.
public class AceMonitor : IAceMonitor
{
    private const string LocalhostIpAddress = "127.0.0.1";
    private const string UnknownCarText = "Unknown";
    private readonly TimeSpan udpTimeoutThreshold = TimeSpan.FromSeconds(5);

    private readonly IAceCarInfoProvider aceCarInfoProvider;
    private readonly IAceCompatibilityChecker aceCompatibilityChecker;
    private readonly Subject<AceMonitorAccident> accidentsSubject = new();
    private readonly IAceLocalConfigProvider aceLocalConfigProvider;
    private readonly IAceNationalityInfoProvider aceNationalityInfoProvider;
    private readonly IAceSharedMemoryConnectionFactory aceSharedMemoryConnectionFactory;
    private readonly IAceUdpConnectionFactory aceUdpConnectionFactory;
    private readonly Subject<AceMonitorEntry> entriesSubject = new();
    private readonly List<AceMonitorEntry> entryList = [];
    private readonly ReplaySubject<IList<AceMonitorEntry>> entryListSubject = new();
    private readonly Subject<AceMonitorEvent> eventEndedSubject = new();
    private readonly Subject<AceMonitorEvent> eventStartedSubject = new();
    private readonly Subject<AceMonitorGreenFlag> greenFlagSubject = new();
    private readonly Subject<bool> isWhiteFlagActiveSubject = new();
    private readonly Subject<bool> isYellowFlagActiveSubject = new();
    private readonly Subject<AceMonitorLap> lapCompletedSubject = new();
    private readonly Subject<LogMessage> logMessagesSubject = new();
    private readonly Subject<AceMonitorPenalty> penaltiesSubject = new();
    private readonly Subject<AceMonitorLap> personalBestLapSubject = new();
    private readonly Subject<AceMonitorSessionPhaseChange> phaseChangedSubject = new();
    private readonly Subject<RealtimeCarUpdate> realtimeCarUpdatesSubject = new();
    private readonly Subject<AceMonitorLap> sessionBestLapSubject = new();
    private readonly Subject<AceMonitorSession> sessionCompletedSubject = new();
    private readonly Subject<AceMonitorSession> sessionStartedSubject = new();
    private readonly Subject<AceMonitorSessionTypeChange> sessionTypeChangedSubject = new();
    private readonly Subject<AceTelemetryFrame> telemetrySubject = new();

    private IAceSharedMemoryConnection? aceSharedMemoryConnection;
    private AceSharedMemorySession? aceSharedMemorySession;
    private IAceUdpConnection? aceUdpConnection;
    private string? connectionIdentifier;
    private AceStatus currentAppStatus;
    private AceMonitorEvent? currentEvent;
    private AceMonitorSession? currentSession;
    private TimeSpan currentSessionTime = TimeSpan.Zero;
    private SessionPhase currentUdpPhase = SessionPhase.NONE;
    private RaceSessionType currentUdpSessionType = RaceSessionType.NONE;
    private IDisposable? disconnectWatcherSubscription;
    private bool isWhiteFlagActive;
    private bool isYellowFlagActive;
    private DateTime lastRealtimeUpdate;
    private CompositeDisposable? sharedMemorySubscriptionSink;
    private TrackDataUpdate? trackData;
    private CompositeDisposable? udpSubscriptionSink;

    public AceMonitor(IAceUdpConnectionFactory aceUdpConnectionFactory,
        IAceSharedMemoryConnectionFactory aceSharedMemoryConnectionFactory,
        IAceCompatibilityChecker aceCompatibilityChecker,
        IAceLocalConfigProvider aceLocalConfigProvider,
        IAceCarInfoProvider aceCarInfoProvider,
        IAceNationalityInfoProvider aceNationalityInfoProvider)
    {
        this.aceUdpConnectionFactory = aceUdpConnectionFactory;
        this.aceSharedMemoryConnectionFactory = aceSharedMemoryConnectionFactory;
        this.aceCompatibilityChecker = aceCompatibilityChecker;
        this.aceLocalConfigProvider = aceLocalConfigProvider;
        this.aceCarInfoProvider = aceCarInfoProvider;
        this.aceNationalityInfoProvider = aceNationalityInfoProvider;
    }

    public IObservable<AceMonitorAccident> Accidents => this.accidentsSubject.AsObservable();
    public IObservable<IList<AceMonitorEntry>> EntryList => this.entryListSubject.AsObservable();
    public IObservable<AceMonitorEntry> Entries => this.entriesSubject.AsObservable();
    public IObservable<AceMonitorEvent> EventEnded => this.eventEndedSubject.AsObservable();
    public IObservable<AceMonitorEvent> EventStarted => this.eventStartedSubject.AsObservable();
    public IObservable<AceMonitorGreenFlag> GreenFlag => this.greenFlagSubject.AsObservable();
    public IObservable<bool> IsWhiteFlagActive => this.isWhiteFlagActiveSubject.AsObservable();
    public IObservable<bool> IsYellowFlagActive => this.isYellowFlagActiveSubject.AsObservable();
    public IObservable<AceMonitorLap> LapCompleted => this.lapCompletedSubject.AsObservable();
    public IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();
    public IObservable<AceMonitorPenalty> Penalties => this.penaltiesSubject.AsObservable();
    public IObservable<AceMonitorLap> PersonalBestLap => this.personalBestLapSubject.AsObservable();
    public IObservable<AceMonitorSessionPhaseChange> PhaseChanged => this.phaseChangedSubject.AsObservable();
    public IObservable<RealtimeCarUpdate> RealtimeCarUpdates => this.realtimeCarUpdatesSubject.AsObservable();
    public IObservable<AceMonitorLap> SessionBestLap => this.sessionBestLapSubject.AsObservable();
    public IObservable<AceMonitorSession> SessionCompleted => this.sessionCompletedSubject.AsObservable();
    public IObservable<AceMonitorSession> SessionStarted => this.sessionStartedSubject.AsObservable();
    public IObservable<AceMonitorSessionTypeChange> SessionTypeChanged =>
        this.sessionTypeChangedSubject.AsObservable();
    public IObservable<AceTelemetryFrame> Telemetry => this.telemetrySubject.AsObservable();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void RequestEntryList()
    {
        this.aceUdpConnection?.RequestEntryList();
    }

    public void Start(string? connectionIdentifier = null)
    {
        this.connectionIdentifier = connectionIdentifier;
        this.LogMessage(LoggingLevel.Information,
            $"Starting Ace Monitor connection with ID: {connectionIdentifier}");

        if(!this.aceCompatibilityChecker.HasValidBroadcastingSettings())
        {
            this.LogMessage(LoggingLevel.Error,
                "Ace Monitor cannot start because Ace Evo has not been configured for broadcasting.");
            throw new InvalidBroadcastingSettingsException();
        }

        this.StartSharedMemoryConnection();
    }

    public void Stop()
    {
        this.disconnectWatcherSubscription?.Dispose();
        this.disconnectWatcherSubscription = null;
        this.EndCurrentEvent();
        this.sharedMemorySubscriptionSink?.Dispose();
        this.aceSharedMemoryConnection?.Dispose();
        this.aceSharedMemoryConnection = null;
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
        var entry = this.entryList.FirstOrDefault(e => e.CarIndex == carInfo.CarIndex);
        if(entry != null)
        {
            return;
        }

        var car = this.aceCarInfoProvider.FindByModelId(carInfo.CarModelType);
        var drivers = carInfo.Drivers.Select(d => new AceMonitorDriver(d.FirstName,
                                                 d.LastName,
                                                 d.ShortName,
                                                 d.Category.ToString(),
                                                 this.aceNationalityInfoProvider
                                                     .GetCountryCode(d.Nationality)))
                             .ToList();
        var eventEntry = new AceMonitorEntry
        {
            AceCarModelId = carInfo.CarModelType,
            // Evo's cars.json has no separate manufacturer field - it's embedded in DisplayName
            // (e.g. "BMW M2 Coupe - Standard") but not cleanly parseable out.
            CarManufacturer = UnknownCarText,
            CarModelName = car?.DisplayName ?? UnknownCarText,
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

    private void CompleteCurrentUdpSession()
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

    private void EndCurrentEvent()
    {
        if(this.currentEvent == null)
        {
            return;
        }

        this.StopUdpConnection();
        this.currentEvent.IsRunning = false;
        this.LogMessage(LoggingLevel.Information, $"Event Ended: {this.currentEvent}");
        this.eventEndedSubject.OnNext(this.currentEvent);
        this.currentEvent = null;
    }

    private void LogMessage(LoggingLevel level, string content)
    {
        this.logMessagesSubject.OnNext(new LogMessage(level, content, nameof(AceMonitor)));
    }

    private void OnNextAppStatusChange(AceAppStatusChange appStatusChange)
    {
        this.LogMessage(LoggingLevel.Information, appStatusChange.ToString());
        this.currentAppStatus = appStatusChange.To;

        if(appStatusChange.To == AceStatus.Off)
        {
            this.EndCurrentEvent();
        }
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
        this.AddEntryIfNotExists(entryListUpdate.CarInfo);
    }

    private void OnNextFlagState(AceFlagState aceFlagState)
    {
        this.LogMessage(LoggingLevel.Information, aceFlagState.ToString());
        this.ProcessYellowFlagState(aceFlagState);
        this.ProcessWhiteFlagState(aceFlagState);
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
        this.lastRealtimeUpdate = DateTime.Now;

        if(!this.entryList.Any())
        {
            this.LogMessage(LoggingLevel.Information, "No Ace Evo entry list.");
            return;
        }

        var sessionPhase = realtimeUpdate.Phase;
        var sessionType = realtimeUpdate.SessionType;
        var hasSessionTypeChanged = this.currentUdpSessionType != sessionType;
        var hasPhaseChanged = this.currentUdpPhase != sessionPhase;

        if(hasSessionTypeChanged)
        {
            var sessionTypeChange =
                new AceMonitorSessionTypeChange(this.currentUdpSessionType, sessionType);
            this.sessionTypeChangedSubject.OnNext(sessionTypeChange);
            this.LogMessage(LoggingLevel.Information, sessionTypeChange.ToString());
        }

        if(hasPhaseChanged)
        {
            var phaseChange = new AceMonitorSessionPhaseChange(this.currentUdpPhase, sessionPhase);
            this.phaseChangedSubject.OnNext(phaseChange);
            this.LogMessage(LoggingLevel.Information, phaseChange.ToString());
        }

        if(hasPhaseChanged && sessionPhase == SessionPhase.PostSession)
        {
            this.CompleteCurrentUdpSession();
        }

        if((hasPhaseChanged && sessionPhase == SessionPhase.Session)
           || (!hasPhaseChanged && this.currentUdpPhase == SessionPhase.Session
                                && realtimeUpdate.SessionTime.TotalMilliseconds
                                < this.currentSessionTime.TotalMilliseconds))
        {
            this.CompleteCurrentUdpSession();
            this.StartNewUdpSession(realtimeUpdate, sessionType);
        }
        else if(this.currentSession == null
                && this.currentUdpPhase == SessionPhase.Session
                && sessionPhase == SessionPhase.Session)
        {
            this.StartNewUdpSession(realtimeUpdate, sessionType);
        }

        this.currentUdpSessionType = sessionType;
        this.currentUdpPhase = sessionPhase;
        this.currentSessionTime = realtimeUpdate.SessionTime;
    }

    private void OnNextSharedMemorySessionEnded(AceSharedMemorySession aceSharedMemorySession)
    {
        this.LogMessage(LoggingLevel.Information, aceSharedMemorySession.ToString());
        this.aceSharedMemorySession = null;
    }

    private void OnNextSharedMemorySessionStarted(AceSharedMemorySession aceSharedMemorySession)
    {
        this.LogMessage(LoggingLevel.Information, aceSharedMemorySession.ToString());
        this.aceSharedMemorySession = aceSharedMemorySession;

        var isNewEvent = this.currentEvent == null
                         || this.currentEvent.TrackName != aceSharedMemorySession.TrackName
                         || this.currentEvent.IsOnline != aceSharedMemorySession.IsOnline;

        if(!isNewEvent)
        {
            this.CompleteCurrentUdpSession();
            if(this.aceUdpConnection == null)
            {
                this.StartUdpConnection();
                this.StartDisconnectWatcher();
            }
            return;
        }

        this.EndCurrentEvent();
        this.StartNewEvent(aceSharedMemorySession);
    }

    private void OnNextTelemetryFrame(AceTelemetryFrame telemetryFrame)
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
            this.aceSharedMemoryConnection!.AppStatusChanges.Subscribe(this.OnNextAppStatusChange),
            this.aceSharedMemoryConnection.FlagState.Subscribe(this.OnNextFlagState),
            this.aceSharedMemoryConnection.LogMessages.Subscribe(m => this.logMessagesSubject.OnNext(m)),
            this.aceSharedMemoryConnection.SessionEnded.Subscribe(this.OnNextSharedMemorySessionEnded),
            this.aceSharedMemoryConnection.SessionStarted.Subscribe(this.OnNextSharedMemorySessionStarted),
            this.aceSharedMemoryConnection.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };
    }

    private void PrepareUdpMessageProcessing()
    {
        this.udpSubscriptionSink = new CompositeDisposable
        {
            this.aceUdpConnection!.LogMessages.Subscribe(m => this.logMessagesSubject.OnNext(m)),
            this.aceUdpConnection.BroadcastingEvents.Subscribe(this.OnNextBroadcastEvent),
            this.aceUdpConnection.EntryListUpdates.Subscribe(this.OnNextEntryListUpdate),
            this.aceUdpConnection.RealTimeUpdates.Subscribe(this.OnNextRealTimeUpdate),
            this.aceUdpConnection.RealTimeCarUpdates.Subscribe(this.OnNextRealTimeCarUpdate),
            this.aceUdpConnection.TrackDataUpdates.Subscribe(this.OnNextTrackDataUpdate)
        };
    }

    private void ProcessAccidentEvent(BroadcastingEvent broadcastingEvent)
    {
        if(this.currentSession == null)
        {
            return;
        }

        var carInfo = broadcastingEvent.CarData;
        var car = this.aceCarInfoProvider.FindByModelId(carInfo.CarModelType);
        var drivers = carInfo.Drivers.Select(d => new AceMonitorDriver(d.FirstName,
                                                 d.LastName,
                                                 d.ShortName,
                                                 d.Category.ToString(),
                                                 this.aceNationalityInfoProvider
                                                     .GetCountryCode(d.Nationality)))
                             .ToList();

        this.AddEntryIfNotExists(carInfo);

        var aceAccident = new AceMonitorAccident
        {
            AceCarModelId = carInfo.CarModelType,
            // Evo's cars.json has no separate manufacturer field - it's embedded in DisplayName
            // (e.g. "BMW M2 Coupe - Standard") but not cleanly parseable out.
            CarManufacturer = UnknownCarText,
            CarModelName = car?.DisplayName ?? UnknownCarText,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentMonitorDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            CarIndex = carInfo.CarIndex,
            RaceNumber = carInfo.RaceNumber,
            SessionId = this.currentSession.SessionId.ToString(),
            TeamName = carInfo.TeamName
        };

        this.accidentsSubject.OnNext(aceAccident);
    }

    private void ProcessBestPersonalLap(BroadcastingEvent broadcastingEvent)
    {
        if(this.currentSession == null)
        {
            return;
        }

        var carInfo = broadcastingEvent.CarData;
        var car = this.aceCarInfoProvider.FindByModelId(carInfo.CarModelType);
        var drivers = carInfo.Drivers.Select(d => new AceMonitorDriver(d.FirstName,
                                                 d.LastName,
                                                 d.ShortName,
                                                 d.Category.ToString(),
                                                 this.aceNationalityInfoProvider
                                                     .GetCountryCode(d.Nationality)))
                             .ToList();

        this.AddEntryIfNotExists(carInfo);

        var aceLap = new AceMonitorLap
        {
            AceCarModelId = carInfo.CarModelType,
            // Evo's cars.json has no separate manufacturer field - it's embedded in DisplayName
            // (e.g. "BMW M2 Coupe - Standard") but not cleanly parseable out.
            CarManufacturer = UnknownCarText,
            CarModelName = car?.DisplayName ?? UnknownCarText,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            CarIndex = carInfo.CarIndex,
            LapTime = broadcastingEvent.Message,
            RaceNumber = carInfo.RaceNumber,
            SessionId = this.currentSession.SessionId.ToString(),
            TeamName = carInfo.TeamName
        };

        this.personalBestLapSubject.OnNext(aceLap);
    }

    private void ProcessBestSessionLapEvent(BroadcastingEvent broadcastingEvent)
    {
        if(this.currentSession == null)
        {
            return;
        }

        var carInfo = broadcastingEvent.CarData;
        var car = this.aceCarInfoProvider.FindByModelId(carInfo.CarModelType);
        var drivers = carInfo.Drivers.Select(d => new AceMonitorDriver(d.FirstName,
                                                 d.LastName,
                                                 d.ShortName,
                                                 d.Category.ToString(),
                                                 this.aceNationalityInfoProvider
                                                     .GetCountryCode(d.Nationality)))
                             .ToList();

        this.AddEntryIfNotExists(carInfo);

        var aceLap = new AceMonitorLap
        {
            AceCarModelId = carInfo.CarModelType,
            // Evo's cars.json has no separate manufacturer field - it's embedded in DisplayName
            // (e.g. "BMW M2 Coupe - Standard") but not cleanly parseable out.
            CarManufacturer = UnknownCarText,
            CarModelName = car?.DisplayName ?? UnknownCarText,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            CarIndex = carInfo.CarIndex,
            LapTime = broadcastingEvent.Message,
            RaceNumber = carInfo.RaceNumber,
            SessionId = this.currentSession.SessionId.ToString(),
            TeamName = carInfo.TeamName
        };

        this.sessionBestLapSubject.OnNext(aceLap);
    }

    private void ProcessGreenFlagEvent(BroadcastingEvent broadcastingEvent)
    {
        this.greenFlagSubject.OnNext(new AceMonitorGreenFlag(this.currentSession?.Id));
    }

    private void ProcessLapCompletedEvent(BroadcastingEvent broadcastingEvent)
    {
        if(this.currentSession == null)
        {
            return;
        }

        var carInfo = broadcastingEvent.CarData;
        var car = this.aceCarInfoProvider.FindByModelId(carInfo.CarModelType);
        var drivers = carInfo.Drivers.Select(d => new AceMonitorDriver(d.FirstName,
                                                 d.LastName,
                                                 d.ShortName,
                                                 d.Category.ToString(),
                                                 this.aceNationalityInfoProvider
                                                     .GetCountryCode(d.Nationality)))
                             .ToList();

        this.AddEntryIfNotExists(carInfo);

        var aceLap = new AceMonitorLap
        {
            AceCarModelId = carInfo.CarModelType,
            // Evo's cars.json has no separate manufacturer field - it's embedded in DisplayName
            // (e.g. "BMW M2 Coupe - Standard") but not cleanly parseable out.
            CarManufacturer = UnknownCarText,
            CarModelName = car?.DisplayName ?? UnknownCarText,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            CarIndex = carInfo.CarIndex,
            LapTime = broadcastingEvent.Message,
            RaceNumber = carInfo.RaceNumber,
            SessionId = this.currentSession.SessionId.ToString(),
            TeamName = carInfo.TeamName
        };

        this.lapCompletedSubject.OnNext(aceLap);
    }

    private void ProcessPenaltyComMsgEvent(BroadcastingEvent broadcastingEvent)
    {
        if(this.currentSession == null)
        {
            return;
        }

        var carInfo = broadcastingEvent.CarData;
        var car = this.aceCarInfoProvider.FindByModelId(carInfo.CarModelType);
        var drivers = carInfo.Drivers.Select(d => new AceMonitorDriver(d.FirstName,
                                                 d.LastName,
                                                 d.ShortName,
                                                 d.Category.ToString(),
                                                 this.aceNationalityInfoProvider
                                                     .GetCountryCode(d.Nationality)))
                             .ToList();

        this.AddEntryIfNotExists(carInfo);

        var acePenalty = new AceMonitorPenalty
        {
            AceCarModelId = carInfo.CarModelType,
            // Evo's cars.json has no separate manufacturer field - it's embedded in DisplayName
            // (e.g. "BMW M2 Coupe - Standard") but not cleanly parseable out.
            CarManufacturer = UnknownCarText,
            CarModelName = car?.DisplayName ?? UnknownCarText,
            CarCupCategory = (CupCategory)carInfo.CupCategory,
            CurrentMonitorDriver = drivers[carInfo.CurrentDriverIndex],
            CurrentDriverIndex = carInfo.CurrentDriverIndex,
            Index = carInfo.CarIndex,
            Penalty = broadcastingEvent.Message,
            RaceNumber = carInfo.RaceNumber,
            SessionId = this.currentSession.SessionId.ToString(),
            TeamName = carInfo.TeamName
        };

        this.penaltiesSubject.OnNext(acePenalty);
    }

    private void ProcessSessionOverEvent(BroadcastingEvent broadcastingEvent)
    {
        if(this.currentSession == null)
        {
            return;
        }

        this.sessionCompletedSubject.OnNext(this.currentSession);
    }

    private void ProcessWhiteFlagState(AceFlagState aceFlagState)
    {
        var isActive = aceFlagState.Flag == AceFlagType.White || aceFlagState.GlobalFlag == AceFlagType.White;
        if(this.isWhiteFlagActive == isActive)
        {
            return;
        }

        this.isWhiteFlagActive = isActive;
        this.isWhiteFlagActiveSubject.OnNext(this.isWhiteFlagActive);
    }

    private void ProcessYellowFlagState(AceFlagState aceFlagState)
    {
        var isActive = aceFlagState.Flag == AceFlagType.Yellow || aceFlagState.GlobalFlag == AceFlagType.Yellow;
        if(this.isYellowFlagActive == isActive)
        {
            return;
        }

        this.isYellowFlagActive = isActive;
        this.isYellowFlagActiveSubject.OnNext(this.isYellowFlagActive);
    }

    private void StartDisconnectWatcher()
    {
        this.disconnectWatcherSubscription?.Dispose();
        this.lastRealtimeUpdate = DateTime.Now;
        this.disconnectWatcherSubscription = Observable.Interval(this.udpTimeoutThreshold)
                                                       .Subscribe(_ =>
                                                       {
                                                           if(this.aceUdpConnection == null
                                                              || this.currentAppStatus != AceStatus.Live)
                                                           {
                                                               return;
                                                           }

                                                           var timeSinceLastUpdate =
                                                               DateTime.Now - this.lastRealtimeUpdate;
                                                           if(timeSinceLastUpdate <= this.udpTimeoutThreshold)
                                                           {
                                                               return;
                                                           }

                                                           this.LogMessage(LoggingLevel.Information,
                                                               "UDP connection appears to have dropped, attempting reconnect.");
                                                           this.StopUdpConnection();
                                                           this.StartUdpConnection();
                                                       });
    }

    private void StartNewEvent(AceSharedMemorySession aceSharedMemorySession)
    {
        this.currentEvent = new AceMonitorEvent
        {
            EventId = Guid.NewGuid(),
            IsOnline = aceSharedMemorySession.IsOnline,
            IsRunning = true,
            NumberOfCars = (int)aceSharedMemorySession.NumberOfCars,
            TrackName = aceSharedMemorySession.TrackName
        };

        this.LogMessage(LoggingLevel.Information, $"Event Started: {this.currentEvent}");
        this.eventStartedSubject.OnNext(this.currentEvent);
        this.StartUdpConnection();
        this.StartDisconnectWatcher();
    }

    private void StartNewUdpSession(RealtimeUpdate realtimeUpdate, RaceSessionType sessionType)
    {
        if(this.aceSharedMemorySession == null || this.currentEvent == null)
        {
            this.LogMessage(LoggingLevel.Information,
                "Cannot start new session: shared memory session or current event is not available.");
            return;
        }

        this.currentSession = new AceMonitorSession
        {
            Duration = TimeSpan.FromMilliseconds(this.aceSharedMemorySession.DurationMs),
            EventId = this.currentEvent.EventId,
            IsOnline = this.currentEvent.IsOnline,
            IsRunning = true,
            NumberOfCars = this.currentEvent.NumberOfCars,
            SessionId = this.aceSharedMemorySession.SessionId,
            SessionType = sessionType.ToFriendlyName(),
            TrackName = this.aceSharedMemorySession.TrackName
        };

        this.sessionStartedSubject.OnNext(this.currentSession);
        this.LogMessage(LoggingLevel.Information, $"Session Started: {this.currentSession}");
    }

    private void StartSharedMemoryConnection()
    {
        this.LogMessage(LoggingLevel.Information, "Preparing connection to Ace Evo Shared Memory interface.");
        this.aceSharedMemoryConnection = this.aceSharedMemoryConnectionFactory.Create();
        this.PrepareSharedMemoryMessageProcessing();
        this.aceSharedMemoryConnection.Start();
    }

    private void StartUdpConnection()
    {
        this.LogMessage(LoggingLevel.Information, "Preparing connection to Ace Evo UDP interface.");
        var broadcastingSettings = this.aceLocalConfigProvider.GetBroadcastingSettings()!;
        this.aceUdpConnection = this.aceUdpConnectionFactory.Create(LocalhostIpAddress,
            broadcastingSettings.UdpListenerPort,
            this.connectionIdentifier ?? $"{LocalhostIpAddress}:{broadcastingSettings.UdpListenerPort}",
            broadcastingSettings.ConnectionPassword,
            broadcastingSettings.CommandPassword);

        this.PrepareUdpMessageProcessing();
        this.aceUdpConnection.Connect();
    }

    private void StopUdpConnection()
    {
        if(this.aceUdpConnection == null)
        {
            return;
        }

        this.CompleteCurrentUdpSession();
        this.udpSubscriptionSink?.Dispose();
        this.aceUdpConnection.Dispose();
        this.aceUdpConnection = null;
        this.entryList.Clear();
        this.entryListSubject.OnNext(this.entryList);
        this.currentUdpPhase = SessionPhase.NONE;
        this.currentUdpSessionType = RaceSessionType.NONE;
        this.currentSessionTime = TimeSpan.Zero;
    }
}
