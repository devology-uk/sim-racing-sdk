using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Acc.Demo.Abstractions;
using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.Demo.Demos;

public class TelemetryOnlyDemo : ITelemetryOnlyDemo
{
    private readonly IAccTelemetryConnectionFactory accTelemetryConnectionFactory;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<TelemetryOnlyDemo> logger;
    private IAccTelemetryConnection accTelemetryConnection = null!;
    private CompositeDisposable subscriptionSink = null!;

    public TelemetryOnlyDemo(ILogger<TelemetryOnlyDemo> logger,
        IConsoleLog consoleLog,
        IAccTelemetryConnectionFactory accTelemetryConnectionFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.accTelemetryConnectionFactory = accTelemetryConnectionFactory;
    }

    public void Start()
    {
        this.Stop();

        this.Log("Starting Telemetry Only Demo...");
        this.subscriptionSink = new CompositeDisposable();
        this.accTelemetryConnection = this.accTelemetryConnectionFactory.Create();
        this.PrepareTelemetryMessageProcessing();
        this.accTelemetryConnection.Start();
    }

    public void Stop()
    {
        this.Log("Stopping Telemetry Only Demo...");
        this.subscriptionSink?.Dispose();
        this.accTelemetryConnection?.Dispose();
        this.accTelemetryConnection = null!;
    }

    public bool Validate()
    {
        this.Log("Validating Telemetry Only Demo...");
        return true;
    }

    private void Log(string message, LogLevel logLevel = LogLevel.Information)
    {
        this.logger.Log(logLevel, message);
        this.consoleLog.Write(message);
    }

    private void OnNexFrame(AccTelemetryFrame accSharedMemoryFrame)
    {
        this.Log(accSharedMemoryFrame.ToString());
    }

    private void PrepareTelemetryMessageProcessing()
    {
        this.subscriptionSink.Add(this.accTelemetryConnection.Frames.Subscribe(this.OnNexFrame));
    }
}