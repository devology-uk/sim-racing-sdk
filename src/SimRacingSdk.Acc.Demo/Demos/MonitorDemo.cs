/*
 * This demo provides an example of how to use the AccMonitor component, which combines the AccUdpConnection and AccTelemetryConnection components
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
 * Whilst driver names can be used to match laps with telemetry frames, this does not guarantee they can be mapped to a user profile in ACC.
 * The Account object provided by this SDK will give you the First and Last name the user has configured in ACC, but there is no guarantee that these names match
 * what is in the data because they can be overridden by Entry Lists when hosting events using ACC Server.
 *
 * In our own applications where we use this SDK we have implemented a user interface that allows users to map driver names to their user profile.
 */

using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Acc.Demo.Abstractions;
using SimRacingSdk.Acc.Monitor.Abstractions;
using SimRacingSdk.Acc.Monitor.Messages;
using SimRacingSdk.Acc.SharedMemory.Models;
using SimRacingSdk.Acc.Udp.Messages;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Acc.Demo.Demos;

public class MonitorDemo : IMonitorDemo
{
    private readonly IAccMonitorFactory accMonitorFactory;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<MonitorDemo> logger;
    private readonly IMonitorLog monitorLog;
    private IAccMonitor? accMonitor;
    private SessionPhase currentPhase;
    private CompositeDisposable? subscriptionSink;
    private int telemetryFrameCount;

    public MonitorDemo(ILogger<MonitorDemo> logger,
        IConsoleLog consoleLog,
        IMonitorLog monitorLog,
        IAccMonitorFactory accMonitorFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.monitorLog = monitorLog;
        this.accMonitorFactory = accMonitorFactory;
    }

    public void Start()
    {
        this.Stop();
        this.Log("Starting ACC Monitor Demo...");
        this.accMonitor = this.accMonitorFactory.Create();

        this.subscriptionSink = new CompositeDisposable
        {
            this.accMonitor.Accidents.Subscribe(this.OnNextAccident),
            this.accMonitor.LapCompleted.Subscribe(this.OnNextCompletedLap),

            // usually you would only subscribe to one of the next two, but both are shown here for demonstration purposes
            this.accMonitor.EntryList.Subscribe(this.OnNextEntryList),
            this.accMonitor.Entries.Subscribe(this.OnNextEventEntry),

            this.accMonitor.GreenFlag.Subscribe(this.OnNextGreenFlag),
            this.accMonitor.LogMessages.Subscribe(this.OnNextLogMessage),
            this.accMonitor.Penalties.Subscribe(this.OnNextPenalty),
            this.accMonitor.PersonalBestLap.Subscribe(this.OnNextPersonalBestLap),
            this.accMonitor.PhaseChanged.Subscribe(this.OnNextPhaseChanged),
            this.accMonitor.RealtimeCarUpdates.Subscribe(this.OnNextRealtimeCarUpdate),
            this.accMonitor.SessionBestLap.Subscribe(this.OnNextSessionBestLap),
            this.accMonitor.SessionTypeChanged.Subscribe(this.OnNextSessionChanged),
            this.accMonitor.SessionCompleted.Subscribe(this.OnNextSessionCompleted),
            this.accMonitor.SessionStarted.Subscribe(this.OnNextSessionStarted),
            this.accMonitor.IsWhiteFlagActive.Subscribe(this.OnNextIsWhiteFlagActive),
            this.accMonitor.IsYellowFlagActive.Subscribe(this.OnNextIsYellowFlagActive),
            this.accMonitor.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };

        this.accMonitor.Start("ACC Monitor Demo");
    }

    private void OnNextSessionChanged(AccMonitorSessionTypeChange accMonitorSessionTypeChange)
    {
        this.monitorLog.Log($"Session Changed: {accMonitorSessionTypeChange}");
    }

    public void Stop()
    {
        if(this.accMonitor == null)
        {
            return;
        }

        this.Log("Stopping ACC Monitor Demo...");
        this.Log($"Total Telemetry Frames: {this.telemetryFrameCount}");
        this.subscriptionSink?.Dispose();
        this.accMonitor?.Dispose();
        this.accMonitor = null;
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

    private void OnNextAccident(AccMonitorAccident monitorAccident)
    {
        this.Log(monitorAccident.ToString());
    }

    private void OnNextCompletedLap(AccMonitorLap accMonitorLap)
    {
        this.Log(accMonitorLap.ToString());
    }

    private void OnNextEntryList(IList<AccMonitorEntry> entryList)
    {
        this.Log("Entry List Updated:");
        foreach(var entry in entryList)
        {
            this.Log(entry.ToString());
        }
    }

    private void OnNextEventEntry(AccMonitorEntry accMonitorEntry)
    {
        this.Log(accMonitorEntry.ToString());
    }

    private void OnNextGreenFlag(AccMonitorGreenFlag accMonitorGreenFlag)
    {
        this.Log(accMonitorGreenFlag.ToString());
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

    private void OnNextPenalty(AccMonitorPenalty accMonitorPenalty)
    {
        this.Log(accMonitorPenalty.ToString());
    }

    private void OnNextPersonalBestLap(AccMonitorLap accMonitorLap)
    {
        this.Log($"Best Session Lap: {accMonitorLap}");
    }

    private void OnNextPhaseChanged(AccMonitorSessionPhaseChange accMonitorSessionPhaseChange)
    {
        this.Log($"Phase Changed: {accMonitorSessionPhaseChange}");
        this.currentPhase = accMonitorSessionPhaseChange.NewPhase;
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

    private void OnNextSessionBestLap(AccMonitorLap accMonitorLap)
    {
        this.Log($"Best Personal Lap: {accMonitorLap}");
    }

    private void OnNextSessionCompleted(AccMonitorSession accMonitorSession)
    {
        // Session Over is produced by a broadcast event from ACC but
        // Not always dispatched so can't rely on it
        this.Log($"Session Completed: {accMonitorSession}");
    }

    private void OnNextSessionStarted(AccMonitorSession accMonitorSession)
    {
        this.Log($"Session Started: {accMonitorSession}");
    }

    private void OnNextTelemetryFrame(AccTelemetryFrame accTelemetryFrame)
    {
        // too much information to log telemetry frames, which are logged via log messages
        // just maintaining a count to report at the end
        this.telemetryFrameCount++;
    }
}