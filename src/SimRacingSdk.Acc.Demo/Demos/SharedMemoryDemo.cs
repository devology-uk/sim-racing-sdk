using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Acc.Demo.Abstractions;
using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.Demo.Demos;

public class SharedMemoryDemo : ISharedMemoryDemo
{
    private readonly IAccSharedMemoryConnectionFactory accSharedMemoryConnectionFactory;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<SharedMemoryDemo> logger;
    private IAccSharedMemoryConnection? accSharedMemoryConnection;
    private CompositeDisposable? subscriptionSink;

    public SharedMemoryDemo(ILogger<SharedMemoryDemo> logger,
        IConsoleLog consoleLog,
        IAccSharedMemoryConnectionFactory accSharedMemoryConnectionFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.accSharedMemoryConnectionFactory = accSharedMemoryConnectionFactory;
    }

    public void Start()
    {
        this.Stop();

        this.Log("Starting Shared Memory Demo...");
        this.subscriptionSink = new CompositeDisposable();
        this.accSharedMemoryConnection = this.accSharedMemoryConnectionFactory.Create();
        this.subscriptionSink.Add(this.accSharedMemoryConnection.NewEvent.Subscribe(this.OnNextNewEvent));
        this.subscriptionSink.Add(this.accSharedMemoryConnection.NewLap.Subscribe(this.OnNextNewLap));
        this.subscriptionSink.Add(this.accSharedMemoryConnection.Frames.Subscribe(this.OnNextFrame));
        this.accSharedMemoryConnection.Start();
    }

    public void Stop()
    {
        this.Log("Stopping Shared Memory Demo...");
        this.subscriptionSink?.Dispose();
        this.accSharedMemoryConnection?.Dispose();
        this.accSharedMemoryConnection = null!;
    }

    public bool Validate()
    {
        this.Log("Validating Shared Memory Demo...");
        return true;
    }

    private void Log(string message, LogLevel logLevel = LogLevel.Information)
    {
        this.logger.Log(logLevel, message);
        this.consoleLog.Write(message);
    }

    private void OnNextFrame(AccTelemetryFrame accSharedMemoryFrame)
    {
        this.Log(accSharedMemoryFrame.ToString());
    }

    private void OnNextNewEvent(AccTelemetryEvent accSharedMemoryEvent)
    {
        this.Log(accSharedMemoryEvent.ToString());
    }

    private void OnNextNewLap(AccTelemetryLap accSharedMemoryLap)
    {
        this.Log(accSharedMemoryLap.ToString());
    }
}