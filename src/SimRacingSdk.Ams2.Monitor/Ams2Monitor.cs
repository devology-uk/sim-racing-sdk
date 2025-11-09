using SimRacingSdk.Acc.Core.Enums;
using SimRacingSdk.Ams2.Core.Abstractions;
using SimRacingSdk.Ams2.Monitor.Abstractions;
using SimRacingSdk.Ams2.SharedMemory.Abstractions;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SimRacingSdk.Core.Enums;
using SimRacingSdk.Core.Messages;

namespace SimRacingSdk.Ams2.Monitor;

public class Ams2Monitor : IAms2Monitor
{

    private readonly Subject<LogMessage> logMessagesSubject = new();

    private readonly IAms2CarInfoProvider accCarInfoProvider;
    private readonly IAms2CompatibilityChecker accCompatibilityChecker;
    private readonly IAms2SharedMemoryConnectionFactory accSharedMemoryConnectionFactory;

    public Ams2Monitor(IAms2SharedMemoryConnectionFactory accSharedMemoryConnectionFactory,
        IAms2CompatibilityChecker accCompatibilityChecker,
        IAms2CarInfoProvider accCarInfoProvider)
    {
        this.accCarInfoProvider = accCarInfoProvider;
        this.accCompatibilityChecker = accCompatibilityChecker;
        this.accSharedMemoryConnectionFactory = accSharedMemoryConnectionFactory;
    }

    public IObservable<LogMessage> LogMessages => this.logMessagesSubject.AsObservable();

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Start(string? connectionIdentifier = null)
    {
       
    }

    public void Stop()
    {
        
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!disposing)
        {
            return;
        }

        this.Stop();
    }

    private void LogMessage(LoggingLevel level, string content)
    {
        this.logMessagesSubject.OnNext(new LogMessage(level, content));
    }
}