using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;
using SimRacingSdk.Core.Services;
using SimRacingSdk.Lmu.SharedMemory.Abstractions;
using SimRacingSdk.Lmu.SharedMemory.Messages;

namespace SimRacingSdk.Lmu.SharedMemory;

// Both the mapped file and its lock are per-machine, per-game-instance OS resources - not something that makes
// sense to open once per Connection - so this is shared as a singleton and lazily (re)opens each on first use, or
// after either fails to open, since the game may not be running yet when a consumer starts polling.
public class LmuSharedMemoryProvider : ILmuSharedMemoryProvider
{
    private static LmuSharedMemoryProvider? singletonInstance;

    private readonly LogMessageBroker logMessageBroker = new(nameof(LmuSharedMemoryProvider));

    private string? lastLoggedFailureReason;
    private LmuSharedMemoryLock? sharedMemoryLock;
    private LmuSharedMemoryReader? sharedMemoryReader;

    public static LmuSharedMemoryProvider Instance => singletonInstance ??= new LmuSharedMemoryProvider();

    public IObservable<LogMessage> LogMessages => this.logMessageBroker.Messages;

    public void Dispose()
    {
        this.sharedMemoryReader?.Dispose();
        this.sharedMemoryReader = null;
        this.sharedMemoryLock?.Dispose();
        this.sharedMemoryLock = null;
        GC.SuppressFinalize(this);
    }

    public LmuSharedMemoryObjectOut? Read()
    {
        if(this.sharedMemoryReader is null && !this.TryOpenReader())
        {
            return null;
        }

        if(this.sharedMemoryLock is null && !this.TryOpenLock())
        {
            return null;
        }

        try
        {
            var data = this.sharedMemoryReader!.Read(this.sharedMemoryLock!);
            this.ClearFailureReason();
            return data;
        }
        catch(TimeoutException exception)
        {
            this.LogFailureReasonOnce(exception.Message);
            return null;
        }
    }

    private void ClearFailureReason()
    {
        this.lastLoggedFailureReason = null;
    }

    private void LogFailureReasonOnce(string reason)
    {
        if(this.lastLoggedFailureReason == reason)
        {
            return;
        }

        this.lastLoggedFailureReason = reason;
        this.logMessageBroker.Log(LoggingLevel.Warning, reason);
    }

    private bool TryOpenLock()
    {
        this.sharedMemoryLock = LmuSharedMemoryLock.TryOpen(out var failureReason);
        if(this.sharedMemoryLock is not null)
        {
            return true;
        }

        this.LogFailureReasonOnce($"LMU shared memory lock not available yet: {failureReason}");
        return false;
    }

    private bool TryOpenReader()
    {
        this.sharedMemoryReader = LmuSharedMemoryReader.TryOpen(out var failureReason);
        if(this.sharedMemoryReader is not null)
        {
            return true;
        }

        this.LogFailureReasonOnce($"LMU shared memory not available yet: {failureReason}");
        return false;
    }
}
