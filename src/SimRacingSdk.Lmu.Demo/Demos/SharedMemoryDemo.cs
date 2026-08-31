/*
 * This demo shows how to use LmuSharedMemoryConnection to read LMU's native shared memory interface
 * (Studio 397's own SharedMemoryInterface.hpp) - no plugin install required, just "Enable Plugins" turned on in
 * the game's own Settings -> Gameplay screen.
 */

using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Lmu.Demo.Abstractions;
using SimRacingSdk.Lmu.SharedMemory.Abstractions;
using SimRacingSdk.Lmu.SharedMemory.Models;
using SimRacingSdk.Wpf.Shared.Logging;

namespace SimRacingSdk.Lmu.Demo.Demos;

public class SharedMemoryDemo : ISharedMemoryDemo
{
    private readonly IConsoleLog consoleLog;
    private readonly ILmuSharedMemoryConnectionFactory lmuSharedMemoryConnectionFactory;
    private readonly ILogger<SharedMemoryDemo> logger;
    private readonly ISharedMemoryLog sharedMemoryLog;

    private ILmuSharedMemoryConnection? lmuSharedMemoryConnection;
    private CompositeDisposable? subscriptionSink;
    private int telemetryFrameCount;

    public SharedMemoryDemo(ILogger<SharedMemoryDemo> logger,
        IConsoleLog consoleLog,
        ISharedMemoryLog sharedMemoryLog,
        ILmuSharedMemoryConnectionFactory lmuSharedMemoryConnectionFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.sharedMemoryLog = sharedMemoryLog;
        this.lmuSharedMemoryConnectionFactory = lmuSharedMemoryConnectionFactory;
    }

    public void Start()
    {
        this.Stop();

        this.telemetryFrameCount = 0;
        this.Log("Starting Shared Memory Demo...");
        this.lmuSharedMemoryConnection = this.lmuSharedMemoryConnectionFactory.Create();
        this.subscriptionSink = new CompositeDisposable
        {
            this.lmuSharedMemoryConnection.LogMessages.Subscribe(this.OnNextLogMessage),
            this.lmuSharedMemoryConnection.Telemetry.Subscribe(this.OnNextTelemetryFrame)
        };
        this.lmuSharedMemoryConnection.Start();
    }

    public void Stop()
    {
        if(this.lmuSharedMemoryConnection == null)
        {
            return;
        }

        this.Log("Stopping Shared Memory Demo...");
        this.Log($"Total Telemetry Frames: {this.telemetryFrameCount}");
        this.subscriptionSink?.Dispose();
        this.lmuSharedMemoryConnection?.Dispose();
        this.lmuSharedMemoryConnection = null;
    }

    public bool Validate()
    {
        this.Log("Validating Shared Memory Demo...");
        this.Log(
            "LMU's native shared memory needs no plugin - just confirm \"Enable Plugins\" is on in the game's own Settings -> Gameplay screen.");
        return true;
    }

    private void Log(string message, LogLevel logLevel = LogLevel.Information)
    {
        this.logger.Log(logLevel, message);
        this.consoleLog.Write(message);
    }

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.sharedMemoryLog.Log(logMessage);
    }

    private void OnNextTelemetryFrame(LmuTelemetryFrame lmuTelemetryFrame)
    {
        // Frames arrive as often as every 20ms - too much to log individually, so just keep a running count and
        // surface the occasional frame so it's visible the data is actually flowing.
        this.telemetryFrameCount++;
        if(this.telemetryFrameCount % 250 == 0)
        {
            this.Log(lmuTelemetryFrame.ToString());
        }
    }
}
