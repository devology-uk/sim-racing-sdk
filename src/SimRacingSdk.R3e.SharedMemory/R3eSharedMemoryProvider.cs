using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Core.Services;
using SimRacingSdk.R3e.SharedMemory.Abstractions;
using SimRacingSdk.R3e.SharedMemory.Messages;
using SimRacingSdk.R3e.SharedMemory.Services;

namespace SimRacingSdk.R3e.SharedMemory;

public class R3eSharedMemoryProvider : IR3eSharedMemoryProvider
{
    private static R3eSharedMemoryProvider? singletonInstance;

    private readonly LogMessageBroker logMessageBroker = new(nameof(R3eSharedMemoryProvider));

    private bool? isLayoutCompatible;
    private string? lastLoggedFailureReason;
    private R3eSharedMemoryReader? sharedMemoryReader;

    public static R3eSharedMemoryProvider Instance => singletonInstance ??= new R3eSharedMemoryProvider();

    public IObservable<LogMessage> LogMessages => this.logMessageBroker.Messages;

    public void Dispose()
    {
        this.sharedMemoryReader?.Dispose();
        this.sharedMemoryReader = null;
        this.isLayoutCompatible = null;
        GC.SuppressFinalize(this);
    }

    public R3eShared? Read()
    {
        if(this.sharedMemoryReader is null && !this.TryOpenReader())
        {
            return null;
        }

        try
        {
            var data = this.sharedMemoryReader!.Read();
            if(!this.IsReadable(data))
            {
                return null;
            }

            this.ClearFailureReason();
            return data;
        }
        catch(Exception exception)
        {
            this.LogFailureReasonOnce(LoggingLevel.Error, $"Failed to read R3E shared memory: {exception.Message}");
            return null;
        }
    }

    private bool CheckLayout(R3eShared data)
    {
        var incompatibility = R3eSharedMemoryLayoutCheck.FindIncompatibility(data);
        if(incompatibility is not null)
        {
            this.logMessageBroker.Log(LoggingLevel.Error, incompatibility);
            return false;
        }

        var level = R3eSharedMemoryLayoutCheck.IsNewerMinorVersion(data)
                        ? LoggingLevel.Warning
                        : LoggingLevel.Information;
        this.logMessageBroker.Log(level,
            $"R3E shared memory API version {data.VersionMajor}.{data.VersionMinor}, layout matches the SDK.");
        return true;
    }

    private void ClearFailureReason()
    {
        this.lastLoggedFailureReason = null;
    }

    private bool IsReadable(R3eShared data)
    {
        if(data.VersionMajor == 0)
        {
            this.LogFailureReasonOnce(LoggingLevel.Information, "R3E shared memory exists but isn't written yet.");
            return false;
        }

        this.isLayoutCompatible ??= this.CheckLayout(data);
        return this.isLayoutCompatible.Value;
    }

    private void LogFailureReasonOnce(LoggingLevel level, string reason)
    {
        if(this.lastLoggedFailureReason == reason)
        {
            return;
        }

        this.lastLoggedFailureReason = reason;
        this.logMessageBroker.Log(level, reason);
    }

    private bool TryOpenReader()
    {
        this.sharedMemoryReader = R3eSharedMemoryReader.TryOpen(out var failureReason);
        if(this.sharedMemoryReader is not null)
        {
            return true;
        }

        this.LogFailureReasonOnce(LoggingLevel.Warning, $"R3E shared memory not available yet: {failureReason}");
        return false;
    }
}
