using System.Reactive.Disposables;
using System.Reactive.Linq;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Lmu.Rest.Abstractions;
using SimRacingSdk.Lmu.Rest.Models;

namespace SimRacingSdk.Lmu.Demo.RestExplorer;

// One run of the REST monitor: starts the connection, records what it sees, and reports every change in game
// state, LMU's window state and API availability, so a rig run needs no clicking once it's started.
public sealed class RestMonitorRecording : IDisposable
{
    private static readonly TimeSpan WindowStatePollInterval = TimeSpan.FromSeconds(1);

    private readonly ILmuRestConnection connection;
    private readonly RestMonitorRecorder recorder;
    private readonly Action<string> report;
    private readonly object reportLock = new();
    private readonly CompositeDisposable subscriptions;
    private readonly LmuWindowStateReader windowStateReader;

    private string currentStateLabel = "Starting";
    private string? previousStatusDescription;
    private string? previousWindowState;

    public RestMonitorRecording(ILmuRestConnection connection,
        RestMonitorRecorder recorder,
        LmuWindowStateReader windowStateReader,
        Action<string> report)
    {
        this.connection = connection;
        this.recorder = recorder;
        this.windowStateReader = windowStateReader;
        this.report = report;
        this.subscriptions = new CompositeDisposable
        {
            connection.Availability.Subscribe(this.OnNextAvailability),
            connection.LogMessages.Subscribe(this.OnNextLogMessage),
            connection.Responses.Subscribe(this.OnNextResponse),
            connection.Status.Subscribe(this.OnNextStatus),
            connection.VehicleConditions.Subscribe(this.OnNextVehicleCondition),
            Observable.Interval(WindowStatePollInterval).Subscribe(this.OnNextWindowStateTick)
        };
    }

    public string CaptureFolderPath => this.recorder.CaptureFolderPath;

    public void Start()
    {
        this.Report($"REST monitor recording to {this.CaptureFolderPath}");
        this.connection.Start();
    }

    public void Dispose()
    {
        this.subscriptions.Dispose();
        this.connection.Dispose();
        this.Report("REST monitor stopped.");
    }

    private void OnNextAvailability(bool isAvailable)
    {
        this.Report($"API {(isAvailable ? "responding" : "NOT responding")} (window {this.windowStateReader.Read()})");
    }

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.Report(logMessage.Content);
    }

    private void OnNextResponse(LmuRestResponse response)
    {
        this.recorder.RecordResponse(response, this.currentStateLabel);
    }

    private void OnNextStatus(LmuRestStatus status)
    {
        var description = status.Describe();
        if(description == this.previousStatusDescription)
        {
            return;
        }

        this.previousStatusDescription = description;
        this.currentStateLabel = RestMonitorStateLabel.For(status);
        this.Report($"State: {this.currentStateLabel} ({description})");
    }

    private void OnNextVehicleCondition(LmuRestVehicleCondition vehicleCondition)
    {
        this.recorder.RecordEvent($"Vehicle condition: fuel {vehicleCondition.Fuel:0.0} of {vehicleCondition.FuelCapacity:0} L, "
                                  + $"tyres {string.Join(" ", vehicleCondition.TireCondition.Select(wear => wear.ToString("0.000")))}");
    }

    private void OnNextWindowStateTick(long tick)
    {
        var windowState = this.windowStateReader.Read();
        if(windowState == this.previousWindowState)
        {
            return;
        }

        this.previousWindowState = windowState;
        this.Report($"LMU window: {windowState}");
    }

    private void Report(string message)
    {
        lock(this.reportLock)
        {
            this.recorder.RecordEvent(message);
            this.report($"{DateTime.Now:HH:mm:ss} {message}");
        }
    }
}
