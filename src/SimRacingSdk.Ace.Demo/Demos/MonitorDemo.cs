/*
 * This demo provides an example of how to use the AceMonitor component, which combines the AceUdpConnection and AceSharedMemoryConnection components
 * to provide data objects an application could use.
 *
 * The demo simply logs entities but in a real application you might want to save these entities in a database
 * and display the data.
 *
 * Bear in mind that the UDP Broadcasting interface provides data for all drivers on track, while the Shared Memory interface only
 * provides data for the player car.  Therefore, telemetry can only be captured for laps completed by the current player on the computer running
 * an application that uses this monitoring component.
 *
 * Also bear in mind that neither of the interfaces provides a unique identifier for a driver, the only properties we can use to match laps from
 * the UDP interface with telemetry frames from the Shared Memory interface are the driver first name and last name, which may not be unique.
 *
 * Whilst driver names can be used to match laps with telemetry frames, this does not guarantee they can be mapped to a user profile in ACE.
 * The Account object provided by this SDK will give you the First and Last name the user has configured in ACE, but there is no guarantee that these names match
 * what is in the data because they can be overridden by Entry Lists when hosting events using a dedicated server.
 *
 * In our own applications where we use this SDK we have implemented a user interface that allows users to map driver names to their user profile.
 */

using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Ace.Core.Enums;
using SimRacingSdk.Ace.Demo.Abstractions;
using SimRacingSdk.Ace.Monitor.Abstractions;
using SimRacingSdk.Ace.Monitor.Messages;
using SimRacingSdk.Ace.SharedMemory.Models;
using SimRacingSdk.Ace.Udp.Messages;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ace.Demo.Demos;

public class MonitorDemo : IMonitorDemo
{
    private readonly IAceMonitorFactory aceMonitorFactory;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<MonitorDemo> logger;
    private readonly IMonitorLog monitorLog;
    private IAceMonitor? aceMonitor;
    private SessionPhase currentPhase;
    private CompositeDisposable? subscriptionSink;
    private int telemetryFrameCount;

    public MonitorDemo(ILogger<MonitorDemo> logger,
        IConsoleLog consoleLog,
        IMonitorLog monitorLog,
        IAceMonitorFactory aceMonitorFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.monitorLog = monitorLog;
        this.aceMonitorFactory = aceMonitorFactory;
    }

    public void Start()
    {
        this.Stop();
        this.Log("Starting ACE Monitor Demo...");
        this.aceMonitor = this.aceMonitorFactory.Create();

        this.subscriptionSink = new CompositeDisposable
        {
            this.aceMonitor.Accidents.Subscribe(this.OnNextAccident),
            this.aceMonitor.LapCompleted.Subscribe(this.OnNextCompletedLap),

            // usually you would only subscribe to one of the next two, but both are shown here for demonstration purposes
            this.aceMonitor.EntryList.Subscribe(this.OnNextEntryList),
            this.aceMonitor.Entries.Subscribe(this.OnNextEventEntry),

            this.aceMonitor.EventStarted.Subscribe(this.OnNextEventStarted),
            this.aceMonitor.EventEnded.Subscribe(this.OnNextEventEnded),
            this.aceMonitor.GreenFlag.Subscribe(this.OnNextGreenFlag),
            this.aceMonitor.LogMessages.Subscribe(this.OnNextLogMessage),
            this.aceMonitor.Penalties.Subscribe(this.OnNextPenalty),
            this.aceMonitor.PersonalBestLap.Subscribe(this.OnNextPersonalBestLap),
            this.aceMonitor.PhaseChanged.Subscribe(this.OnNextPhaseChanged),
            this.aceMonitor.RealtimeCarUpdates.Subscribe(this.OnNextRealtimeCarUpdate),
            this.aceMonitor.SessionBestLap.Subscribe(this.OnNextSessionBestLap),
            this.aceMonitor.SessionTypeChanged.Subscribe(this.OnNextSessionChanged),
            this.aceMonitor.SessionCompleted.Subscribe(this.OnNextSessionCompleted),
            this.aceMonitor.SessionStarted.Subscribe(this.OnNextSessionStarted),
            this.aceMonitor.IsWhiteFlagActive.Subscribe(this.OnNextIsWhiteFlagActive),
            this.aceMonitor.IsYellowFlagActive.Subscribe(this.OnNextIsYellowFlagActive),
            this.aceMonitor.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };

        this.aceMonitor.Start("ACE Monitor Demo");
    }

