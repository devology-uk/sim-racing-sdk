using System.Reactive.Disposables;
using Microsoft.Extensions.Logging;
using SimRacingSdk.Ams2.Core.Abstractions;
using SimRacingSdk.Ams2.Demo.Abstractions;
using SimRacingSdk.Ams2.SharedMemory.Abstractions;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ams2.Demo.Demos;

public class SharedMemoryDemo : ISharedMemoryDemo
{
    private readonly IAms2CompatibilityChecker ams2CompatibilityChecker;
    private readonly IAms2SharedMemoryConnectionFactory ams2SharedMemoryConnectionFactory;
    private readonly IConsoleLog consoleLog;
    private readonly ISharedMemoryLog sharedMemoryLog;
    private readonly ILogger<ISharedMemoryDemo> logger;

    private IAms2SharedMemoryConnection? ams2SharedMemoryConnection;
    private CompositeDisposable subscriptionSink = null!;

    public SharedMemoryDemo(ILogger<ISharedMemoryDemo> logger,
        IConsoleLog consoleLog,
        ISharedMemoryLog sharedMemoryLog,
        IAms2CompatibilityChecker ams2CompatibilityChecker,
        IAms2SharedMemoryConnectionFactory ams2SharedMemoryConnectionFactory)
    {
        this.logger = logger;
        this.consoleLog = consoleLog;
        this.sharedMemoryLog = sharedMemoryLog;
        this.ams2CompatibilityChecker = ams2CompatibilityChecker;
        this.ams2SharedMemoryConnectionFactory = ams2SharedMemoryConnectionFactory;
    }

    public void Start()
    {
        this.Stop();
        this.Log("Starting Shared Memory Demo...");

        this.ams2SharedMemoryConnection = this.ams2SharedMemoryConnectionFactory.Create();
        this.PrepareMessageHandling();
        this.ams2SharedMemoryConnection.Start();
    }

    public void Stop()
    {
        if(this.ams2SharedMemoryConnection == null)
        {
            return;
        }

        this.Log("Stopping Shared Memory Demo...");
        this.subscriptionSink?.Dispose();
        this.ams2SharedMemoryConnection?.Dispose();
        this.ams2SharedMemoryConnection = null;
    }

    public bool Validate()
    {
        return this.ams2CompatibilityChecker.IsAms2Installed();
    }

    private void Log(string message)
    {
        this.logger.LogInformation(message);
        this.consoleLog.Write(message);
    }

    private void OnNextLogMessage(LogMessage logMessage)
    {
        this.sharedMemoryLog.Log(logMessage.ToString());
    }

    private void PrepareMessageHandling()
    {
        if(this.ams2SharedMemoryConnection == null)
        {
            return;
        }

        this.subscriptionSink = new CompositeDisposable
        {
            this.ams2SharedMemoryConnection.LogMessages.Subscribe(this.OnNextLogMessage)
        };
    }
}