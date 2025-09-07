/*
 * This demo shows how to use the AccSharedMemoryConnection to read and process data
 * using the Shared Memory interface provided by ACC.
 */

using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Acc.Core.Messages;
using SimRacingSdk.Acc.Demo.Abstractions;
using SimRacingSdk.Acc.SharedMemory.Abstractions;
using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.Demo.Demos;

public class SharedMemoryDemo : ISharedMemoryDemo
{
    private readonly IAccSharedMemoryConnectionFactory accSharedMemoryConnectionFactory;
    private readonly IConsoleLog consoleLog;
    private readonly ILogger<SharedMemoryDemo> logger;
    private readonly ISharedMemoryLog sharedMemoryLog;
    private IAccSharedMemoryConnection? accSharedMemoryConnection;
    private CompositeDisposable? subscriptionSink;

    public SharedMemoryDemo(ILogger<SharedMemoryDemo> logger,
        IConsoleLog consoleLog,
        ISharedMemoryLog sharedMemoryLog,
        IAccSharedMemoryConnectionFactory accSharedMemoryConnectionFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.sharedMemoryLog = sharedMemoryLog;
        this.accSharedMemoryConnectionFactory = accSharedMemoryConnectionFactory;
    }

    public void Start()
    {
        this.Stop();

        this.Log("Starting Shared Memory Demo...");
        this.accSharedMemoryConnection = this.accSharedMemoryConnectionFactory.Create();
        this.subscriptionSink = new CompositeDisposable
        {
            this.accSharedMemoryConnection.LogMessages.Subscribe(this.OnNextLogMessage),
            this.accSharedMemoryConnection.NewEvent.Subscribe(this.OnNextNewEvent),
            this.accSharedMemoryConnection.NewLap.Subscribe(this.OnNextNewLap),
            this.accSharedMemoryConnection.Telemetry.Subscribe(this.OnNextFrame)
        };
        this.accSharedMemoryConnection.Start();
    }

    public void Stop()
    {
        if(this.accSharedMemoryConnection == null)
        {
            return;
        }

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

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.sharedMemoryLog.Log(logMessage.ToString());
    }

    private void OnNextNewEvent(AccSharedMemoryEvent accSharedMemoryEvent)
    {
        this.Log(accSharedMemoryEvent.ToString());
    }

    private void OnNextNewLap(AccSharedMemoryLap accSharedMemoryLap)
    {
        this.Log(accSharedMemoryLap.ToString());
    }
}