    private void OnNextSessionChanged(AceMonitorSessionTypeChange aceMonitorSessionTypeChange)
    {
        this.monitorLog.Log($"Session Changed: {aceMonitorSessionTypeChange}");
    }

    public void Stop()
    {
        if(this.aceMonitor == null)
        {
            return;
        }

        this.Log("Stopping ACE Monitor Demo...");
        this.Log($"Total Telemetry Frames: {this.telemetryFrameCount}");
        this.subscriptionSink?.Dispose();
        this.aceMonitor?.Dispose();
        this.aceMonitor = null;
    }

    public bool Validate()
    {
        return true;
    }

    private void Log(string message)
    {
        this.logger.LogInformation(message);
        this.consoleLog.Write(message);
    }

    private void OnNextEventStarted(AceMonitorEvent aceMonitorEvent)
    {
        this.Log($"Event Started: {aceMonitorEvent}");
    }

    private void OnNextEventEnded(AceMonitorEvent aceMonitorEvent)
    {
        this.Log($"Event Ended: {aceMonitorEvent}");
    }

    private void OnNextAccident(AceMonitorAccident monitorAccident)
    {
        this.Log(monitorAccident.ToString());
    }

    private void OnNextCompletedLap(AceMonitorLap aceMonitorLap)
    {
        this.Log(aceMonitorLap.ToString());
    }

    private void OnNextEntryList(IList<AceMonitorEntry> entryList)
    {
        this.Log("Entry List Updated:");
        foreach(var entry in entryList)
        {
            this.Log(entry.ToString());
        }
    }

    private void OnNextEventEntry(AceMonitorEntry aceMonitorEntry)
    {
        this.Log(aceMonitorEntry.ToString());
    }

    private void OnNextGreenFlag(AceMonitorGreenFlag aceMonitorGreenFlag)
    {
        this.Log(aceMonitorGreenFlag.ToString());
    }

    private void OnNextIsWhiteFlagActive(bool isWhiteFlagActive)
    {
        this.Log($"White Flag Is Active: {isWhiteFlagActive}");
    }

    private void OnNextIsYellowFlagActive(bool isYellowFlagActive)
    {
        this.Log($"Yellow Flag Is Active: {isYellowFlagActive}");
    }

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.monitorLog.Log(logMessage.ToString());
    }

    private void OnNextPenalty(AceMonitorPenalty aceMonitorPenalty)
    {
        this.Log(aceMonitorPenalty.ToString());
    }

    private void OnNextPersonalBestLap(AceMonitorLap aceMonitorLap)
    {
        this.Log($"Best Session Lap: {aceMonitorLap}");
    }

    private void OnNextPhaseChanged(AceMonitorSessionPhaseChange aceMonitorSessionPhaseChange)
    {
        this.Log($"Phase Changed: {aceMonitorSessionPhaseChange}");
        this.currentPhase = aceMonitorSessionPhaseChange.NewPhase;
    }

    private void OnNextRealtimeCarUpdate(RealtimeCarUpdate realtimeCarUpdate)
    {
        if(this.currentPhase != SessionPhase.Session && this.currentPhase != SessionPhase.SessionOver)
        {
            // filter out updates where the car is not actually on a meaningful lap
            // SessionOver is the phase where the game is waiting for all players to complete the last lap
            return;
        }

        this.Log(realtimeCarUpdate.ToString());
    }

    private void OnNextSessionBestLap(AceMonitorLap aceMonitorLap)
    {
        this.Log($"Best Personal Lap: {aceMonitorLap}");
    }

    private void OnNextSessionCompleted(AceMonitorSession aceMonitorSession)
    {
        // Session Over is produced by a broadcast event from ACE but
        // Not always dispatched so can't rely on it
        this.Log($"Session Completed: {aceMonitorSession}");
    }

    private void OnNextSessionStarted(AceMonitorSession aceMonitorSession)
    {
        this.Log($"Session Started: {aceMonitorSession}");
    }

    private void OnNextTelemetryFrame(AceTelemetryFrame aceTelemetryFrame)
    {
        // too much information to log telemetry frames, which are logged via log messages
        // just maintaining a count to report at the end
        this.telemetryFrameCount++;
    }
}
