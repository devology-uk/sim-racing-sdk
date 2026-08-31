/*
 * This demo shows how to use the LmuSharedMemoryConnection to read and process merged Telemetry+Scoring+Extended
 * data using the two rFactor2-API-family plugins (LMU_SharedMemoryMapPlugin64.dll and
 * rFactor2SharedMemoryMapPlugin64.dll) required for LMU shared memory support.
 */

using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Lmu.Core.Abstractions;
using SimRacingSdk.Lmu.Demo.Abstractions;
using SimRacingSdk.Lmu.SharedMemory.Abstractions;
using SimRacingSdk.Lmu.SharedMemory.Models;
using SimRacingSdk.Wpf.Shared.Logging;

namespace SimRacingSdk.Lmu.Demo.Demos;

public class SharedMemoryDemo : ISharedMemoryDemo
{
    private readonly IConsoleLog consoleLog;
    private readonly ILmuSharedMemoryConnectionFactory lmuSharedMemoryConnectionFactory;
    private readonly ILmuSharedMemoryPluginInstaller lmuSharedMemoryPluginInstaller;
    private readonly ILogger<SharedMemoryDemo> logger;
    private readonly IRfactor2SharedMemoryPluginInstaller rfactor2SharedMemoryPluginInstaller;
    private readonly ISharedMemoryLog sharedMemoryLog;

    private ILmuSharedMemoryConnection? lmuSharedMemoryConnection;
    private CompositeDisposable? subscriptionSink;
    private int telemetryFrameCount;

    public SharedMemoryDemo(ILogger<SharedMemoryDemo> logger,
        IConsoleLog consoleLog,
        ISharedMemoryLog sharedMemoryLog,
        ILmuSharedMemoryConnectionFactory lmuSharedMemoryConnectionFactory,
        ILmuSharedMemoryPluginInstaller lmuSharedMemoryPluginInstaller,
        IRfactor2SharedMemoryPluginInstaller rfactor2SharedMemoryPluginInstaller)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.sharedMemoryLog = sharedMemoryLog;
        this.lmuSharedMemoryConnectionFactory = lmuSharedMemoryConnectionFactory;
        this.lmuSharedMemoryPluginInstaller = lmuSharedMemoryPluginInstaller;
        this.rfactor2SharedMemoryPluginInstaller = rfactor2SharedMemoryPluginInstaller;
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

        this.Log("Checking LMU_SharedMemoryMapPlugin64.dll is installed and configured...");
        if(!this.lmuSharedMemoryPluginInstaller.IsInstalled)
        {
            this.Log("Installing LMU_SharedMemoryMapPlugin64.dll...");
            this.lmuSharedMemoryPluginInstaller.Install();
        }

        this.Log("Checking rFactor2SharedMemoryMapPlugin64.dll is installed and configured...");
        if(!this.rfactor2SharedMemoryPluginInstaller.IsInstalled)
        {
            this.Log("Installing rFactor2SharedMemoryMapPlugin64.dll...");
            this.rfactor2SharedMemoryPluginInstaller.Install();
        }

        if(!this.lmuSharedMemoryPluginInstaller.IsInstalled || !this.rfactor2SharedMemoryPluginInstaller.IsInstalled)
        {
            this.Log(
                "One or both plugins could not be installed/configured automatically. Please check CustomPluginVariables.JSON and the game's Plugins folder.",
                LogLevel.Warning);
            return false;
        }

        this.Log("Both required plugins are installed and configured. If LMU was already running, restart it so the plugins load.");
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
